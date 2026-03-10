using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public enum EquipmentSlotType
{
    Helmet,
    Chest,
    Legs,
    Boots,
    MainWeapon,
    SecondaryWeapon
}

public class EquipmentSlot : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] private EquipmentSlotType slotType;  
    [SerializeField] private Image icon;                  

    private InventoryItem currentItem;

    private float lastClickTime;
    private const float doubleClickDelay = 0.3f;

    public void SetItem(InventoryItem item)
    {
        currentItem = item;
        icon.sprite = item.Icon;
        icon.gameObject.SetActive(true);

        var playerEquip  = GameManager.Instance.Player.GetComponent<PlayerEquipment>();
        var playerAttack = GameManager.Instance.Player.GetComponent<PlayerAttack>();
        if (playerEquip == null || playerAttack == null) return;

        Armor armor = item as Armor;
        if (armor != null)
        {
            playerEquip.EquipArmor(armor);
            return;
        }

        Weapon weapon = item as Weapon;
        if (weapon != null)
        {
            if (slotType == EquipmentSlotType.MainWeapon)
            {
                playerEquip.EquipMainWeapon(weapon);
                playerAttack.EquipWeapon(weapon);
                WeaponManager.Instance.EquipWeapon(weapon);
            }
            else if (slotType == EquipmentSlotType.SecondaryWeapon)
            {
                playerEquip.EquipSecondaryWeapon(weapon);
            }
            return;
        }
    }

    public InventoryItem GetCurrentItem() => currentItem;

    public void ClearSlot()
    {
        if (currentItem == null) return;

        currentItem = null;
        icon.sprite = null;
        icon.gameObject.SetActive(false);

        var playerEquip  = GameManager.Instance.Player.GetComponent<PlayerEquipment>();
        var playerAttack = GameManager.Instance.Player.GetComponent<PlayerAttack>();
        if (playerEquip == null || playerAttack == null) return;

        switch (slotType)
        {
            case EquipmentSlotType.Helmet:       
                playerEquip.UnequipArmor(ArmorType.Helmet); 
                break;

            case EquipmentSlotType.Chest:        
                playerEquip.UnequipArmor(ArmorType.Chest); 
                break;

            case EquipmentSlotType.Legs:         
                playerEquip.UnequipArmor(ArmorType.Legs); 
                break;

            case EquipmentSlotType.Boots:        
                playerEquip.UnequipArmor(ArmorType.Boots); 
                break;

            case EquipmentSlotType.MainWeapon:   
                playerEquip.UnequipMainWeapon();
                playerAttack.EquipWeapon(null);
                WeaponManager.Instance.EquipWeapon(null);
                break;

            case EquipmentSlotType.SecondaryWeapon: 
                playerEquip.UnequipSecondaryWeapon();
                break;
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (Time.time - lastClickTime < doubleClickDelay)
        {
            if (currentItem != null)
            {
                Debug.Log($"❌ {currentItem.Name} slotundan çıkarıldı!");
                Unequip();
            }
        }
        lastClickTime = Time.time;
    }

    private void Unequip()
    {
        if (currentItem == null) return;

        InventoryItem unequippedItem = currentItem;
        ClearSlot();
        Inventory.Instance.AddItem(unequippedItem, 1);

        Debug.Log($"🔄 {unequippedItem.Name} envantere geri döndü!");
        EquipmentUI.Instance.RefreshSave();
    }
}
