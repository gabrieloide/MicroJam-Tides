using UnityEngine;

[CreateAssetMenu(fileName = "NewCycleCard", menuName = "Cards/Utility/Cycle")]
public class CycleCardData : CardData
{
    public int cardsToDraw = 3;

    public override void ExecuteEffect(Card cardInstance)
    {
        Debug.Log("Cycling hand...");
        CardPlayer.Instance.DiscardHand();
        CardPlayer.Instance.DrawCards(cardsToDraw);
        StatManager.Instance.ModifyMaxHandSize(-Cost);
    }
}
