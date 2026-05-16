using UnityEngine;

[CreateAssetMenu(fileName = "NewAttackCard", menuName = "Cards/Attack")]
public class AttackCardData : CardData
{
    public int bonusDamage = 0; // 0 for Base, 5 for Heavy
    
    public override void ExecuteEffect(Card cardInstance)
    {
        int damage = StatManager.Instance.currentStrength + bonusDamage;
        Debug.Log($"Attacking: {CardName} dealt {damage} damage.");
        
        if (Boss.Instance != null)
        {
            Boss.Instance.TakeDamage(damage);
        }

        StatManager.Instance.ModifyStrength(-Cost);
    }
}
