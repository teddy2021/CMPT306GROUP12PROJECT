using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using TMPro;

public class Inventory : MonoBehaviour
{
    public List<Item> items;

    //This can be used to add an item to the inventory, every item is unique
    //An item quantity may be negative
    #region UI Methods
    
    public void AddItem(Item item) 
    {
        UpdateList(item);
    }

    #endregion

    public void SaveInventory()
    {
        SaveLoad.SaveInventory(this);
    }

    public void LoadInventory()
    {
        InventoryData data = SaveLoad.LoadInventory();

        Inventory inventory = GameObject.Find("Inventory").GetComponent<Inventory>();
        items = data.items;
    }

    public void DeleteInventory()
    {
        SaveLoad.DeleteInventory();
    }

    public void UpdateList(Item item)
    {
    }
}
