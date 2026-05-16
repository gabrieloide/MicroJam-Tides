using UnityEngine;

[CreateAssetMenu(fileName = "NewDrawCard", menuName = "Cards/Utility/Draw")]
public class DrawCardData : CardData
{
    public int drawAmount = 2;

    public override void ExecuteEffect(Card cardInstance)
    {
        Debug.Log("Drawing extra cards...");
        CardPlayer.Instance.DrawCards(drawAmount);
        StatManager.Instance.ModifyMaxHandSize(-Cost);
    }
}
