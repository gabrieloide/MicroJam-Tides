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
    [SerializeField] private Image healthFillImage;
    [SerializeField] private Image ghostFillImage;
    [SerializeField] private TMP_Text healthText;
    [SerializeField] private RectTransform healthContainer;

    private float maxHealth = 100f;

    private void Start()
    {
        if (Boss.Instance != null)
        {
            Boss.Instance.OnBossTakeDamage += UpdateHealthUI;
            
            // Set max health
            maxHealth = Boss.Instance.GetHealth();
            
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
        float targetFill = Mathf.Clamp01(currentHealth / maxHealth);
        
        // 1. Animate main health fill quickly
        if (healthFillImage != null)
        {
            healthFillImage.DOKill();
            healthFillImage.DOFillAmount(targetFill, 0.25f).SetEase(Ease.OutQuad);
        }

        // 2. Animate trailing ghost fill slowly after a short delay
        if (ghostFillImage != null)
        {
            ghostFillImage.DOKill();
            ghostFillImage.DOFillAmount(targetFill, 0.75f).SetDelay(0.4f).SetEase(Ease.OutCubic);
        }

        // 3. Add a juicy squash/stretch impact effect to the whole container
        if (healthContainer != null)
        {
            healthContainer.DOKill();
            healthContainer.localScale = Vector3.one;
            healthContainer.DOPunchScale(new Vector3(0.12f, -0.08f, 0f), 0.35f, 10, 1f);
        }
        
        if (healthText != null)
        {
            healthText.text = $"{currentHealth}/{maxHealth}";
        }
    }
}
