using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using TMPro;

public class Inventory : MonoBehaviour
{
    public Item[] items = new Item[5];

    [SerializeField] private TextMeshProUGUI coalNum;
    [SerializeField] private TextMeshProUGUI copperNum;
    [SerializeField] private TextMeshProUGUI ironNum;
    [SerializeField] private TextMeshProUGUI postNum;
    [SerializeField] private TextMeshProUGUI furnaceNum;

    //This can be used to add an item to the inventory, every item is unique
    //An item quantity may be negative

    public void SaveInventory()
    {
        SaveLoad.SaveInventory(this);
    }

    public void LoadInventory()
    {
        InventoryData data = SaveLoad.LoadInventory();

        Inventory inventory = GameObject.Find("Inventory").GetComponent<Inventory>();
        items = data.items;
        UpdateUI();
    }

    public void DeleteInventory()
    {
        SaveLoad.DeleteInventory();
    }

    public void AddItem(Item item)
    {
        switch (item.name) 
        {
            case "coalChunk":
                if (items[0] != null)
                {
                    items[0].quantity += item.quantity;
                }
                else
                {
                    items[0] = item;
                }
                break;
            case "copperChunk":
                if (items[1] != null)
                {
                    items[1].quantity += item.quantity;
                }
                else
                {
                    items[1] = item;
                }
                break;
            case "ironChunk":
                if (items[2] != null)
                {
                    items[2].quantity += item.quantity;
                }
                else
                {
                    items[2] = item;
                }
                break;
            case "powerPost":
                if (items[3] != null)
                {
                    items[3].quantity += item.quantity;
                }
                else
                {
                    items[3] = item;
                }
                break;
            case "furnace":
                if (items[4] != null)
                {
                    items[4].quantity += item.quantity;
                }
                else
                {
                    items[4] = item;
                }
                break;
            default:
                break;
        }
        UpdateUI();
    }

    public void CraftPowerPost()
    {
        //Subtract recources if enough. Update ui and inventory
        if ((items[1] != null && items[2] != null) && (items[1].quantity >= 2 && items[2].quantity >= 3))
        {
            items[1].quantity = items[1].quantity - 2;
            items[2].quantity = items[2].quantity - 3;
            items[3].quantity ++;
            UpdateUI();
        }
    }

    public void CraftFurnace()
    {
        //Subtract recources if enough. Update ui and inventory
        //Subtract recources if enough. Update ui and inventory
        if (items[2].quantity != null && items[2].quantity >= 5)
        {
            items[2].quantity = items[2].quantity - 5;
            items[4].quantity++;
            UpdateUI();
        }

    }

    public void UpdateUI()
    {
        //Update all of the GameInfo elements ie item counters on bottom, key panel
        coalNum.text = items[0].quantity.ToString();
        copperNum.text = items[1].quantity.ToString();
        ironNum.text = items[2].quantity.ToString();
        postNum.text = items[3].quantity.ToString();
        furnaceNum.text = items[4].quantity.ToString();
    }

}
