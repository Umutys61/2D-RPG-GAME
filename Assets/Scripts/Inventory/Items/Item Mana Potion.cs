using UnityEngine;
[CreateAssetMenu(fileName = "ItemManaPotion", menuName = "Items/Mana Potion")]

public class ItemManaPotion : InventoryItem
{
    [Header("Config")]
    public float manaValue;

    public override bool UseItem()
    {
        if (GameManager.Instance.Player.playerMana.CanRecoverMana())
        {
            GameManager.Instance.Player.playerMana.RecoverMana(manaValue);
            return true;
        }
        return false;
    }
}
