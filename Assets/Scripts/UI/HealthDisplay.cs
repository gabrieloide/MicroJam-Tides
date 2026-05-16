using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;

public class HealthDisplay : MonoBehaviour
{
    [Header("Data Source")]
    [SerializeField] private LifeValue lifeData;

    [Header("UI Components")]
    [SerializeField] private TMP_Text healthText;
    [SerializeField] private Slider healthSlider;

    private void OnEnable()
    {
        if (lifeData != null)
        {
            lifeData.OnValueChanged += UpdateHealthUI;
            UpdateHealthUI();
        }
    }

    private void OnDisable()
    {
        if (lifeData != null)
        {
            lifeData.OnValueChanged -= UpdateHealthUI;
        }
    }

    private void UpdateHealthUI()
    {
        if (lifeData == null) return;

        if (healthText != null && healthText.text != lifeData.Value.ToString())
        {
            healthText.text = lifeData.Value.ToString();
            healthText.transform.DOPunchScale(Vector3.one * 0.2f, 0.3f);
        }

        if (healthSlider != null && healthSlider.value != lifeData.Value)
        {
            healthSlider.DOValue(lifeData.Value, 0.5f).SetEase(Ease.OutCubic);
        }
    }
}
