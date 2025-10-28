using UnityEngine;

public enum AttributeType
{
    Strength,
    Dexterity,
    Intelligence
}

[CreateAssetMenu(fileName = "PlayerStats", menuName = "Player Stats")]
public class PlayerStats : ScriptableObject
{
    [Header("Config")]
    public int level = 1;

    [Header("Base Stats")]
    public float baseMaxHealth = 100;
    public float baseMaxMana = 200;
    public float baseMagicResist = 0;

    [Header("Health")]
    public float health;
    public float maxHealth;

    [Header("Mana")]
    public float mana;
    public float maxMana;

    [Header("Experience")]
    public float currentExp;
    public float nextLevelExp;
    public float InitialNextLevelExp = 100;
    [Range(1f, 100f)] public float ExpMultiplier = 20;

    [Header("Attack")]
    public float BaseDamage = 2;
    public float CriticalChance = 10;
    public float CriticalDamage = 50;

    [Header("Combat Skills")]
    public int meleeLevel = 1;
    public int meleeExp = 0;
    public int meleeNextExp = 10;

    public int magicLevel = 1;
    public int magicExp = 0;
    public int magicNextExp = 10;

    public int bowLevel = 1;
    public int bowExp = 0;
    public int bowNextExp = 10;

    public int defenseLevel = 1;
    public int defenseExp = 0;
    public int defenseNextExp = 10;

    [Header("Resist & Regen")]
    public float magicResist = 0f;
    public float healthRegenRate = 1f;
    public float manaRegenRate = 1f;

    [Header("Attributes")]
    public int Strength;
    public int Dexterity;
    public int Intelligence;
    public int AttributePoints;

    [HideInInspector] public float TotalExp;
    [HideInInspector] public float TotalDamage;

    [Header("Gathering Skills")]
    public int woodcuttingLevel = 1;
    public int woodcuttingExp = 0;
    public int woodcuttingNextExp = 10;

    public int miningLevel = 1;
    public int miningExp = 0;
    public int miningNextExp = 10;

    public int fishingLevel = 1;
    public int fishingExp = 0;
    public int fishingNextExp = 10;

    // -------------------------
    // BONUS HESAPLAMA METHODLARI
    // -------------------------
    public float GetMeleeBonusDamage() => meleeLevel * 0.5f;
    public float GetMagicBonusDamage() => magicLevel * 0.3f;
    public float GetBowBonusDamage() => bowLevel * 0.2f;

    public float GetDefenseReduction()
    {
        float reduction = defenseLevel * 0.005f;
        return Mathf.Clamp(reduction, 0f, 0.7f); // max %70 reduction
    }

    public float GetGatherTimeMultiplier(ResourceType type)
    {
        switch (type)
        {
            case ResourceType.Wood: return 1f - (woodcuttingLevel * 0.01f);
            case ResourceType.Ore: return 1f - (miningLevel * 0.01f);
            case ResourceType.Fish: return 1f - (fishingLevel * 0.01f);
            default: return 1f;
        }
    }

    // -------------------------
    // XP EKLEME
    // -------------------------
    public void AddCombatExp(WeaponType type, int amount)
    {
        switch (type)
        {
            case WeaponType.Melee:
                meleeExp += amount;
                if (meleeExp >= meleeNextExp)
                {
                    meleeLevel++;
                    meleeExp = 0;
                    meleeNextExp += 10;
                    Debug.Log($"⚔️ Melee seviyesi {meleeLevel} oldu!");
                }
                break;

            case WeaponType.Magic:
                magicExp += amount;
                if (magicExp >= magicNextExp)
                {
                    magicLevel++;
                    magicExp = 0;
                    magicNextExp += 10;
                    Debug.Log($"✨ Magic seviyesi {magicLevel} oldu!");
                }
                break;

            case WeaponType.Bow:
                bowExp += amount;
                if (bowExp >= bowNextExp)
                {
                    bowLevel++;
                    bowExp = 0;
                    bowNextExp += 10;
                    Debug.Log($"🏹 Bow seviyesi {bowLevel} oldu!");
                }
                break;
        }
    }

    public void AddDefenseExp(int amount)
    {
        defenseExp += amount;
        if (defenseExp >= defenseNextExp)
        {
            defenseLevel++;
            defenseExp = 0;
            defenseNextExp += 10;
            Debug.Log($"🛡️ Defense seviyesi {defenseLevel} oldu!");
        }
    }

