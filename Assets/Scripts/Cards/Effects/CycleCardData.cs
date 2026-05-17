using UnityEngine;

[CreateAssetMenu(fileName = "NewCycleCard", menuName = "Cards/Utility/Cycle")]
public class CycleCardData : CardData
{
    public int cardsToDraw = 3;
    public int healthCost = 3;

    public override void ExecuteEffect(Card cardInstance)
    {
        Debug.Log("Cycling hand...");
        
        if (CardPlayer.Instance != null)
        {
            CardPlayer.Instance.DiscardHand();
            CardPlayer.Instance.DrawCards(cardsToDraw);
            CardPlayer.Instance.TakeDamage(healthCost);
        }
    }
}
