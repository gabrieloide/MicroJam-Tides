using System;
using System.Collections;
using UnityEngine;
using Code.Scripts.Audio;

public class TurnManager : MonoBehaviour
{
    public static TurnManager Instance { get; private set; }
    private int CurrentTurn = 0;
    public static Action OnTurnChange;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    private void Start()
    {
        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.PlayMusic("Beach_Theme");
        }
    }

    public void NextTurn()
    {
        if (CurrentTurn == 0 && AudioManager.Instance != null)
        {
            AudioManager.Instance.PlaySFX("SFX_Turn_End_Click");
        }
        CurrentTurn = (CurrentTurn + 1) % 3;
        TurnChange();
    }

    public int GetTurn() => CurrentTurn;

    private void TurnChange()
    {
        OnTurnChange?.Invoke();
        switch (CurrentTurn)
        {
            case 0:
                Debug.Log("Player's Turn");
                if (AudioManager.Instance != null)
                {
                    AudioManager.Instance.PlaySFX("SFX_Player_Turn_Start");
                }
                if (BossAI.Instance != null && StatManager.Instance != null && CardPlayer.Instance != null)
                {
                    BossAI.Instance.DecideIntent(
                        StatManager.Instance.currentShieldStat, 
                        CardPlayer.Instance.GetShieldCardsCount()
                    );
                }
                break;
            case 1:
                Debug.Log("Boss's Turn");
                StartCoroutine(BossTurnRoutine());
                break;
            case 2:
                Debug.Log("Cleanup Turn");
                if (CardPlayer.Instance != null && StatManager.Instance != null)
                {
                    int currentHand = CardPlayer.Instance.GetHandCount();
                    int maxHand = StatManager.Instance.currentMaxHandSize;
                    int amountToDraw = Mathf.Max(0, maxHand - currentHand);

                    if (amountToDraw > 0)
                    {
                        CardPlayer.Instance.DrawCards(amountToDraw);
                    }
                }

                if (StatManager.Instance != null)
                {
                    StatManager.Instance.ResetTurnShield();
                }

                Invoke(nameof(NextTurn), 0.5f);
                break;
        }
    }

    private IEnumerator BossTurnRoutine()
    {
        // Wait for ENEMY TURN notification to be clearly visible
        yield return new WaitForSeconds(1.5f);

        // Resolve Player's cards
        if (CardPlayer.Instance != null)
        {
            CardPlayer.Instance.ResolvePlayedCards();
        }

        // Wait a bit for card visual resolution (shake/shrink)
        yield return new WaitForSeconds(0.8f);

        // Execute Boss attack
        if (BossAI.Instance != null)
        {
            BossAI.Instance.ExecuteIntent();
        }

        // Wait for the attack animation and damage feedback
        yield return new WaitForSeconds(2.0f);

        NextTurn();
    }
}