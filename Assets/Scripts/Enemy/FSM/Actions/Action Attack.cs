using UnityEngine;

public class ActionAttack : FSMAction
{
    [Header("Config")]
    [SerializeField] private float damage = 5f;
    [SerializeField] private float attackRange = 1.5f;
    [SerializeField] private float attackCooldown = 1f;

    private EnemyBrain enemyBrain;
    private float cooldownTimer;

    public float AttackRange => attackRange;

    private void Awake()
    {
        enemyBrain = GetComponent<EnemyBrain>();
    }

    public override void Act()
    {
        if (enemyBrain.Player == null) return;

        cooldownTimer -= Time.deltaTime;
        if (cooldownTimer > 0f) return;

        float distance = Vector2.Distance(transform.position, enemyBrain.Player.position);
        if (distance <= attackRange)
        {
            IDamageable player = enemyBrain.Player.GetComponent<IDamageable>();
            if (player != null)
            {
                player.TakeDamage(damage);
                cooldownTimer = attackCooldown;
            }
        }
    }
}
