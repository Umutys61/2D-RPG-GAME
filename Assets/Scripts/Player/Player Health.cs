using UnityEngine;
using Vector3 = UnityEngine.Vector3;

public class PlayerHealth : MonoBehaviour, IDamageable
{
    [Header("Config")]
    [SerializeField] private PlayerStats stats;
    private PlayerAnimations playerAnimations;

    private void Awake()
    {
        playerAnimations = GetComponent<PlayerAnimations>();
    }

    private void Update()
    {
        if (stats.health <= 0f)
        {
            PlayerDead();
            return;
        }

        // ✅ Health Regen
        if (stats.health < stats.maxHealth)
        {
            stats.health += stats.healthRegenRate * Time.deltaTime;
            if (stats.health > stats.maxHealth)
                stats.health = stats.maxHealth;
        }
    }

    public void TakeDamage(float amount)
    {
        if (stats.health <= 0f) return;

        // 🛡️ Defense level hasarı azaltır
        float reduced = amount * (1f - stats.GetDefenseReduction());

        if (reduced < 1f) reduced = 1f; // minimum damage sınırı

        stats.health -= reduced;
        DamageManager.Instance.ShowDamageText(reduced, transform);

        // 🛡️ Defense EXP kazan
        stats.AddDefenseExp(1);

        if (stats.health <= 0f)
        {
            stats.health = 0f;
            PlayerDead();
        }
    }

    public void RestoreHealth(float amount)
    {
        stats.health += amount;
        if (stats.health > stats.maxHealth)
        {
            stats.health = stats.maxHealth;
        }
    }

    public bool CanRestoreHealth()
    {
        return stats.health > 0 && stats.health < stats.maxHealth;
    }

    private void PlayerDead()
    {
        playerAnimations.SetDeadAnimation();
    }
}
