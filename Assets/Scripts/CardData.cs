using UnityEngine;

public enum CardType
{
    Attack,
    Shield,
    Utility
}

[CreateAssetMenu(fileName = "NewCard", menuName = "Card")]
public class CardData : ScriptableObject
{
    public string CardName;
    public int Cost;
    public CardType Type;
    public int Amount;
    public Sprite Sprite;
}

[System.Serializable]
public class Card
{
    public CardData data;
    public int currentStat;

    public Card(CardData data)
    {
        this.data = data;
        currentStat = data.Amount;
    }

    public void Play()
    {
        Debug.Log("Playing card: " + data.CardName);
        if (StatManager.Instance != null)
        {
            StatManager.Instance.DecreaseStat();
        }
    }
}