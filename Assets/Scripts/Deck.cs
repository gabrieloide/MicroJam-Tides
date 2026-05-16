using System;
using UnityEngine;
using System.Collections.Generic;
using Random = UnityEngine.Random;

public class Deck : MonoBehaviour
{
    [Header("Deck Info")] [SerializeField] private List<CardData> _cardTypes = new List<CardData>();
    private const int STARTING_HAND_SIZE = 5;
    private List<Card> _actualDeck = new List<Card>();
    public Stack<Card> DrawStack = new Stack<Card>();
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

        OnInitializeDeck?.Invoke(STARTING_HAND_SIZE);
        ShuffleDeck();
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
}