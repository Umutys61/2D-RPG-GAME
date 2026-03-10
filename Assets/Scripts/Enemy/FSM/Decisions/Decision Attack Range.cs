using UnityEngine;

public class DecisionAttackRange : FSMDecision
{
    [Header("Config")]
    [SerializeField] private ActionAttack actionAttack;

    private EnemyBrain enemy;

    private void Awake()
    {
        enemy = GetComponent<EnemyBrain>();
        if (actionAttack == null)
            actionAttack = GetComponent<ActionAttack>();
    }

    public override bool Decide()
    {
        return PlayerInAttackRange();
    }

    private bool PlayerInAttackRange()
    {
        if (enemy.Player == null || actionAttack == null) return false;

        float distance = Vector2.Distance(enemy.transform.position, enemy.Player.position);
        return distance <= actionAttack.AttackRange;
    }

    private void OnDrawGizmosSelected()
    {
        if (actionAttack == null) return;
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, actionAttack.AttackRange);
    }
}
