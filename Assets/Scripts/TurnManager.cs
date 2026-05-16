using System;
using UnityEngine;

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

    public void NextTurn()
    {
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
                if (CardPlayer.Instance != null)
                {
                    CardPlayer.Instance.ResolvePlayedCards();
                }
                
                if (BossAI.Instance != null)
                {
                    BossAI.Instance.ExecuteIntent();
                }

                Invoke(nameof(NextTurn), 1.5f);
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
}