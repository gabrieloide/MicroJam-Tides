using UnityEngine;
using TMPro;

public class StatsDisplay : MonoBehaviour
{
    [Header("UI Text References")]
    [SerializeField] private TMP_Text strengthText;
    [SerializeField] private TMP_Text shieldText;
    [SerializeField] private TMP_Text handSizeText;
    [SerializeField] private TMP_Text activeShieldText;
    [SerializeField] private TMP_Text handCountText;
    [SerializeField] private TMP_Text deckCountText;

    private void OnEnable()
    {
        StatManager.OnStatChanged += UpdateStatsUI;
    }

    private void OnDisable()
    {
        StatManager.OnStatChanged -= UpdateStatsUI;
    }

    private void Start()
    {
        UpdateStatsUI();
    }

    public void UpdateStatsUI()
    {
        if (StatManager.Instance == null) return;

        if (strengthText != null) 
            strengthText.text = StatManager.Instance.currentStrength.ToString();

        if (shieldText != null) 
            shieldText.text = StatManager.Instance.currentShieldStat.ToString();

        if (handSizeText != null) 
            handSizeText.text = StatManager.Instance.currentMaxHandSize.ToString();

        if (activeShieldText != null)
            activeShieldText.text = StatManager.Instance.activeShield.ToString();

        if (handCountText != null && CardPlayer.Instance != null)
            handCountText.text = CardPlayer.Instance.GetHandCount().ToString();

        if (deckCountText != null && Deck.Instance != null)
            deckCountText.text = Deck.Instance.DrawStack.Count.ToString();
    }
}
