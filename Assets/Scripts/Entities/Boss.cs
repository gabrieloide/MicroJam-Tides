using UnityEngine;
using System;
using DG.Tweening;
using Code.Scripts.Audio;

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

    private void Start()
    {
        if (bossLifeValue != null)
        {
            bossLifeValue.Initialize(100);
        }
    }

    public void TakeDamage(int damage)
    {
        bossLifeValue.ModifyValue(-damage);
        OnBossTakeDamage?.Invoke();

        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.PlaySFX("SFX_Damage_Boss");
        }

        transform.DOShakePosition(0.4f, new Vector3(0.5f, 0f, 0f), 20, 90, false, true);
        
        if (bossLifeValue.Value <= 0)
        {
            OnBossDeath?.Invoke();
            if (AudioManager.Instance != null)
            {
                AudioManager.Instance.PlaySFX("SFX_Victory");
                AudioManager.Instance.StopMusic();
            }
            transform.DOScale(0, 1f).SetEase(Ease.InBack);
        }
    }

    public int GetHealth() => bossLifeValue != null ? bossLifeValue.Value : 0;
}
