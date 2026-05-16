using UnityEngine;
using System.Collections.Generic;

public class CardPlayer : MonoBehaviour
{
    private LifeValue playerLifeValue;

    private List<Card> hand;
    private int cardsPlayedThisTurn;
    [SerializeField] private Transform handPosition;
    [SerializeField] private GameObject cardPrefab;

    private void OnEnable()
    {
        Deck.OnInitializeDeck += DrawCards;
    }

    private void OnDisable()
    {
        Deck.OnInitializeDeck -= DrawCards;
    }

    private void DrawCards(int drawAmount)
    {
        Debug.Log("Drawing initial cards.");
        for (int i = 0; i < drawAmount; i++)
        {
            if (Deck.Instance.DrawStack.Count > 0) hand.Add(Deck.Instance.DrawStack.Pop());
            else
            {
                Debug.Log("Deck is empty");
                break;
            }

            CreateCardInUi(hand[i].data.Sprite, hand[i]);
        }
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