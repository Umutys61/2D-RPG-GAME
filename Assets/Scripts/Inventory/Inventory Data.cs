[System.Serializable]
public class InventoryData
{
    public string[] ItemContent;
    public int[] ItemQuantity;

    // ✅ Ekipman kayıt alanları
    public string EquippedHelmet;
    public string EquippedChest;
    public string EquippedLegs;
    public string EquippedBoots;

    public string EquippedMainWeapon;
    public string EquippedSecondaryWeapon;
}
