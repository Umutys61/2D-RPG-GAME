using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class EnemyLoot : MonoBehaviour
{
    [Header("Config")]
    [SerializeField] private float expDrop;
    [SerializeField] private DropItem[] dropItems;

    public List<DropItem> Items { get; private set; }
    public float ExpDrop => expDrop;

    // ✅ sadece loot listesi hazırlar
    public void DropLoot()
    {
        Items = new List<DropItem>();

        foreach (DropItem item in dropItems)
        {
            if (item.Item == null) continue;

            float prob = Random.Range(0f, 100f);
            if (prob <= item.DropChance)
            {
                DropItem newDrop = new DropItem
                {
                    Name = item.Name,
                    Item = item.Item,
                    Quantity = item.Quantity,
                    DropChance = item.DropChance,
                    PickedItem = false
                };

                Items.Add(newDrop);
            }
        }
    }
}

[Serializable]
public class DropItem
{
    [Header("Config")]
    public string Name;
    public InventoryItem Item;
    public int Quantity;

    [Header("Drop Chance (%)")]
    [Range(0, 100)] public float DropChance;

    public bool PickedItem { get; set; }
}
