using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum InventoryStatus
{
    Player,
    Seller

}

public class Inventory : MonoBehaviour
{
    [SerializeField] private WalletController _walletController;
    [SerializeField] private List<ItemParameters> _itemParameters;
    [SerializeField] private Item _itemPrefab;

    [SerializeField] public InventoryStatus InventoryStatus;

    private List<Item> _items;

    private void Start()
    {
        CreateItems();
    }
    private void CreateItems()
    {
        _items = new List<Item>(_itemParameters.Count);
        foreach (var parameter in _itemParameters)
        {
            var item = Instantiate<Item>(_itemPrefab, transform);
            item.SetItemData(parameter);
            _items.Add(item);
        }
    }

    public bool TryBuyItem(Item item)
    {
        if (_walletController.TrySubtract(item.CurrentPrice))
        {
            _items.Add(item);
            return true;
        }

        return false;
    }

    public void SellItem(Item item)
    {
        _walletController.AddСurrency(item.CurrentPrice);
        _items.Remove(item);
    }
}