using UnityEngine;

[CreateAssetMenu(fileName = "NewDrawCard", menuName = "Cards/Utility/Draw")]
public class DrawCardData : CardData
{
    public int drawAmount = 2;
    public int healthCost = 3;

    public override void ExecuteEffect(Card cardInstance)
    {
        Debug.Log("Drawing extra cards...");
        
        if (CardPlayer.Instance != null)
        {
            CardPlayer.Instance.DrawCards(drawAmount);
            CardPlayer.Instance.TakeDamage(healthCost);
        }
    }
}
