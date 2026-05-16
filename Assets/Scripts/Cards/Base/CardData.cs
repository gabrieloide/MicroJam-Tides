using UnityEngine;

public enum CardType
{
    Attack,
    Shield,
    Utility
}

public abstract class CardData : ScriptableObject, ICardEffect
{
    public string CardName;
    public int Cost;
    public int Amount; // Cantidad de esta carta en el mazo
    public Sprite Sprite;

    public abstract void ExecuteEffect(Card cardInstance);
}

[System.Serializable]
public class Card
{
    public CardData data;
    public int currentStat;

    public Card(CardData data)
    {
        this.data = data;
    }

    public void Play()
    {
        Debug.Log("Playing card: " + data.CardName);
        data.ExecuteEffect(this);
    }
}