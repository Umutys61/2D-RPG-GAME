using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ShopCard : MonoBehaviour
{
    [Header("Config")]
    [SerializeField] private Image ItemIcon;
    [SerializeField] private TextMeshProUGUI ItemName;
    [SerializeField] private TextMeshProUGUI itemCost;
    [SerializeField] private TextMeshProUGUI buyAmount;

    private ShopItem item;
    private int quantity;
    private float initialCost;
    private float currentCost;

    private void Update()
    {
        buyAmount.text = quantity.ToString();
        itemCost.text = currentCost.ToString();
    }
    public void ConfigShopCard(ShopItem shopItem)
    {
        item = shopItem;
        ItemIcon.sprite = shopItem.Item.Icon;
        ItemName.text = shopItem.Item.Name;
        itemCost.text = shopItem.Cost.ToString();
        quantity = 1;
        initialCost = shopItem.Cost;
        currentCost = shopItem.Cost;
    }
    public void buyItem()
    {
        if (CoinManager.Instance.Coins >= currentCost)
        {
            Inventory.Instance.AddItem(item.Item, quantity);
            CoinManager.Instance.RemoveCoins(currentCost);
            quantity = 1;
            currentCost = initialCost;
        }
    }
    public void Add()
    {
        float buyCost = initialCost * (quantity + 1);
        if (CoinManager.Instance.Coins >= buyCost)
        {
            quantity++;
            currentCost = initialCost * quantity;
        }
    }
    public void Remove()
    {
        if (quantity == 1) return;
        quantity--;
        currentCost = initialCost * quantity;
    }

}
