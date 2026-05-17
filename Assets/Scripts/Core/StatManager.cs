using System;
using UnityEngine;
using Code.Scripts.Audio;

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
        if (amount < 0 && AudioManager.Instance != null)
        {
            AudioManager.Instance.PlaySFX("SFX_Stat_Degrade");
            if (CardPlayer.Instance != null && FloatingTextManager.Instance != null) 
            { 
                // Spawn degradation lower down (hips)
                FloatingTextManager.Instance.Show(CardPlayer.Instance.transform.position + Vector3.up * 0.7f, $"{amount} STRENGTH!", new Color(0.8f, 0.2f, 0.8f)); 
            }
        }
        else if (amount > 0 && CardPlayer.Instance != null && FloatingTextManager.Instance != null)
        {
            // Spawn strength gain in the middle (chest)
            FloatingTextManager.Instance.Show(CardPlayer.Instance.transform.position + Vector3.up * 1.1f, $"+{amount} STRENGTH", Color.yellow);
        }
        OnStatChanged?.Invoke();
    }

    public void ModifyShieldStat(int amount)
    {
        currentShieldStat += amount;
        currentShieldStat = Mathf.Max(0, currentShieldStat);
        if (amount < 0 && AudioManager.Instance != null)
        {
            AudioManager.Instance.PlaySFX("SFX_Stat_Degrade");
            if (CardPlayer.Instance != null && FloatingTextManager.Instance != null) 
            { 
                // Spawn max shield degradation lower down (hips)
                FloatingTextManager.Instance.Show(CardPlayer.Instance.transform.position + Vector3.up * 0.7f, $"{amount} MAX SHIELD!", new Color(0.8f, 0.2f, 0.8f)); 
            }
        }
        else if (amount > 0 && CardPlayer.Instance != null && FloatingTextManager.Instance != null)
        {
            // Spawn max shield gain higher up (head/sky)
            FloatingTextManager.Instance.Show(CardPlayer.Instance.transform.position + Vector3.up * 1.5f, $"+{amount} MAX SHIELD", Color.cyan);
        }
        OnStatChanged?.Invoke();
    }

    public void ModifyMaxHandSize(int amount)
    {
        currentMaxHandSize += amount;
        currentMaxHandSize = Mathf.Max(0, currentMaxHandSize);
        if (amount < 0 && AudioManager.Instance != null)
        {
            AudioManager.Instance.PlaySFX("SFX_Stat_Degrade");
        }
        OnStatChanged?.Invoke();
    }

    public void AddActiveShield(int amount)
    {
        activeShield += amount;
        if (amount > 0 && CardPlayer.Instance != null && FloatingTextManager.Instance != null)
        {
            // Spawn active shield gain higher up (head/sky)
            FloatingTextManager.Instance.Show(CardPlayer.Instance.transform.position + Vector3.up * 1.5f, $"+{amount} SHIELD", Color.cyan);
        }
        OnStatChanged?.Invoke();
    }

    public void ResetTurnShield()
    {
        activeShield = 0;
        OnStatChanged?.Invoke();
    }
}