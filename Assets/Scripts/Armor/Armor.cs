using UnityEngine;

public enum ArmorType
{
    Helmet,
    Chest,
    Legs,
    Boots
}

[CreateAssetMenu(fileName = "Armor_", menuName = "Items/Armor")]
public class Armor : InventoryItem
{
    [Header("Armor Stats")]
    public ArmorType armorType;
    public int defense;
    public int magicResist;
    public float healthBonus;

    public override void EquipItem()
    {
        var playerEquip = GameManager.Instance.Player.GetComponent<PlayerEquipment>();
        if (playerEquip != null)
        {
            playerEquip.EquipArmor(this);
        }
    }
}
