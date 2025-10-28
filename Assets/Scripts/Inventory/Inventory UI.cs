using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InventoryUI : Singleton<InventoryUI>
{
    [Header("Config")]
    [SerializeField] private GameObject inventoryPanel;
    [SerializeField] private InventorySlot slotPrefab;
    [SerializeField] private Transform container;

    [Header("Description Panel")]
    [SerializeField] private GameObject descriptionPanel;
    [SerializeField] private Image itemIcon;
    [SerializeField] private TextMeshProUGUI itemName;
    [SerializeField] private TextMeshProUGUI itemDescriptionTMP;

    [Header("Remove Panel")]
    [SerializeField] private GameObject removePanel;
    [SerializeField] private TextMeshProUGUI removeTitleText;   // hangi item silinecek
    [SerializeField] private Slider removeSlider;
    [SerializeField] private TextMeshProUGUI removeQuantityText;
    [SerializeField] private Button confirmRemoveButton;
    [SerializeField] private Button cancelRemoveButton;

    [Header("Drop Panel")]
    [SerializeField] private GameObject dropPanel; 
    [SerializeField] private Slider quantitySlider;
    [SerializeField] private TextMeshProUGUI quantityText;
    [SerializeField] private Button confirmDropButton;
    [SerializeField] private Button cancelDropButton;

    private int pendingRemoveIndex = -1;
    private int pendingDropIndex = -1;

    public InventorySlot CurrentSlot { get; set; }
    private List<InventorySlot> slotList = new List<InventorySlot>();

    // ✅ Singleton korunsun diye override
    protected override void Awake()
    {
        base.Awake();

        if (confirmRemoveButton != null)
            confirmRemoveButton.onClick.AddListener(ConfirmRemoveItem);

        if (cancelRemoveButton != null)
            cancelRemoveButton.onClick.AddListener(CancelRemoveItem);

        if (confirmDropButton != null)
            confirmDropButton.onClick.AddListener(ConfirmDrop);

        if (cancelDropButton != null)
            cancelDropButton.onClick.AddListener(CancelDrop);

        if (removePanel != null)
            removePanel.SetActive(false);

        if (dropPanel != null)
            dropPanel.SetActive(false);
    }

    private void Start()
    {
        InitInventory();
    }

    private void InitInventory()
    {
        for (int i = 0; i < Inventory.Instance.InventorySize; i++)
        {
            InventorySlot slot = Instantiate(slotPrefab, container);
            slot.Index = i;
            slotList.Add(slot);
        }
    }

    public void DrawItem(InventoryItem item, int index)
    {
        InventorySlot slot = slotList[index];
        if (item == null)
        {
            slot.ShowSlotInformation(false);
            return;
        }
        slot.ShowSlotInformation(true);
        slot.UpdateSlot(item);
    }

    public void UseItem()
    {
        if (CurrentSlot == null) return;
        Inventory.Instance.UseItem(CurrentSlot.Index);
    }

    // ---------------- REMOVE SYSTEM ----------------
    public void RequestRemoveCurrent()
    {
        if (CurrentSlot == null) return;
        RequestRemoveItem(CurrentSlot.Index);
    }

    public void RequestRemoveItem(int index)
    {
        pendingRemoveIndex = index;
        var item = Inventory.Instance.InventoryItems[index];
        if (item == null) return;

        // Başlık
        removeTitleText.text = $"{item.Name} - Silinecek miktarı seç:";

        // Slider ayarları
        removeSlider.minValue = 1;
        removeSlider.maxValue = item.Quantity;
        removeSlider.value = 1;

        // İlk miktar
        removeQuantityText.text = "1";

        // Paneli aç
        removePanel.SetActive(true);
    }

    public void OnRemoveSliderChanged()
    {
        removeQuantityText.text = removeSlider.value.ToString("0");
    }

    public void ConfirmRemoveItem()
    {
        if (pendingRemoveIndex == -1) return;

        int amount = Mathf.RoundToInt(removeSlider.value);
        Inventory.Instance.RemoveItemAmount(pendingRemoveIndex, amount);

        removePanel.SetActive(false);
        pendingRemoveIndex = -1;
    }

    public void CancelRemoveItem()
    {
        pendingRemoveIndex = -1;
        removePanel.SetActive(false);
    }
    // -------------------------------------------------

    // ---------------- DROP SYSTEM ----------------
    public void OpenDropPanel()
    {
        if (CurrentSlot == null) return;

        pendingDropIndex = CurrentSlot.Index;
        var item = Inventory.Instance.InventoryItems[pendingDropIndex];
        if (item == null) return;

        quantitySlider.minValue = 1;
        quantitySlider.maxValue = item.Quantity;
        quantitySlider.value = 1;
        quantityText.text = "1";

        dropPanel.SetActive(true);
    }

    public void OnQuantitySliderChanged()
    {
        quantityText.text = quantitySlider.value.ToString("0");
    }

    public void ConfirmDrop()
    {
        if (pendingDropIndex == -1) return;

        int amount = Mathf.RoundToInt(quantitySlider.value);
        Inventory.Instance.DropItem(pendingDropIndex, amount);

        dropPanel.SetActive(false);
        pendingDropIndex = -1;
    }

    public void CancelDrop()
    {
        pendingDropIndex = -1;
        dropPanel.SetActive(false);
    }
    // -------------------------------------------------

    public void EquipItem()
    {
        if (CurrentSlot == null) return;
        Inventory.Instance.EquipItem(CurrentSlot.Index);
    }

     public void ShowItemDescription(int index)
{
    if (Inventory.Instance.InventoryItems[index] == null) return;

    descriptionPanel.SetActive(true);
    var item = Inventory.Instance.InventoryItems[index];

    itemIcon.sprite = item.Icon;
    itemName.text = item.Name;
    itemDescriptionTMP.text = item.GetDescription(); // 🔥 artık dinamik açıklama
}


    public void OpenCloseInventory()
    {
        inventoryPanel.SetActive(!inventoryPanel.activeSelf);
        if (!inventoryPanel.activeSelf)
        {
            descriptionPanel.SetActive(false);
            CurrentSlot = null;
        }
    }

    private void SlotSelectedCallback(int slotIndex)
    {
        CurrentSlot = slotList[slotIndex];
        ShowItemDescription(slotIndex);
    }

    private void OnEnable()
    {
        InventorySlot.OnSlotSelectedEvent += SlotSelectedCallback;
    }

    private void OnDisable()
    {
        InventorySlot.OnSlotSelectedEvent -= SlotSelectedCallback;
    }
}
