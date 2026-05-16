using System;
using UnityEngine;

public class StatManager : MonoBehaviour
{
    public static StatManager Instance { get; private set; }

    [Header("Stats")] public int currentStrength = 10;
    public int currentShieldStat = 10;
    public int currentMaxHandSize = 5;

    [Header("Card References")] public CardData attackCardReference;
    public CardData shieldCardReference;

    [Header("Current Turn Status")] public int activeShield = 0;

    public static Action OnStatChanged;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    public void ModifyStrength(int amount)
    {
        currentStrength += amount;
        currentStrength = Mathf.Max(0, currentStrength);
        OnStatChanged?.Invoke();
    }

    public void ModifyShieldStat(int amount)
    {
        currentShieldStat += amount;
        currentShieldStat = Mathf.Max(0, currentShieldStat);
        OnStatChanged?.Invoke();
    }

    public void ModifyMaxHandSize(int amount)
    {
        currentMaxHandSize += amount;
        currentMaxHandSize = Mathf.Max(0, currentMaxHandSize);
        OnStatChanged?.Invoke();
    }

    public void AddActiveShield(int amount)
    {
        activeShield += amount;
        OnStatChanged?.Invoke();
    }

    public void ResetTurnShield()
    {
        activeShield = 0;
        OnStatChanged?.Invoke();
    }
}