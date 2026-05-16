using UnityEngine;
using System.Collections.Generic;

public class CardPlayer : MonoBehaviour
{
    public static CardPlayer Instance { get; private set; }
    [SerializeField] private LifeValue playerLifeValue;

    private List<Card> hand = new List<Card>();
    private List<Card> playedCardsThisTurnList = new List<Card>();
    private List<GameObject> played3DCards = new List<GameObject>();
    private int cardsPlayedThisTurn;
    [SerializeField] private Transform handPosition;
    [SerializeField] private GameObject cardUiPrefab;
    [SerializeField] private GameObject card3dPrefab;

    public int GetHandCount() => hand.Count;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
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
            if (Deck.Instance.DrawStack.Count == 0)
            {
                Deck.Instance.RefillDeckFromDiscard();
            }

            if (Deck.Instance.DrawStack.Count > 0)
            {
                Card newCard = Deck.Instance.DrawStack.Pop();
                hand.Add(newCard);
                CreateCardInUi(newCard.data.Sprite, newCard);
            }
            else
            {
                Debug.Log("Deck is empty and no cards to refill");
                break;
            }
        }
    }

    public void DiscardHand()
    {
        foreach (Transform child in handPosition)
        {
            Destroy(child.gameObject);
        }

        foreach (var card in hand)
        {
            Deck.Instance.DiscardCard(card);
        }
        hand.Clear();
    }

    public void PlayCard(CardDisplay display, Card card)
    {
        if (hand.Contains(card) && cardsPlayedThisTurn < 3)
        {
            hand.Remove(card);
            Destroy(display.gameObject);

            Vector3 pos = CardPlacement.Instance.GetPlayerPlayPosition(cardsPlayedThisTurn);
            GameObject card3D = Instantiate(card3dPrefab, pos, Quaternion.identity);

            playedCardsThisTurnList.Add(card);
            played3DCards.Add(card3D);
            cardsPlayedThisTurn++;

            Debug.Log($"Card {card.data.CardName} placed on board.");
        }
        else if (cardsPlayedThisTurn >= 3)
        {
            Debug.Log("Limit of 3 cards per turn reached.");
        }
    }

    public void ResolvePlayedCards()
    {
        Debug.Log("Resolving played cards...");
        foreach (var card in playedCardsThisTurnList)
        {
            card.Play();
            Deck.Instance.DiscardCard(card);
        }

        playedCardsThisTurnList.Clear();
        cardsPlayedThisTurn = 0;

        foreach (var obj in played3DCards)
        {
            Destroy(obj);
        }

        played3DCards.Clear();
    }

    public int GetShieldCardsCount()
    {
        int count = 0;
        foreach (var card in hand)
        {
            if (card.data is ShieldCardData)
            {
                count++;
            }
        }

        return count;
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