using UnityEngine;

public class PlayerExp : MonoBehaviour
{
    [Header("Config")]
    [SerializeField] private PlayerStats stats;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.C))
        {
            AddExp(300f);
        }
    }

    public void AddExp(float amount)
    {
        stats.TotalExp += amount;
        stats.currentExp += amount;

        while (stats.currentExp >= stats.nextLevelExp)
        {
            stats.currentExp -= stats.nextLevelExp;
            NextLevel();
        }
    }

    private void NextLevel()
    {
        stats.level++;
        stats.AttributePoints++;

        float currentExpRequired = stats.nextLevelExp;
        float newNextLevelExp = Mathf.Round(
            currentExpRequired + ((stats.ExpMultiplier / 100f) * stats.nextLevelExp)
        );
        stats.nextLevelExp = newNextLevelExp;

        // Statları yeniden hesapla
        PlayerEquipment eq = FindFirstObjectByType<PlayerEquipment>();
        if (eq != null)
        {
            stats.RecalculateStats(eq);
        }

        // Level atlayınca can/mana doldur
        stats.health = stats.maxHealth;
        stats.mana   = stats.maxMana;

        Debug.Log($"🎉 Level {stats.level} oldun! Yeni Max HP: {stats.maxHealth}");
    }
}
