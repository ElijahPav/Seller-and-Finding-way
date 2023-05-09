using System;
using System.Text;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;


public class Item : MonoBehaviour
{
    static public event Action<Item> ItemSelected;

    [SerializeField] private TextMeshProUGUI _priceText;
    [SerializeField] private Image _itemImage;

    private ItemParameters _itemParameters;

    public int CurrentPrice { get; private set; }
   

    private Inventory _owner;

    public bool IsAnotherInventory(Inventory anotherOwner) => _owner != anotherOwner;

    private void Awake()
    {
        _owner = transform.parent.gameObject.GetComponent<Inventory>();
    }

    public void SetItemData(ItemParameters itemParameters)
    {
        _itemParameters = itemParameters;
        _itemImage.sprite = _itemParameters.Image;
        _itemImage.SetNativeSize();
        UpdatePrice(_owner);

    }
    public void Select()
    {
        ItemSelected?.Invoke(this);
    }

    public void Unselect()
    {
        transform.SetParent(_owner.transform);
    }

    public void UnpinItem(Transform parent)
    {
        transform.SetParent(parent);
    }

    public void ChangeOwner(Inventory newOwner)
    {
        if (IsAnotherInventory(newOwner))
        {
            _owner.SellItem(this);
            _owner = newOwner;
            transform.SetParent(_owner.transform);

            UpdatePrice(_owner);
        }
    }

    private void UpdatePrice(Inventory inventory)
    {
        switch (_owner.InventoryStatus)
        {
            case InventoryStatus.Seller:
                CurrentPrice = _itemParameters.SellerPrice;
                _priceText.text = CurrentPrice.ToString();
                break;
            case InventoryStatus.Player:
                CurrentPrice = _itemParameters.PlayerPrice;
                _priceText.text = CurrentPrice.ToString();
                break;
        }
    }

}