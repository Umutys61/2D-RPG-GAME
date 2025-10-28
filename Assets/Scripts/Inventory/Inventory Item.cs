using UnityEngine;

public enum ItemType
{
    Weapon,
    Armor,
    Potion,
    Scroll,
    Ingredients,
    Treasure
}

[CreateAssetMenu(menuName = "Items/Item")]
public class InventoryItem : ScriptableObject
{
    [Header("Config")]
    public string ID;
    public string Name;
    public Sprite Icon;
    [TextArea] public string Description;

    [Header("Info")]
    public ItemType ItemType;
    public bool IsConsumable;
    public bool IsStackable;
    public int MaxStack;

    [HideInInspector] public int Quantity;

    // ✅ Kopya oluştur (Inventory içinde stack yönetimi için)
    public InventoryItem CopyItem()
    {
        InventoryItem instance = Instantiate(this);
        return instance;
    }

    // ✅ Override edilebilecek sanal metodlar
    public virtual bool UseItem()
    {
        return true; // Default: consumable gibi kullanılır
    }

    public virtual void EquipItem()
    {
        // Default: hiçbir şey yapmaz, Weapon/Armor subclass'ı override edebilir
    }

    public void RemoveItem()
    {
        // Eğer özel bir şey yapılacaksa buraya eklersin    
    }
public virtual string GetDescription()
{
    string desc = Description; // Inspector’dan girilen temel açıklama

    // Armor ise
    Armor armor = this as Armor;
    if (armor != null)
    {
        if (armor.defense > 0) desc += $"\nDefense: +{armor.defense}";
        if (armor.magicResist > 0) desc += $"\nMagic Resist: +{armor.magicResist}";
        if (armor.healthBonus > 0) desc += $"\nHealth: +{armor.healthBonus}";
    }

    // Weapon ise
    Weapon weapon = this as Weapon;
    if (weapon != null)
    {
        if (weapon.Damage > 0) desc += $"\nDamage: {weapon.Damage}";
        if (weapon.attackBonus > 0) desc += $"\nAttack: +{weapon.attackBonus}";
        if (weapon.healthBonus > 0) desc += $"\nHealth: +{weapon.healthBonus}";
        if (weapon.manaBonus > 0) desc += $"\nMana: +{weapon.manaBonus}";
        if (weapon.RequiredMana > 0) desc += $"\nMana Cost: {weapon.RequiredMana}";
        if (weapon.Range > 0) desc += $"\nRange: {weapon.Range}";
    }

    return desc;
}

}
