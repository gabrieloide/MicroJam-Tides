using UnityEngine;

[CreateAssetMenu(fileName = "NewSacrificeCard", menuName = "Cards/Utility/Sacrifice")]
public class SacrificeCardData : CardData
{
    public int strengthGain = 2;
    public int shieldGain = 2;

    public override void ExecuteEffect(Card cardInstance)
    {
        Debug.Log("Sacrifice: +Stats for -Hand Limit.");
        StatManager.Instance.ModifyStrength(strengthGain);
        StatManager.Instance.ModifyShieldStat(shieldGain);
        StatManager.Instance.ModifyMaxHandSize(-Cost);
    }
}