    public void AddGatherExp(ResourceType type, int amount)
    {
        switch (type)
        {
            case ResourceType.Wood:
                woodcuttingExp += amount;
                if (woodcuttingExp >= woodcuttingNextExp)
                {
                    woodcuttingLevel++;
                    woodcuttingExp = 0;
                    woodcuttingNextExp += 10;
                    Debug.Log($"🌲 Woodcutting seviyesi {woodcuttingLevel} oldu!");
                }
                break;

            case ResourceType.Ore:
                miningExp += amount;
                if (miningExp >= miningNextExp)
                {
                    miningLevel++;
                    miningExp = 0;
                    miningNextExp += 10;
                    Debug.Log($"⛏️ Mining seviyesi {miningLevel} oldu!");
                }
                break;

            case ResourceType.Fish:
                fishingExp += amount;
                if (fishingExp >= fishingNextExp)
                {
                    fishingLevel++;
                    fishingExp = 0;
                    fishingNextExp += 10;
                    Debug.Log($"🎣 Fishing seviyesi {fishingLevel} oldu!");
                }
                break;
        }
    }

    // -------------------------
    // STAT HESAPLAMA
    // -------------------------
    public void RecalculateStats(PlayerEquipment equipment)
    {
        // 🔹 Level bonusu
        float newMaxHealth   = baseMaxHealth + ((level - 1) * 15);
        float newMaxMana     = baseMaxMana;
        float newMagicResist = baseMagicResist;
        float newDefense     = 0;
        float newAttack      = BaseDamage;

        // 🔹 Armor bonusları
        if (equipment != null)
        {
            newMaxHealth   += equipment.GetTotalHealthBonus();
            newMagicResist += equipment.GetTotalMagicResist();
            newDefense     += equipment.GetTotalDefense();

            // 🔹 Main Weapon bonusları
            if (equipment.MainWeapon != null)
            {
                newAttack    += equipment.MainWeapon.attackBonus;
                newMaxHealth += equipment.MainWeapon.healthBonus;
                newMaxMana   += equipment.MainWeapon.manaBonus;
            }

            // 🔹 Secondary Weapon bonusları
            if (equipment.SecondaryWeapon != null)
            {
                newAttack    += equipment.SecondaryWeapon.attackBonus;
                newMaxHealth += equipment.SecondaryWeapon.healthBonus;
                newMaxMana   += equipment.SecondaryWeapon.manaBonus;
            }
        }

        // Son değerleri güncelle
        maxHealth   = newMaxHealth;
        maxMana     = newMaxMana;
        magicResist = newMagicResist;
        defenseLevel = Mathf.RoundToInt(newDefense);
        TotalDamage  = newAttack;

        if (health > maxHealth) health = maxHealth;

        Debug.Log($"📊 Stat Güncellendi → HP:{maxHealth}, Mana:{maxMana}, Atk:{TotalDamage}, Def:{defenseLevel}, MR:{magicResist}");
    }

    // -------------------------
    // RESET
    // -------------------------
  public void resetPlayer()
{
    level = 1;
    currentExp = 0;
    nextLevelExp = InitialNextLevelExp;
    TotalExp = 0f;

    BaseDamage = 2;
    CriticalChance = 10;
    CriticalDamage = 50;

    Strength = 0;
    Dexterity = 0;
    Intelligence = 0;
    AttributePoints = 0;

    meleeLevel = 1;
    meleeExp = 0;
    meleeNextExp = 10;

    magicLevel = 1;
    magicExp = 0;
    magicNextExp = 10;

    bowLevel = 1;
    bowExp = 0;
    bowNextExp = 10;

    defenseLevel = 1;
    defenseExp = 0;
    defenseNextExp = 10;

    magicResist = baseMagicResist;
    healthRegenRate = 1f;
    manaRegenRate = 1f;

    woodcuttingLevel = 1;
    woodcuttingExp = 0;
    woodcuttingNextExp = 10;

    miningLevel = 1;
    miningExp = 0;
    miningNextExp = 10;

    fishingLevel = 1;
    fishingExp = 0;
    fishingNextExp = 10;

    // ✅ Statları yeniden hesapla (level 1, base + equipment)
    PlayerEquipment eq = Object.FindFirstObjectByType<PlayerEquipment>();
    if (eq != null)
    {
        RecalculateStats(eq);
    }

    // ✅ Can ve mana full çek
    health = maxHealth;
    mana   = maxMana;
}

}
