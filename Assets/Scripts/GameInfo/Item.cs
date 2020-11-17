using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System.Runtime.Serialization;

[System.Serializable()]
public class Item
{
    public string name;
    public int quantity;

    public Item(string itemName, int itemQuantity=1)
    {
        name = itemName;
        quantity = itemQuantity;
    }

    public string Print()
    {
        return name + " x" + quantity;
    }
}

