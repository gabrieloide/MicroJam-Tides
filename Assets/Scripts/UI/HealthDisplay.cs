using UnityEngine;
using UnityEngine.UI;
using TMPro;

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

        if (healthText != null)
        {
            healthText.text = lifeData.Value.ToString();
        }

        if (healthSlider != null)
        {
            healthSlider.value = lifeData.Value;
        }
    }
}
