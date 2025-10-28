using UnityEngine;
using BayatGames.SaveGameFree;

public class EquipmentManager : Singleton<EquipmentManager>
{
    [Header("Config")]
    [SerializeField] private EquipmentSlot[] slots; // Helmet, Chest, Legs, Boots, MainWeapon, SecondaryWeapon

    private readonly string EQUIPMENT_KEY = "MY_EQUIPMENT";

    [System.Serializable]
    public class EquipmentData
    {
        public string[] equippedIDs; // Slot bazlı item ID’leri
    }

    private void Start()
    {
        LoadEquipment();
    }

    public void SaveEquipment()
    {
        EquipmentData data = new EquipmentData();
        data.equippedIDs = new string[slots.Length];

        for (int i = 0; i < slots.Length; i++)
        {
            var item = slots[i].GetCurrentItem();
            data.equippedIDs[i] = (item != null) ? item.ID : null;
        }

        SaveGame.Save(EQUIPMENT_KEY, data);
        Debug.Log("✅ Equipment saved!");
    }

    public void LoadEquipment()
    {
        if (!SaveGame.Exists(EQUIPMENT_KEY)) return;

        EquipmentData data = SaveGame.Load<EquipmentData>(EQUIPMENT_KEY);

        for (int i = 0; i < slots.Length; i++)
        {
            string id = data.equippedIDs[i];
            if (!string.IsNullOrEmpty(id))
            {
                InventoryItem item = Inventory.Instance.GetItemFromContent(id);
                if (item != null)
                {
                    slots[i].SetItem(item.CopyItem());
                }
            }
        }

        Debug.Log("✅ Equipment loaded!");
    }
}
