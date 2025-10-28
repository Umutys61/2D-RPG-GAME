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

        // Eski butonları temizle
        for (int i = 0; i < container.childCount; i++)
        {
            Destroy(container.GetChild(i).gameObject);
        }

        // Yeni butonları ekle
        foreach (DropItem item in enemyLoot.Items)
        {
            if (item.PickedItem) continue;
            LootButton lootButton = Instantiate(lootButtonPrefab, container);
            lootButton.ConfigLootButton(item);
        }

        // Eğer hala item varsa paneli aç
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
