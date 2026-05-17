using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;
using DG.Tweening;

public class CardDisplay : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler
{
    [SerializeField] private Image _image;
    [SerializeField] private TMP_Text _valueText;
    [SerializeField] private TMP_FontAsset _fontAsset;
    [SerializeField] private Image _badgeImage;
    [SerializeField] private float hoverYOffset = 30f;
    [SerializeField] private RectTransform visualContainer;
    private Card card;
    
    private Vector3 originalScale;
    private RectTransform targetRect;

    private void Awake()
    {
        targetRect = visualContainer != null ? visualContainer : GetComponent<RectTransform>();
        originalScale = targetRect.localScale;
    }

    private void OnEnable()
    {
        StatManager.OnStatChanged += UpdateCardValue;
    }

    private void OnDisable()
    {
        StatManager.OnStatChanged -= UpdateCardValue;
        targetRect.DOKill();
    }

    private void OnCardPlayed()
    {
        targetRect.DOKill();
        CardPlayer.Instance.PlayCard(this, card);
    }

    private void UpdateCardValue()
    {
        if (card == null || card.data == null) return;

        int displayValue = 0;
        Color themeColor = Color.white;

        if (card.data is AttackCardData attack)
        {
            displayValue = StatManager.Instance.currentStrength + attack.bonusDamage;
            ColorUtility.TryParseHtmlString("#794100", out themeColor);
        }
        else if (card.data is ShieldCardData shield)
        {
            displayValue = StatManager.Instance.currentShieldStat + shield.bonusShield;
            ColorUtility.TryParseHtmlString("#61A2FF", out themeColor);
        }
        else
        {
            displayValue = card.data.Cost;
            ColorUtility.TryParseHtmlString("#51A200", out themeColor);
        }

        if (_valueText != null)
        {
            _valueText.text = displayValue.ToString();
            if (_fontAsset != null) _valueText.font = _fontAsset;
        }

        if (_badgeImage != null)
        {
            _badgeImage.color = themeColor;
        }
    }

    public void Setup(Sprite sprite, Card card)
    {
        if (_image != null)
        {
            _image.sprite = sprite;
        }

        this.card = card;
        
        targetRect.localScale = Vector3.zero;
        targetRect.DOScale(Vector3.one, 0.4f).SetEase(Ease.OutBack);
        
        UpdateCardValue();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        targetRect.DOScale(originalScale * 1.15f, 0.2f).SetEase(Ease.OutBack);
        targetRect.DOLocalMoveY(hoverYOffset, 0.2f).SetEase(Ease.OutCirc);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        targetRect.DOScale(originalScale, 0.2f).SetEase(Ease.OutQuad);
        targetRect.DOLocalMoveY(0f, 0.2f).SetEase(Ease.OutQuad);
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        targetRect.DOPunchScale(new Vector3(-0.1f, -0.1f, 0), 0.15f).OnComplete(OnCardPlayed);
    }
}