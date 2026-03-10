using UnityEngine;

public class LootManager : Singleton<LootManager>
{
    [Header("Config")]
    [SerializeField] private GameObject lootPanel;
    [SerializeField] private LootButton lootButtonPrefab;
    [SerializeField] private Transform container;

    public void ShowLoot(EnemyLoot enemyLoot)
    {
        if (enemyLoot == null || enemyLoot.Items == null || enemyLoot.Items.Count == 0)
        {
            Debug.Log("❌ Loot yok, panel açılmadı.");
            return;
        }

        for (int i = 0; i < container.childCount; i++)
        {
            Destroy(container.GetChild(i).gameObject);
        }

        foreach (DropItem item in enemyLoot.Items)
        {
            if (item.PickedItem) continue;
            LootButton lootButton = Instantiate(lootButtonPrefab, container);
            lootButton.ConfigLootButton(item);
        }

        if (container.childCount > 0)
        {
            lootPanel.SetActive(true);
        }
        else
        {
            lootPanel.SetActive(false);
        }
    }

    public void ClosePanel()
    {
        lootPanel.SetActive(false);
    }
}
