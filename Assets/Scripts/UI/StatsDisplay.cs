using UnityEngine;
using TMPro;
using DG.Tweening;

public class StatsDisplay : MonoBehaviour
{
    [Header("UI Text References")]
    [SerializeField] private TMP_Text strengthText;
    [SerializeField] private TMP_Text shieldText;
    [SerializeField] private TMP_Text handSizeText;
    [SerializeField] private TMP_Text activeShieldText;
    [SerializeField] private TMP_Text handCountText;
    [SerializeField] private TMP_Text deckCountText;
    
    [Header("Fonts")]
    [SerializeField] private TMP_FontAsset pixelFont;

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

        if (strengthText != null && strengthText.text != StatManager.Instance.currentStrength.ToString())
        {
            strengthText.text = StatManager.Instance.currentStrength.ToString();
            strengthText.transform.DOPunchScale(Vector3.one * 0.2f, 0.3f);
        }

        if (shieldText != null && shieldText.text != StatManager.Instance.currentShieldStat.ToString())
        {
            shieldText.text = StatManager.Instance.currentShieldStat.ToString();
            shieldText.transform.DOPunchScale(Vector3.one * 0.2f, 0.3f);
        }

        if (handSizeText != null && handSizeText.text != StatManager.Instance.currentMaxHandSize.ToString())
        {
            handSizeText.text = StatManager.Instance.currentMaxHandSize.ToString();
            handSizeText.transform.DOPunchScale(Vector3.one * 0.2f, 0.3f);
        }

        if (activeShieldText != null && activeShieldText.text != StatManager.Instance.activeShield.ToString())
        {
            activeShieldText.text = StatManager.Instance.activeShield.ToString();
            activeShieldText.transform.DOPunchScale(Vector3.one * 0.2f, 0.3f);
        }

        if (handCountText != null && CardPlayer.Instance != null && handCountText.text != CardPlayer.Instance.GetHandCount().ToString())
        {
            handCountText.text = CardPlayer.Instance.GetHandCount().ToString();
            handCountText.transform.DOPunchScale(Vector3.one * 0.2f, 0.3f);
        }

        if (deckCountText != null && Deck.Instance != null && deckCountText.text != Deck.Instance.DrawStack.Count.ToString())
        {
            deckCountText.text = Deck.Instance.DrawStack.Count.ToString();
            deckCountText.transform.DOPunchScale(Vector3.one * 0.2f, 0.3f);
        }

        if (pixelFont != null) ApplyFont();
    }

    private void ApplyFont()
    {
        if (strengthText != null) strengthText.font = pixelFont;
        if (shieldText != null) shieldText.font = pixelFont;
        if (handSizeText != null) handSizeText.font = pixelFont;
        if (activeShieldText != null) activeShieldText.font = pixelFont;
        if (handCountText != null) handCountText.font = pixelFont;
        if (deckCountText != null) deckCountText.font = pixelFont;
    }
}
