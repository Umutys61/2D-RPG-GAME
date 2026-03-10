using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public enum ResourceType
{
    Wood,
    Ore,
    Fish
}

public class ResourceNode : MonoBehaviour
{
    [Header("Config")]
    public string itemID;
    public int amount;
    public float gatherTime;
    public float respawnTime;

    [Header("Type")]
    public ResourceType resourceType;

    [Header("UI")]
    public GameObject progressBarPrefab;

    [Header("Feedback")]
    public GameObject bonusTextPrefab; 

    private Slider progressBarInstance;
    private GameObject progressBarGO;

    private bool isAvailable = true;
    private SpriteRenderer sr;
    private Collider2D col;

    private Coroutine gatherCoroutine;

    private void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
        col = GetComponent<Collider2D>();
    }

    public void Gather()
    {
        if (!isAvailable) return;

        if (!HasRequiredTool())
        {
            Debug.Log($"❌ {resourceType} toplamak için gerekli alet yok!");
            return;
        }

        if (gatherCoroutine != null)
            StopCoroutine(gatherCoroutine);

        gatherCoroutine = StartCoroutine(GatherRoutine());
    }

    private bool HasRequiredTool()
    {
        switch (resourceType)
        {
            case ResourceType.Wood: return Inventory.Instance.HasItem("ItemAxe");
            case ResourceType.Ore: return Inventory.Instance.HasItem("ItemPickaxe");
            case ResourceType.Fish: return Inventory.Instance.HasItem("ItemFishingRod");
            default: return true;
        }
    }

    private IEnumerator GatherRoutine()
    {
        isAvailable = false;

        float actualTime = gatherTime * GameManager.Instance.Player.Stats.GetGatherTimeMultiplier(resourceType);

        if (progressBarPrefab != null)
        {
            progressBarGO = Instantiate(progressBarPrefab, transform.position + Vector3.up * 1.5f, Quaternion.identity);
            progressBarGO.transform.SetParent(transform);

            progressBarInstance = progressBarGO.GetComponentInChildren<Slider>();
            progressBarInstance.minValue = 0;
            progressBarInstance.maxValue = actualTime;
            progressBarInstance.value = 0;
        }

        float elapsed = 0f;
        while (elapsed < actualTime)
        {
            elapsed += Time.deltaTime;
            if (progressBarInstance != null)
                progressBarInstance.value = elapsed;
            yield return null;
        }

        if (progressBarGO != null) Destroy(progressBarGO);

        InventoryItem item = Inventory.Instance.GetItemFromContent(itemID);
        if (item != null)
        {
            int finalAmount = amount;

            int level = 0;
            switch (resourceType)
            {
                case ResourceType.Wood: level = GameManager.Instance.Player.Stats.woodcuttingLevel; break;
                case ResourceType.Ore: level = GameManager.Instance.Player.Stats.miningLevel; break;
                case ResourceType.Fish: level = GameManager.Instance.Player.Stats.fishingLevel; break;
            }

            float bonusChance = level * 0.01f;
            bonusChance = Mathf.Clamp01(bonusChance);
            if (Random.value < bonusChance)
            {
                finalAmount += 1;

                if (bonusTextPrefab != null)
                {
                    var bonusText = Instantiate(bonusTextPrefab, transform.position + Vector3.up * 1.5f, Quaternion.identity);
                    bonusText.transform.rotation = Quaternion.identity;
                    Destroy(bonusText, 1.5f);
                }
            }

            Inventory.Instance.AddItem(item, finalAmount);
            GameManager.Instance.Player.Stats.AddGatherExp(resourceType, 1);
        }

        if (sr != null) sr.enabled = false;
        if (col != null) col.enabled = false;

        yield return new WaitForSeconds(respawnTime);

        if (sr != null) sr.enabled = true;
        if (col != null) col.enabled = true;
        isAvailable = true;

        gatherCoroutine = null;

        var playerInteraction = FindFirstObjectByType<PlayerInteraction>();
        if (playerInteraction != null)
            playerInteraction.ClearTarget();
    }

    public void CancelGather()
    {
        if (gatherCoroutine != null)
        {
            StopCoroutine(gatherCoroutine);
            gatherCoroutine = null;
            if (progressBarGO != null) Destroy(progressBarGO);
            isAvailable = true;
        }

        var playerInteraction = FindFirstObjectByType<PlayerInteraction>();
        if (playerInteraction != null)
            playerInteraction.ClearTarget();
    }
}
