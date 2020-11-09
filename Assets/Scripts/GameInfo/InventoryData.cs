using UnityEngine;
using UnityEngine.UI;
using System.IO;
using System.Collections.Generic;
using System.Runtime.Serialization;

[System.Serializable]
public class InventoryData
{    
    public List<Item> items;

    public InventoryData (Inventory inventory)
    {
        items = inventory.items;
    }

}
