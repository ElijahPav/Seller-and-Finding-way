using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;


public class TradeController : MonoBehaviour
{

    [SerializeField] private GraphicRaycaster raycaster;
    [SerializeField] private EventSystem eventSystem;

    private PointerEventData _pointerEventData;
    private Item _currentItem;

#if UNITY_EDITOR

    private void Reset()
    {
        raycaster = GetComponent<GraphicRaycaster>();
        eventSystem = FindObjectOfType<EventSystem>();

        if (raycaster == null || eventSystem == null)
        {
            EditorUtility.DisplayDialog("Ui raycaster", "please add ui on canvas only", "OK");
            DestroyImmediate(this);
        }
    }
#endif


    private void Start()
    {
        Item.ItemSelected += SelectItem;
    }
    private void Update()
    {
        if (Input.GetKeyUp(KeyCode.Mouse0) && _currentItem != null)
        {
            var inventory = GetInventoryUnderMouse();
            if (inventory != null)
            {
                TrySellItem(inventory);
            }
            UnselectItem();

        }
    }

    private Inventory GetInventoryUnderMouse()
    {
        _pointerEventData = new PointerEventData(eventSystem)
        {
            position = Input.mousePosition
        };
        var result = new List<RaycastResult>();
        raycaster.Raycast(_pointerEventData, result);

        foreach (var raycastResalt in result)
        {
            if (raycastResalt.gameObject.GetComponent<Inventory>())
            {
                return (raycastResalt.gameObject.GetComponent<Inventory>());
            }
        }
        return null;
    }

    public void SelectItem(Item item)
    {
        _currentItem = item;
        _currentItem.UnpinItem(this.transform);
    }

    private void UnselectItem()
    {
        _currentItem.Unselect();
        _currentItem = null;
    }

    public bool TrySellItem(Inventory inventory)
    {
        if (_currentItem.IsAnotherInventory(inventory) && inventory.TryBuyItem(_currentItem))
        {
            _currentItem.ChangeOwner(inventory);
            return true;
        }
        return false;

    }
    private void OnDestroy()
    {
        Item.ItemSelected -= SelectItem;
    }

}