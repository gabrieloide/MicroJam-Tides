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

    public int NextTurn() => CurrentTurn = (CurrentTurn + 1) % 3;

    public int GetTurn() => CurrentTurn;

    private void TurnChange()
    {
        OnTurnChange?.Invoke();
        switch (CurrentTurn)
        {
            case 0:
                Debug.Log("Turno del jugador");
                break;
            case 1:
                Debug.Log("Turno del jefe");
                break;
            case 2:
                Debug.Log("Turno de limpieza");
                break;
        }
    }
}