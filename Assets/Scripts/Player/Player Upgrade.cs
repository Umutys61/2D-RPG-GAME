using System;
using UnityEngine;

public class PlayerUpgrade : MonoBehaviour
{
    public static event Action OnPlayerUpgradeEvent;

    [Header("Config")]
    [SerializeField] private PlayerStats stats;

    [Header("Settings")]
    [SerializeField] private UpgradeSettings[] settings;

    private void UpgradePlayer(int upgradeIndex)
    {
        stats.BaseDamage += settings[upgradeIndex].DamageUpgrade;
        stats.TotalDamage += settings[upgradeIndex].DamageUpgrade;

        stats.baseMaxHealth += settings[upgradeIndex].HealthUpgrade;
        stats.baseMaxMana   += settings[upgradeIndex].ManaUpgrade;

        stats.CriticalChance += settings[upgradeIndex].CChanceUpgrade;
        stats.CriticalDamage += settings[upgradeIndex].CdamageUpgrade;

        PlayerEquipment eq = FindFirstObjectByType<PlayerEquipment>();
        if (eq != null)
        {
            stats.RecalculateStats(eq);
        }

        stats.health = stats.maxHealth;
        stats.mana   = stats.maxMana;
    }

    private void AttributeCallback(AttributeType attributeType)
    {
        if (stats.AttributePoints == 0) return;

        switch (attributeType)
        {
            case AttributeType.Strength:
                UpgradePlayer(0);
                stats.Strength++;
                break;

            case AttributeType.Dexterity:
                UpgradePlayer(1);
                stats.Dexterity++;
                break;

            case AttributeType.Intelligence:
                UpgradePlayer(2);
                stats.Intelligence++;
                break;
        }

        stats.AttributePoints--;
        OnPlayerUpgradeEvent?.Invoke();
    }

    private void OnEnable()
    {
        AttributeButton.OnAttributeSelectedEvent += AttributeCallback;
    }

    private void OnDisable()
    {
        AttributeButton.OnAttributeSelectedEvent -= AttributeCallback;
    }
}

[Serializable]
public class UpgradeSettings
{
    public string Name;

    [Header("Values")]
    public float DamageUpgrade;
    public float HealthUpgrade;
    public float ManaUpgrade;
    public float CChanceUpgrade;
    public float CdamageUpgrade;
}
