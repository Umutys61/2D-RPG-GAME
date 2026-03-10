using UnityEngine;

public class PlayerMana : MonoBehaviour
{
    [Header("Config")]
    [SerializeField] private PlayerStats stats;

    public float CurrentMana { get; private set; }

    private void Start()
    {
        ResetMana();
    }

    private void Update()
    {
        if (stats.mana < stats.maxMana)
        {
            stats.mana += stats.manaRegenRate * Time.deltaTime;
            if (stats.mana > stats.maxMana)
                stats.mana = stats.maxMana;
        }

        if (Input.GetKeyDown(KeyCode.X))
        {
            UseMana(1f);
        }
    }

    public void UseMana(float amount)
    {
        stats.mana = Mathf.Max(stats.mana - amount, 0f);
        CurrentMana = stats.mana;
    }

    public void RecoverMana(float amount)
    {
        stats.mana += amount;
        stats.mana = Mathf.Min(stats.mana, stats.maxMana);
    }

    public bool CanRecoverMana()
    {
        return stats.mana > 0 && stats.mana < stats.maxMana;
    }

    public void ResetMana()
    {
        CurrentMana = stats.maxMana;
        stats.mana = stats.maxMana;
    }
}
