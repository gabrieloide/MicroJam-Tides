using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CardDisplay : MonoBehaviour
{
    [SerializeField] private Image _image;
    [SerializeField] private TMP_Text _valueText;
    private Card card;

    private void OnEnable()
    {
        StatManager.OnStatChanged += UpdateCardValue;
    }

    private void OnCardPlayed()
    {
        CardPlayer.Instance.PlayCard(this, card);
    }

    private void OnDisable()
    {
        StatManager.OnStatChanged -= UpdateCardValue;
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
        GetComponent<Button>().onClick.AddListener(OnCardPlayed);
        UpdateCardValue();
    }
}