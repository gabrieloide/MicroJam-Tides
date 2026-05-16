using UnityEngine;
using UnityEngine.UI;

public class CardDisplay : MonoBehaviour
{
    [SerializeField] private Image _image;
    [SerializeField] private Text _text;
    private int value;
    private Card card;

    private void OnEnable()
    {
        StatManager.OnStatChanged += UpdateCardValue;
    }

    private void OnCardPlayed()
    {
        card.Play();
    }

    private void OnDisable()
    {
        StatManager.OnStatChanged -= UpdateCardValue;
    }

    private void UpdateCardValue()
    {
        _text.text = value.ToString();
    }

    public void Setup(Sprite sprite, Card card)
    {
        _image.sprite = sprite;
        this.card = card;
        GetComponent<Button>().onClick.AddListener(OnCardPlayed);
    }
}