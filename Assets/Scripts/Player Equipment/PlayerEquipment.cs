using UnityEngine;

public class PlayerEquipment : MonoBehaviour
{
    public Armor Helmet;
    public Armor Chest;
    public Armor Legs;
    public Armor Boots;

    public Weapon MainWeapon;
    public Weapon SecondaryWeapon;

    public void EquipArmor(Armor armor)
    {
        switch (armor.armorType)
        {
            case ArmorType.Helmet: Helmet = armor; break;
            case ArmorType.Chest:  Chest = armor; break;
            case ArmorType.Legs:   Legs = armor; break;
            case ArmorType.Boots:  Boots = armor; break;
        }

        Debug.Log($"🛡️ {armor.Name} kuşanıldı! (DEF +{armor.defense}, HP +{armor.healthBonus}, MR +{armor.magicResist})");

        GameManager.Instance.Player.Stats.RecalculateStats(this);
    }

    public void UnequipArmor(ArmorType type)
    {
        switch (type)
        {
            case ArmorType.Helmet: Helmet = null; break;
            case ArmorType.Chest:  Chest = null; break;
            case ArmorType.Legs:   Legs = null; break;
            case ArmorType.Boots:  Boots = null; break;
        }

        Debug.Log($"🛡️ {type} çıkarıldı!");

        GameManager.Instance.Player.Stats.RecalculateStats(this);
    }

    public void EquipMainWeapon(Weapon weapon)
    {
        MainWeapon = weapon;
        Debug.Log($"⚔️ {weapon.name} Main Weapon olarak kuşanıldı!");

        GameManager.Instance.Player.Stats.RecalculateStats(this);
    }

    public void UnequipMainWeapon()
    {
        Debug.Log($"⚔️ {MainWeapon?.name ?? "None"} çıkarıldı!");
        MainWeapon = null;

        GameManager.Instance.Player.Stats.RecalculateStats(this);
    }

    public void EquipSecondaryWeapon(Weapon weapon)
    {
        SecondaryWeapon = weapon;
        Debug.Log($"🗡️ {weapon.name} Secondary Weapon olarak kuşanıldı!");

        GameManager.Instance.Player.Stats.RecalculateStats(this);
    }

    public void UnequipSecondaryWeapon()
    {
        Debug.Log($"🗡️ {SecondaryWeapon?.name ?? "None"} çıkarıldı!");
        SecondaryWeapon = null;

        GameManager.Instance.Player.Stats.RecalculateStats(this);
    }

    public int GetTotalDefense()
    {
        int def = 0;
        if (Helmet != null) def += Helmet.defense;
        if (Chest != null) def += Chest.defense;
        if (Legs != null) def += Legs.defense;
        if (Boots != null) def += Boots.defense;
        return def;
    }

    public int GetTotalMagicResist()
    {
        int resist = 0;
        if (Helmet != null) resist += Helmet.magicResist;
        if (Chest != null) resist += Chest.magicResist;
        if (Legs != null) resist += Legs.magicResist;
        if (Boots != null) resist += Boots.magicResist;
        return resist;
    }

    public float GetTotalHealthBonus()
    {
        float bonus = 0;
        if (Helmet != null) bonus += Helmet.healthBonus;
        if (Chest != null) bonus += Chest.healthBonus;
        if (Legs != null) bonus += Legs.healthBonus;
        if (Boots != null) bonus += Boots.healthBonus;
        return bonus;
    }
}
