using UnityEngine;

[CreateAssetMenu(fileName = "NewAttackCard", menuName = "Cards/Attack")]
public class AttackCardData : CardData
{
    public int bonusDamage = 0; // 0 para Base, 5 para Pesada

    public override void ExecuteEffect(Card cardInstance)
    {
        int damage = StatManager.Instance.currentStrength + bonusDamage;
        Debug.Log($"Atacando: {CardName} hizo {damage} de daño.");
        
        if (Boss.Instance != null)
        {
            Boss.Instance.TakeDamage(damage);
        }

        StatManager.Instance.ModifyStrength(-Cost);
    }
}
