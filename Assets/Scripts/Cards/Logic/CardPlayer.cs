using UnityEngine;
using System.Collections.Generic;

public class CardPlayer : MonoBehaviour
{
    public static CardPlayer Instance { get; private set; }
    [SerializeField] private LifeValue playerLifeValue;
    [SerializeField] private int initialHealth = 100;

    private List<Card> hand = new List<Card>();
    private int cardsPlayedThisTurn;
    [SerializeField] private Transform handPosition;
    [SerializeField] private GameObject cardUiPrefab;
    [SerializeField] private GameObject card3dPrefab;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;

        if (playerLifeValue != null)
        {
            playerLifeValue.Initialize(initialHealth);
        }
    }

    public void TakeDamage(int amount)
    {
        if (playerLifeValue != null)
        {
            playerLifeValue.ModifyValue(-amount);
            Debug.Log($"Player took {amount} damage. Current health: {playerLifeValue.Value}");

            if (playerLifeValue.Value <= 0)
            {
                Debug.Log("Player Dead!");
            }
        }
    }

    private void OnEnable()
    {
        Deck.OnInitializeDeck += DrawCards;
    }

    private void OnDisable()
    {
        Deck.OnInitializeDeck -= DrawCards;
    }

    public void DrawCards(int drawAmount)
    {
        for (int i = 0; i < drawAmount; i++)
        {
            if (Deck.Instance.DrawStack.Count > 0)
            {
                Card newCard = Deck.Instance.DrawStack.Pop();
                hand.Add(newCard);
                CreateCardInUi(newCard.data.Sprite, newCard);
            }
            else
            {
                Debug.Log("Deck is empty");
                break;
            }
        }
    }

    public void DiscardHand()
    {
        // Logic to clear UI and list
        foreach (Transform child in handPosition)
        {
            Destroy(child.gameObject);
        }

        hand.Clear();
    }

    public void PlayCard(CardDisplay display, Card card)
    {
        if (hand.Contains(card))
        {
            card.Play();
            hand.Remove(card);
            Destroy(display.gameObject);
            Instantiate(card3dPrefab);
            Debug.Log($"Card {card.data.CardName} played and removed from hand.");
        }
    }

    private void CreateCardInUi(Sprite sprite, Card card)
    {
        var c = Instantiate(cardUiPrefab, handPosition);
        c.transform.SetParent(handPosition);
        c.transform.localScale = Vector3.one;
        c.transform.localPosition = Vector3.zero;
        c.GetComponent<CardDisplay>().Setup(sprite, card);
    }
}