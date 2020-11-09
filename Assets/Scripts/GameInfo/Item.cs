using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System.Runtime.Serialization;

[System.Serializable()]
public class Item
{
    public string name;
    public int quantity;
    public int value;

    public Item(string itemName, int itemQuantity=1, int itemValue=0)
    {
        name = itemName;
        quantity = itemQuantity;
        value = itemValue;
    }

    public string Print()
    {
        return name + " x" + quantity + "("+quantity*value+"gp)";
    }
}

