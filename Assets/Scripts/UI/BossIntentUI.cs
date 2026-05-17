using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;

public class BossIntentUI : MonoBehaviour
{
    [Header("UI Components")]
    [SerializeField] private TMP_Text intentValueText;
    [SerializeField] private Image intentIcon;
    [SerializeField] private TMP_FontAsset intentFont;

    [Header("Icons")]
    [SerializeField] private Sprite lightAttackIcon;
    [SerializeField] private Sprite heavyAttackIcon;
    [SerializeField] private Sprite restIcon;

    [Header("Health UI")]
    [SerializeField] private Slider healthSlider;
    [SerializeField] private TMP_Text healthText;

    private void Start()
    {
        if (Boss.Instance != null)
        {
            Boss.Instance.OnBossTakeDamage += UpdateHealthUI;
            
            // Set slider max value to initial health (usually 100)
            if (healthSlider != null) healthSlider.maxValue = Boss.Instance.GetHealth();
            
            UpdateHealthUI();
        }
    }

    private void OnDestroy()
    {
        if (Boss.Instance != null)
        {
            Boss.Instance.OnBossTakeDamage -= UpdateHealthUI;
        }
    }

    private void OnEnable()
    {
        BossAI.OnIntentDecided += UpdateIntentUI;
    }

    private void OnDisable()
    {
        BossAI.OnIntentDecided -= UpdateIntentUI;
    }

    private void UpdateIntentUI(EnemyIntent intent)
    {
        int damageValue = 0;
        Sprite iconToUse = null;

        switch (intent)
        {
            case EnemyIntent.LightAttack:
                damageValue = 6;
                iconToUse = lightAttackIcon;
                break;
            case EnemyIntent.HeavyAttack:
                damageValue = 12;
                iconToUse = heavyAttackIcon;
                break;
            case EnemyIntent.Rest:
                damageValue = 0;
                iconToUse = restIcon;
                break;
        }

        if (intentValueText != null)
        {
            intentValueText.text = damageValue > 0 ? damageValue.ToString() : "Zzz";
            if (intentFont != null) intentValueText.font = intentFont;
        }

        if (intentIcon != null && iconToUse != null)
        {
            if (intentIcon.sprite != iconToUse)
            {
                intentIcon.sprite = iconToUse;
                intentIcon.transform.DOPunchScale(Vector3.one * 0.3f, 0.4f, 10, 1);
            }
        }
    }

    private void UpdateHealthUI()
    {
        if (Boss.Instance == null) return;

        int currentHealth = Boss.Instance.GetHealth();
        
        if (healthSlider != null)
        {
            healthSlider.DOValue(currentHealth, 0.3f).SetEase(Ease.OutCubic);
        }
        
        if (healthText != null)
        {
            float maxHealth = healthSlider != null ? healthSlider.maxValue : 100;
            healthText.text = $"{currentHealth}/{maxHealth}";
        }
    }
}
