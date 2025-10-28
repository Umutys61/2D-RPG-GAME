using System;
using System.Collections.Generic;
using BayatGames.SaveGameFree;
using UnityEngine;

public class Inventory : Singleton<Inventory>
{
    [Header("Config")]
    [SerializeField] private GameContent gameContent;
    [SerializeField] private int inventorySize;
    [SerializeField] private InventoryItem[] inventoryItems;

    [Header("Testing")]
    public InventoryItem testItem;
    public int InventorySize => inventorySize;

    public InventoryItem[] InventoryItems => inventoryItems;

    private readonly string INVENTORY_KEY_DATA = "MY_INVENTORY";

    [Header("World Drop")]
    [SerializeField] private GameObject worldItemPrefab;   // ✅ sadece tek prefab

    private void Start()
    {
        inventoryItems = new InventoryItem[inventorySize];
        VerifyItemsForDraw();
        LoadInventory();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.L))
        {
            AddItem(testItem, 1);
        }
    }

    public void AddItem(InventoryItem item, int quantity)
    {
        if (item == null || quantity <= 0) return;

        // 🔑 Coin kontrolü
        if (item.ID == "ItemGoldCoin")
        {
            CoinManager.Instance.AddCoins(quantity);
            return; // coin envantere girmez
        }

        List<int> itemIndexes = CheckItemStockIndexes(item.ID);
        if (item.IsStackable && itemIndexes.Count > 0)
        {
            foreach (int index in itemIndexes)
            {
                int maxStack = item.MaxStack;
                if (inventoryItems[index].Quantity < maxStack)
                {
                    inventoryItems[index].Quantity += quantity;
                    if (inventoryItems[index].Quantity > maxStack)
                    {
                        int dif = inventoryItems[index].Quantity - maxStack;
                        inventoryItems[index].Quantity = maxStack;
                        AddItem(item, dif);
                    }
                    InventoryUI.Instance.DrawItem(inventoryItems[index], index);
                    SaveInventory();
                    return;
                }
            }
        }

        int quantityToAdd = quantity > item.MaxStack ? item.MaxStack : quantity;
        AddItemFreeSlot(item, quantityToAdd);
        int remainingAmount = quantity - quantityToAdd;
        if (remainingAmount > 0)
        {
            AddItem(item, remainingAmount);
        }
        SaveInventory();
    }
