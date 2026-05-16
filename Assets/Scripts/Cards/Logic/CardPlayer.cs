using UnityEngine;
using System.Collections.Generic;

public class CardPlayer : MonoBehaviour
{
    public static CardPlayer Instance { get; private set; }
    private LifeValue playerLifeValue;

    private List<Card> hand = new List<Card>();
    private int cardsPlayedThisTurn;
    [SerializeField] private Transform handPosition;
    [SerializeField] private GameObject cardPrefab;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
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

    private void CreateCardInUi(Sprite sprite, Card card)
    {
        var c = Instantiate(cardPrefab, handPosition);
        c.transform.SetParent(handPosition);
        c.transform.localScale = Vector3.one;
        c.transform.localPosition = Vector3.zero;
        c.GetComponent<CardDisplay>().Setup(sprite, card);
    }
}