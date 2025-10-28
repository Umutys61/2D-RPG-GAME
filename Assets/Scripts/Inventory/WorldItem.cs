using UnityEngine;

public class WorldItem : MonoBehaviour
{
    public InventoryItem ItemData { get; private set; }
    public int Quantity { get; private set; }

    private SpriteRenderer spriteRenderer;
    private bool playerInRange = false;
    private GameObject playerRef;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void Init(InventoryItem item, int amount)
    {
        ItemData = item.CopyItem();
        ItemData.Quantity = amount;
        Quantity = amount;

        if (spriteRenderer != null && ItemData.Icon != null)
            spriteRenderer.sprite = ItemData.Icon;
    }

    private void Update()
    {
        if (playerInRange && Input.GetKeyDown(KeyCode.F))
        {
            Debug.Log($"PLAYER aldı (F) → {ItemData.Name} x{Quantity}");
            Inventory.Instance.AddItem(ItemData, Quantity);
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = true;
            playerRef = other.gameObject;
            Debug.Log("Player yakında, F'ye basarak alabilir.");
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
            playerRef = null;
            Debug.Log("Player uzaklaştı.");
        }
    }
}