public void UseItem(int index)
{
    if (inventoryItems[index] == null) return;

    InventoryItem item = inventoryItems[index];

    // ❌ consumable değilse hiç çalışmasın
    if (!item.IsConsumable)
    {
        Debug.LogWarning($"{item.Name} kullanılamaz (Consumable değil).");
        return;
    }

    // ✅ consumable ise kullan
    if (item.UseItem())
    {
        DecreaseItemStack(index);
        SaveInventory();
    }
}



    // ---------------- REMOVE ----------------
    public void RemoveItem(int index)
    {
        // eski RemoveItem = hepsini siler
        if (inventoryItems[index] == null) return;
        RemoveItemAmount(index, inventoryItems[index].Quantity);
    }

    public void RemoveItemAmount(int index, int amount)
    {
        if (inventoryItems[index] == null || amount <= 0) return;

        inventoryItems[index].Quantity -= amount;

        if (inventoryItems[index].Quantity <= 0)
        {
            inventoryItems[index] = null;
            InventoryUI.Instance.DrawItem(null, index);
        }
        else
        {
            InventoryUI.Instance.DrawItem(inventoryItems[index], index);
        }

        SaveInventory();
    }
    // ----------------------------------------

    // ✅ DROP ITEM
    public void DropItem(int index, int amount)
    {
        if (inventoryItems[index] == null || amount <= 0) return;

        InventoryItem item = inventoryItems[index];

        // envanterden eksilt
        inventoryItems[index].Quantity -= amount;
        if (inventoryItems[index].Quantity <= 0)
        {
            inventoryItems[index] = null;
            InventoryUI.Instance.DrawItem(null, index);
        }
        else
        {
            InventoryUI.Instance.DrawItem(inventoryItems[index], index);
        }

        SaveInventory();

        // sahneye bırak
        if (worldItemPrefab != null)
        {
            Vector3 playerPos = GameManager.Instance.Player.transform.position;

            // sağ -> sol -> yukarı -> aşağı kontrol
            Vector3[] dirs = { Vector3.right, Vector3.left, Vector3.up, Vector3.down };
            Vector3 dropPos = playerPos;

            foreach (var dir in dirs)
            {
                Vector3 tryPos = playerPos + dir;
                if (!Physics2D.OverlapCircle(tryPos, 0.4f))
                {
                    dropPos = tryPos;
                    break;
                }
            }

            GameObject drop = Instantiate(worldItemPrefab, dropPos, Quaternion.identity);

            var worldItem = drop.GetComponent<WorldItem>();
            if (worldItem != null)
            {
                worldItem.Init(item, amount);
            }
        }
        else
        {
            Debug.LogWarning("WorldItem prefab atanmadı!");
        }
    }

    public void EquipItem(int index)
    {
        if (inventoryItems[index] == null) return;

        InventoryItem item = inventoryItems[index];

        // ❌ sadece Weapon veya Armor kuşanılabilir
        if (item.ItemType != ItemType.Weapon && item.ItemType != ItemType.Armor)
        {
            Debug.LogWarning($"❌ {item.Name} kuşanılamaz! Sadece silah ve zırh kuşanılabilir.");
            return;
        }

        // ✅ kuşanma işlemi
        EquipmentUI.Instance.AssignItemToSlot(item);

        // envanterden düş
        inventoryItems[index] = null;
        InventoryUI.Instance.DrawItem(null, index);

        SaveInventory();
    }


    public void UnequipItem(InventoryItem item)
    {
        if (item == null) return;
        AddItem(item, 1);
        SaveInventory();
    }

    private void AddItemFreeSlot(InventoryItem item, int quantity)
    {
        for (int i = 0; i < inventorySize; i++)
        {
            if (inventoryItems[i] != null) continue;
            inventoryItems[i] = item.CopyItem();
            inventoryItems[i].Quantity = quantity;
            InventoryUI.Instance.DrawItem(inventoryItems[i], i);
            return;
        }
    }

    private void DecreaseItemStack(int index)
    {
        inventoryItems[index].Quantity--;
        if (inventoryItems[index].Quantity <= 0)
        {
            inventoryItems[index] = null;
            InventoryUI.Instance.DrawItem(null, index);
        }
        else
        {
            InventoryUI.Instance.DrawItem(inventoryItems[index], index);
        }
    }

    public void ConsumeItem(string itemID)
    {
        List<int> indexes = CheckItemStockIndexes(itemID);
        if (indexes.Count > 0)
        {
            DecreaseItemStack(indexes[^1]);
        }
    }

    private List<int> CheckItemStockIndexes(string itemID)
    {
        List<int> itemIndexes = new List<int>();
        for (int i = 0; i < inventoryItems.Length; i++)
        {
            if (inventoryItems[i] == null) continue;
            if (inventoryItems[i].ID == itemID)
            {
                itemIndexes.Add(i);
            }
        }
        return itemIndexes;
    }

    public int GetItemCurrentStock(string itemID)
    {
        List<int> indexes = CheckItemStockIndexes(itemID);
        int currentStock = 0;
        foreach (int index in indexes)
        {
            if (inventoryItems[index].ID == itemID)
            {
                currentStock += inventoryItems[index].Quantity;
            }
        }
        return currentStock;
    }

    private void VerifyItemsForDraw()
    {
        for (int i = 0; i < inventorySize; i++)
        {
            if (inventoryItems[i] == null)
            {
                InventoryUI.Instance.DrawItem(null, i);
            }
        }
    }

    private InventoryItem ItemExistsInGameContent(string itemID)
    {
        for (int i = 0; i < gameContent.GameItems.Length; i++)
        {
            if (gameContent.GameItems[i].ID == itemID)
            {
                return gameContent.GameItems[i];
            }
        }
        return null;
    }

    private void LoadInventory()
    {
        if (SaveGame.Exists(INVENTORY_KEY_DATA))
        {
            InventoryData loadData = SaveGame.Load<InventoryData>(INVENTORY_KEY_DATA);
            for (int i = 0; i < inventorySize; i++)
            {
                if (loadData.ItemContent[i] != null)
                {
                    InventoryItem itemFromContent = ItemExistsInGameContent(loadData.ItemContent[i]);
                    if (itemFromContent != null)
                    {
                        inventoryItems[i] = itemFromContent.CopyItem();
                        inventoryItems[i].Quantity = loadData.ItemQuantity[i];
                        InventoryUI.Instance.DrawItem(inventoryItems[i], i);
                    }
                }
                else
                {
                    inventoryItems[i] = null;
                }
            }
        }
    }

    private void SaveInventory()
    {
        InventoryData saveData = new InventoryData();
        saveData.ItemContent = new string[inventorySize];
        saveData.ItemQuantity = new int[inventorySize];
        for (int i = 0; i < inventorySize; i++)
        {
            if (inventoryItems[i] == null)
            {
                saveData.ItemContent[i] = null;
                saveData.ItemQuantity[i] = 0;
            }
            else
            {
                saveData.ItemContent[i] = inventoryItems[i].ID;
                saveData.ItemQuantity[i] = inventoryItems[i].Quantity;
            }
        }
        SaveGame.Save(INVENTORY_KEY_DATA, saveData);
    }

    public InventoryItem GetItemFromContent(string itemID)
    {
        for (int i = 0; i < gameContent.GameItems.Length; i++)
        {
            if (gameContent.GameItems[i] != null && gameContent.GameItems[i].ID == itemID)
            {
                return gameContent.GameItems[i];
            }
        }
        return null;
    }

    public bool HasItem(string itemID)
    {
        for (int i = 0; i < inventoryItems.Length; i++)
        {
            if (inventoryItems[i] == null) continue;
            if (inventoryItems[i].ID == itemID && inventoryItems[i].Quantity > 0)
            {
                return true;
            }
        }   
        return false;
    }
}
