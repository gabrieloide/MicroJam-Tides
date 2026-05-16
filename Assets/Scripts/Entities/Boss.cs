using UnityEngine;
using System;

public class Boss : MonoBehaviour
{
    [SerializeField] private LifeValue bossLifeValue;

    public static Boss Instance { get; private set; }
    public Action OnBossDeath;
    public Action OnBossTakeDamage;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    public void TakeDamage(int damage)
    {
        bossLifeValue.ModifyValue(-damage);
        OnBossTakeDamage?.Invoke();
        if (bossLifeValue.Value <= 0)
        {
            OnBossDeath?.Invoke();
        }
    }

    public int GetHealth() => bossLifeValue != null ? bossLifeValue.Value : 0;
}
