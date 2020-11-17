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

    [SerializeField] private GameObject coalPanel;
    [SerializeField] private GameObject copperPanel;
    [SerializeField] private GameObject ironPanel;
    [SerializeField] private GameObject postPanel;
    [SerializeField] private GameObject furnacePanel;

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

        if (items[0].quantity < 1)
        {
            coalNum.text = items[0].quantity.ToString();
            coalPanel.SetActive(false);
        }
        else
        {
            coalPanel.SetActive(true);
        }

        if (items[1].quantity < 1)
        {
            copperNum.text = items[1].quantity.ToString();
            copperPanel.SetActive(false);
        }
        else
        {
            copperPanel.SetActive(true);
        }

        if (items[2].quantity < 1)
        {
            ironNum.text = items[2].quantity.ToString();
            ironPanel.SetActive(false);
        }
        else
        {
            ironPanel.SetActive(true);
        }

        if (items[3].quantity < 1)
        {
            postNum.text = items[3].quantity.ToString();
            postPanel.SetActive(false);
        }
        else
        {
            postPanel.SetActive(true);
        }

        if (items[4].quantity < 1)
        {
            furnaceNum.text = items[4].quantity.ToString();
            furnacePanel.SetActive(false);
        }
        else
        {
            furnacePanel.SetActive(true);
        }
    }

}
