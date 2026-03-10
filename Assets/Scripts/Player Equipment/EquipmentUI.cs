using UnityEngine;

public class EquipmentUI : Singleton<EquipmentUI>
{
    [Header("Slots")]
    public EquipmentSlot helmetSlot;
    public EquipmentSlot chestSlot;
    public EquipmentSlot legsSlot;
    public EquipmentSlot bootsSlot;
    public EquipmentSlot mainWeaponSlot;
    public EquipmentSlot secondaryWeaponSlot;

    private void Start()
    {
        var playerAttack = GameManager.Instance.Player.playerAttack;

        if (mainWeaponSlot != null && mainWeaponSlot.GetCurrentItem() is Weapon weapon)
        {
            playerAttack.EquipWeapon(weapon);
            WeaponManager.Instance.EquipWeapon(weapon);
        }
        else
        {
            playerAttack.EquipWeapon(null);
            WeaponManager.Instance.EquipWeapon(null);
        }
    }

    public void AssignItemToSlot(InventoryItem item)
    {
        Armor armor = item as Armor;
        if (armor != null)
        {
            EquipmentSlot targetSlot = null;
            switch (armor.armorType)
            {
                case ArmorType.Helmet: targetSlot = helmetSlot; break;
                case ArmorType.Chest:  targetSlot = chestSlot; break;
                case ArmorType.Legs:   targetSlot = legsSlot; break;
                case ArmorType.Boots:  targetSlot = bootsSlot; break;
            }

            if (targetSlot != null)
            {
                var oldItem = targetSlot.GetCurrentItem();
                if (oldItem != null)
                {
                    Inventory.Instance.AddItem(oldItem, 1);
                }

                targetSlot.SetItem(item);
            }

            EquipmentManager.Instance.SaveEquipment();
            return;
        }

        Weapon weapon = item as Weapon;
        if (weapon != null)
        {
            if (mainWeaponSlot != null)
            {
                var oldItem = mainWeaponSlot.GetCurrentItem();
                if (oldItem != null)
                {
                    Inventory.Instance.AddItem(oldItem, 1);
                }

                mainWeaponSlot.SetItem(item);
            }

            EquipmentManager.Instance.SaveEquipment();
        }
    }

    public void RefreshSave()
    {
        EquipmentManager.Instance.SaveEquipment();
    }
}
