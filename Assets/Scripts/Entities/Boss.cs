using UnityEngine;
using System;
using DG.Tweening;
using Code.Scripts.Audio;

public class Boss : MonoBehaviour
{
    [SerializeField] private LifeValue bossLifeValue;

    public static Boss Instance { get; private set; }
    public static Action OnBossDeath;
    public Action OnBossTakeDamage;

    private Vector3 initialPosition;
    private bool isDead = false;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;

        initialPosition = transform.localPosition;

        if (bossLifeValue != null)
        {
            bossLifeValue.Initialize(115);
        }
    }

    public void TakeDamage(int damage)
    {
        if (isDead) return;

        // TACTICAL MECHANIC: Rest gives 50% damage reduction (Shell Armor)
        if (BossAI.Instance != null && BossAI.Instance.CurrentIntent == EnemyIntent.Rest)
        {
            int originalDamage = damage;
            damage = Mathf.Max(1, damage / 2);
            int mitigated = originalDamage - damage;

            if (FloatingTextManager.Instance != null)
            {
                FloatingTextManager.Instance.Show(transform.position + Vector3.up * 2f, $"SHELL SHIELD! (-{mitigated} damage)", new Color(0.3f, 0.7f, 0.9f));
            }
        }

        bossLifeValue.ModifyValue(-damage);
        OnBossTakeDamage?.Invoke();

        if (FloatingTextManager.Instance != null)
        {
            FloatingTextManager.Instance.Show(transform.position + Vector3.up * 1f, $"-{damage}", Color.white);
        }

        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.PlaySFX("SFX_Damage_Boss");
        }

        // Prevents the drift bug by killing previous shakes and resetting to initial position
        transform.DOKill();
        transform.localPosition = initialPosition;
        transform.DOShakePosition(0.4f, new Vector3(0.5f, 0f, 0f), 20, 90, false, true);
        
        if (bossLifeValue.Value <= 0)
        {
            isDead = true;
            OnBossDeath?.Invoke();
            if (AudioManager.Instance != null)
            {
                AudioManager.Instance.PlaySFX("SFX_Victory");
                AudioManager.Instance.StopMusic();
            }
            
            if (GameNotificationManager.Instance != null)
            {
                GameNotificationManager.Instance.ShowVictory();
            }

            transform.DOScale(0, 1f).SetEase(Ease.InBack);
        }
    }

    public int GetHealth() => bossLifeValue != null ? bossLifeValue.Value : 0;
}
