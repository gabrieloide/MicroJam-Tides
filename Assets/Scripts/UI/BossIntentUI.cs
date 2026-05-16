using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class BossIntentUI : MonoBehaviour
{
    [Header("UI Components")]
    [SerializeField] private TMP_Text intentValueText;
    [SerializeField] private Image intentIcon;

    [Header("Icons")]
    [SerializeField] private Sprite lightAttackIcon;
    [SerializeField] private Sprite heavyAttackIcon;
    [SerializeField] private Sprite restIcon;

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
        }

        if (intentIcon != null && iconToUse != null)
        {
            intentIcon.sprite = iconToUse;
        }
    }
}
