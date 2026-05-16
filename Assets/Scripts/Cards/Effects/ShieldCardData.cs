using UnityEngine;

[CreateAssetMenu(fileName = "NewShieldCard", menuName = "Cards/Shield")]
public class ShieldCardData : CardData
{
    public int bonusShield = 0;
    
    public override void ExecuteEffect(Card cardInstance)
    {
        int shieldValue = StatManager.Instance.currentShieldStat + bonusShield;
        Debug.Log($"Shielding: {CardName} added {shieldValue} shield.");
        StatManager.Instance.AddActiveShield(shieldValue);
        StatManager.Instance.ModifyShieldStat(-Cost);
    }
}
