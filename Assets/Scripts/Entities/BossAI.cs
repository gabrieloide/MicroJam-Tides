using UnityEngine;

public enum EnemyIntent
{
    LightAttack,
    HeavyAttack,
    Rest
}

public class BossAI : MonoBehaviour
{
    public static BossAI Instance { get; private set; }
    public static System.Action<EnemyIntent> OnIntentDecided;
    
    [Header("Current State")]
    [SerializeField] private EnemyIntent currentIntent;
    [SerializeField] private bool isEnraged;
    [SerializeField] private int heavyAttackCooldown = 0;
    
    private Boss _boss;

    public EnemyIntent CurrentIntent => currentIntent;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        _boss = GetComponent<Boss>();
    }

    public void DecideIntent(int playerShieldStat, int shieldCardsInHand)
    {
        CheckBossPhase();

        if (heavyAttackCooldown > 0)
        {
            heavyAttackCooldown--;
        }
        
        if (playerShieldStat == 0 && shieldCardsInHand == 0 && heavyAttackCooldown == 0)
        {
            SetIntent(EnemyIntent.HeavyAttack);
            return;
        }

        if (shieldCardsInHand >= 3 && !isEnraged)
        {
            SetIntent(EnemyIntent.Rest);
            return;
        }

        if (isEnraged)
        {
            float rand = Random.value;
            if (rand < 0.6f || heavyAttackCooldown > 0) 
                SetIntent(EnemyIntent.LightAttack);
            else 
                SetIntent(EnemyIntent.HeavyAttack);
            
            return;
        }

        float decision = Random.value;

        if (decision < 0.2f && !isEnraged)
        {
            SetIntent(EnemyIntent.Rest);
        }
        else if (decision < 0.7f || heavyAttackCooldown > 0)
        {
            SetIntent(EnemyIntent.LightAttack);
        }
        else
        {
            SetIntent(EnemyIntent.HeavyAttack);
        }
    }

    private void SetIntent(EnemyIntent intent)
    {
        currentIntent = intent;
        if (intent == EnemyIntent.HeavyAttack)
        {
            heavyAttackCooldown = isEnraged ? 1 : 2;
        }
        
        OnIntentDecided?.Invoke(intent);
        Debug.Log($"[BossAI] Phase: {(isEnraged ? "ENRAGED" : "Normal")} | Decision: {currentIntent}");
    }

    private void CheckBossPhase()
    {
        if (_boss == null) return;
        
        if (_boss.GetHealth() <= 40 && !isEnraged)
        {
            isEnraged = true;
            Debug.Log("<color=red>[BossAI] BOSS IS ENRAGED!</color>");
        }
    }

    public void ExecuteIntent()
    {
        int damage = 0;
        switch (currentIntent)
        {
            case EnemyIntent.LightAttack: damage = 6; break;
            case EnemyIntent.HeavyAttack: damage = 12; break;
            case EnemyIntent.Rest: damage = 0; break;
        }

        Debug.Log($"[BossAI] Executing Intent: {currentIntent} for {damage} damage.");

        if (damage > 0 && StatManager.Instance != null && CardPlayer.Instance != null)
        {
            int blocked = Mathf.Min(damage, StatManager.Instance.activeShield);
            if (blocked > 0)
            {
                StatManager.Instance.AddActiveShield(-blocked);
                damage -= blocked;
                Debug.Log($"[BossAI] Player blocked {blocked} damage. Remaining damage: {damage}");
            }

            if (damage > 0)
            {
                CardPlayer.Instance.TakeDamage(damage);
            }
        }
    }
}
