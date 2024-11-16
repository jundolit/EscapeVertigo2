using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryController : MonoBehaviour
{
    [SerializeField]
    private UIInventoryPage inventoryUI;

    [SerializeField]
    private InventorySO inventoryData;

    public List<InventoryItem> initialItems = new List<InventoryItem>();


    private void Start()
    {
        Debug.Log("Start method called.");
        PrepareUI();
        //inventoryData.Initialize();
    }  

    private void PrepareUI()
    {
        if (inventoryUI == null) 
        {
            Debug.LogError("inventoryUI is null.");
            return;
        }

        Debug.Log("Preparing UI.");
        inventoryUI.InitializeInventoryUI(inventoryData.Size);
        this.inventoryUI.OnDescriptionRequested += HandleDescriptionRequest;
        this.inventoryUI.OnSwapItems += HandleSwapItems;
        this.inventoryUI.OnStartDragging += HandleDragging;
        this.inventoryUI.OnItemActionRequested += HandleItemActionRequest;

    }

    private void HandleItemActionRequest(int itemIndex)
    {
        
    }

    private void HandleDragging(int itemIndex)
    {
        
    }

    private void HandleSwapItems(int itemIndex_1, int itemIndex_2)
    {
        
    }

    private void HandleDescriptionRequest(int itemIndex)
    {
        if (inventoryData == null)
        {
            Debug.LogError("inventoryData is null."); // Ãß°¡
            return;
        }
        InventoryItem inventoryItem = inventoryData.GetItemAt(itemIndex);
        if (inventoryItem.IsEmpty)
        {
            Debug.Log("Selected item is empty.");
            inventoryUI.ResetSelection();
            return;
        }
        
        ItemSO item = inventoryItem.item;
        inventoryUI.UpdateDescription(itemIndex, item.ItemImage, item.name, item.Description);
    }

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.I))
        {
            if (inventoryUI.isActiveAndEnabled == false)
            {
                inventoryUI.Show();
                foreach (var item in inventoryData.GetCurrentInventoryState()) 
                {
                    inventoryUI.UpdateData(item.Key, 
                        item.Value.item.ItemImage, 
                        item.Value.quantity);
                }             
            }
            else 
            {
                inventoryUI.Hide();
            }
        }
    }
}
