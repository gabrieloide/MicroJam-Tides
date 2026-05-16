using System;
using UnityEngine;
using System.Collections.Generic;
using Random = UnityEngine.Random;

public class Deck : MonoBehaviour
{
    [Header("Deck Info")] [SerializeField] public List<CardData> _cardTypes = new List<CardData>();
    private const int STARTING_HAND_SIZE = 5;
    private List<Card> _actualDeck = new List<Card>();
    public Stack<Card> DrawStack = new Stack<Card>();
    private List<Card> _discardPile = new List<Card>();

    public static Action<int> OnInitializeDeck;
    public static Deck Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    private void Start()
    {
        foreach (var cardType in _cardTypes)
        {
            for (int j = 0; j < cardType.Amount; j++)
            {
                Card card = new Card(cardType);
                _actualDeck.Add(card);
                Debug.Log(cardType.name);
            }
        }

        ShuffleDeck();
        OnInitializeDeck?.Invoke(STARTING_HAND_SIZE);
        Debug.Log(_actualDeck.Count);
    }

    public void ShuffleDeck()
    {
        for (int i = 0; i < _actualDeck.Count; i++)
        {
            Card temp = _actualDeck[i];
            int randomIndex = Random.Range(i, _actualDeck.Count);
            _actualDeck[i] = _actualDeck[randomIndex];
            _actualDeck[randomIndex] = temp;
            DrawStack.Push(_actualDeck[i]);
        }

        Debug.Log("Deck shuffled.");
    }

    public void DiscardCard(Card card)
    {
        _discardPile.Add(card);
    }

    public void RefillDeckFromDiscard()
    {
        if (_discardPile.Count == 0) return;

        Debug.Log("Refilling deck from discard pile...");
        _actualDeck.Clear();
        _actualDeck.AddRange(_discardPile);
        _discardPile.Clear();
        ShuffleDeck();
    }
}