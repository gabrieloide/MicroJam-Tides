using UnityEngine;
using System;
using DG.Tweening;

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

        transform.DOShakePosition(0.4f, new Vector3(0.5f, 0f, 0f), 20, 90, false, true);
        
        if (bossLifeValue.Value <= 0)
        {
            OnBossDeath?.Invoke();
            transform.DOScale(0, 1f).SetEase(Ease.InBack);
        }
    }

    public int GetHealth() => bossLifeValue != null ? bossLifeValue.Value : 0;
}
