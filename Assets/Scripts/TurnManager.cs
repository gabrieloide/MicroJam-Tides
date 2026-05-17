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

        // Self-heal AudioManager: Load & bind AudioDatabase dynamically if unassigned
        if (AudioManager.Instance != null)
        {
            var field = typeof(AudioManager).GetField("_database", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            if (field != null && field.GetValue(AudioManager.Instance) == null)
            {
                var db = Resources.Load<AudioDatabase>("Audio/AudioDatabase");
                if (db != null)
                {
                    field.SetValue(AudioManager.Instance, db);
                    Debug.Log("[Audio Self-Heal] Successfully loaded and bound AudioDatabase to AudioManager dynamically!");
                }
                else
                {
                    Debug.LogError("[Audio Self-Heal] Failed to find AudioDatabase in Resources/Audio/AudioDatabase!");
                }
            }
        }
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

        // Resolve Player's cards effects (Shield applied before attack)
        if (CardPlayer.Instance != null)
        {
            CardPlayer.Instance.ResolvePlayedCardEffects();
        }

        // Wait a bit before boss attacks
        yield return new WaitForSeconds(0.8f);

        // Execute Boss attack while cards are still visually on the board
        if (BossAI.Instance != null)
        {
            BossAI.Instance.ExecuteIntent();
        }

        // Wait for the attack animation and damage feedback
        yield return new WaitForSeconds(2.0f);
        
        // Clear the 3D cards from the board after the attack finishes
        if (CardPlayer.Instance != null)
        {
            CardPlayer.Instance.ClearPlayedCardsVisuals();
        }

        NextTurn();
    }
}