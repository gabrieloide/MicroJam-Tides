using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;
using DG.Tweening;

public class CardDisplay : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler
{
    [SerializeField] private Image _image;
    [SerializeField] private TMP_Text _valueText;
    private Card card;
    
    private Vector3 originalScale;
    private Vector3 originalPosition;
    private RectTransform rectTransform;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
    }

    private void Start()
    {
        originalScale = rectTransform.localScale;
        originalPosition = rectTransform.anchoredPosition;
    }

    private void OnEnable()
    {
        StatManager.OnStatChanged += UpdateCardValue;
    }

    private void OnDisable()
    {
        StatManager.OnStatChanged -= UpdateCardValue;
        rectTransform.DOKill();
    }

    private void OnCardPlayed()
    {
        rectTransform.DOKill();
        CardPlayer.Instance.PlayCard(this, card);
    }

    private void UpdateCardValue()
    {
        if (card == null || card.data == null) return;

        int displayValue = 0;
        if (card.data is AttackCardData attack)
            displayValue = StatManager.Instance.currentStrength + attack.bonusDamage;
        else if (card.data is ShieldCardData shield)
            displayValue = StatManager.Instance.currentShieldStat + shield.bonusShield;
        else
            displayValue = card.data.Cost;


        if (_valueText != null)
        {
            _valueText.text = displayValue.ToString();
        }
    }

    public void Setup(Sprite sprite, Card card)
    {
        if (_image != null)
        {
            _image.sprite = sprite;
        }

        this.card = card;
        
        // Animación de entrada suave
        rectTransform.localScale = Vector3.zero;
        rectTransform.DOScale(Vector3.one, 0.4f).SetEase(Ease.OutBack);
        
        UpdateCardValue();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        rectTransform.DOScale(originalScale * 1.15f, 0.2f).SetEase(Ease.OutBack);
        rectTransform.DOAnchorPosY(originalPosition.y + 30f, 0.2f).SetEase(Ease.OutCirc);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        rectTransform.DOScale(originalScale, 0.2f).SetEase(Ease.OutQuad);
        rectTransform.DOAnchorPosY(originalPosition.y, 0.2f).SetEase(Ease.OutQuad);
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        rectTransform.DOPunchScale(new Vector3(-0.1f, -0.1f, 0), 0.15f).OnComplete(OnCardPlayed);
    }
}