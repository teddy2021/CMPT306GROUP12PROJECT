using UnityEngine;
using UnityEngine.UI;
using System.IO;
using System.Collections.Generic;
using System.Runtime.Serialization;

[System.Serializable]
public class InventoryData
{    
    public Item[] items = new Item[6];

    public InventoryData (Inventory inventory)
    {
        items = inventory.items;
    }

}
