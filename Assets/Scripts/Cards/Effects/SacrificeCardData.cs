using UnityEngine;

[CreateAssetMenu(fileName = "NewSacrificeCard", menuName = "Cards/Utility/Sacrifice")]
public class SacrificeCardData : CardData
{
    public int strengthGain = 2;
    public int shieldGain = 2;
    public int healthCost = 20;

    public override void ExecuteEffect(Card cardInstance)
    {
        Debug.Log($"Sacrifice: +Stats for -{healthCost} HP.");
        
        StatManager.Instance.ModifyStrength(strengthGain);
        StatManager.Instance.ModifyShieldStat(shieldGain);
        
        // Take damage to gain massive stats
        if (CardPlayer.Instance != null)
        {
            CardPlayer.Instance.TakeDamage(healthCost);
        }
    }
}
