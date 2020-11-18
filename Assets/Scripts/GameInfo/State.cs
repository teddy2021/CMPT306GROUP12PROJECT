using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;
using System.Linq;

public class State : MonoBehaviour
{
    public int floorsExplored;
    public List<string> floorCodes;
    public string lastPlayedText;

    public State(string floorCode, Player playerObject, Inventory inventoryObject)
    {
        floorsExplored = 1;
        if (floorCode == null)
        {
            floorCode = GenerateFloorCode();
        }
        floorCodes = new List<string>(){floorCode};

        lastPlayedText = System.DateTime.Now.ToString(@"dd\/MM\/yyyy HH:mm:ss");
    }    

    public void SaveState()
    {
        lastPlayedText = System.DateTime.Now.ToString(@"dd\/MM\/yyyy HH:mm:ss");
        SaveLoad.SaveState(this);
    }

    public void LoadState()
    {
        StateData data = SaveLoad.LoadState();

        floorCodes = data.floorCodes;
        floorsExplored = data.floorsExplored;
        lastPlayedText = data.lastPlayedText;
        if (lastPlayedText.Length == 0)
        {
            lastPlayedText = System.DateTime.Now.ToString(@"dd\/MM\/yyyy HH:mm:ss");
        }

        GetTextObjectByName("LastPlayedText").text = lastPlayedText;
        GetTextObjectByName("FloorText").text = floorsExplored.ToString();

    }

    public string GenerateFloorCode()
    {
        string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
        char[] stringChars = new char[20];

        System.Random random = new System.Random();

        for (int i = 0; i < stringChars.Length; i++)
        {
            stringChars[i] = chars[random.Next(chars.Length)];
        }

        return new string(stringChars);
    }

    #region UI Methods

    public void AddFloor(int amount) 
    {
        floorsExplored += amount;
        floorCodes.Add(this.GenerateFloorCode());
        Text floorText = GetTextObjectByName("FloorText");
        floorText.text = floorsExplored.ToString();
    }

    private Text GetTextObjectByName(string name)
    {
        GameObject canvas = GameObject.Find("Canvas");
        var texts = canvas.GetComponentsInChildren<Text>();
        return texts.FirstOrDefault(textObject => textObject.name == name);
    }

    #endregion

    public void DeleteState()
    {
        SaveLoad.DeleteState();
    }
}
