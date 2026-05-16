using UnityEngine;

[CreateAssetMenu(fileName = "NewShieldCard", menuName = "Cards/Shield")]
public class ShieldCardData : CardData
{
    public int bonusShield = 0; // 0 para Base, 5 para Pesada

    public override void ExecuteEffect(Card cardInstance)
    {
        int shieldValue = StatManager.Instance.currentShieldStat + bonusShield;
        Debug.Log($"Escudando: {CardName} dio {shieldValue} de escudo.");
        StatManager.Instance.AddActiveShield(shieldValue);
        StatManager.Instance.ModifyShieldStat(-1);
    }
}
