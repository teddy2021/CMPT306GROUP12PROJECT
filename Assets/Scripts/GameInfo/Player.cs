using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class Player : MonoBehaviour
{
    public int health;
    public new string name;
    public int floor;
    public int gold;

    public Player(string playerName = "Darvin", int playerHealth = 100, int currentFloor = 1)
    {
        health = playerHealth;
        name = playerName;
        floor = currentFloor;
        gold = 0;
    }

    public void SavePlayer()
    {
        Debug.Log(name);

        SaveLoad.SavePlayer(this);
    }

    public void LoadPlayer(Text nameInput)
    {
        PlayerData data = SaveLoad.LoadPlayer();

        Player player = GameObject.Find("Player").GetComponent<Player>();
        name = data.playerName;
        health = data.playerHealth;
        floor = data.currentFloor;
        gold = data.gold;

        Debug.Log(name);

        Text floorText = GetTextObjectByName("CurrentFloorText");
        floorText.text = floor.ToString();

        Text healthText = GetTextObjectByName("HealthText");
        healthText.text = health.ToString();

        Text goldText = GetTextObjectByName("GoldText");
        goldText.text = gold.ToString();    
    }

    private Text GetTextObjectByName(string name)
    {
        GameObject canvas = GameObject.Find("Canvas");
        var texts = canvas.GetComponentsInChildren<Text>();
        return texts.FirstOrDefault(textObject => textObject.name == name);
    }

    #region UI Methods

    public void ChangeFloor(int amount)
    {
        if ((floor > 1) || (amount > 0))
        {
            floor += amount;
            Text floorText = GetTextObjectByName("CurrentFloorText");
            floorText.text = floor.ToString();

            
        }
        if (amount > 0)
        {
            State state = GameObject.Find("State").GetComponent<State>();
            if (floor > state.floorsExplored)
            {
                state.AddFloor(1);
            }
        }

    }

    public void ChangeHealth(int amount)
    {
        if (health > 0 || amount > 0)
        {
            health += amount;
            Text healthText = GetTextObjectByName("HealthText");
            healthText.text = health.ToString();
        }
        
    }

    //Gold may run into the negatives
    public void ChangeGold(int amount)
    {
        gold += amount;
        Text goldText = GetTextObjectByName("GoldText");
        goldText.text = gold.ToString();    
    }

    public void ChangeName(string val)
    {
        name = val;
        Debug.Log(name);
    }

    #endregion

    public void DeletePlayer()
    {
        SaveLoad.DeletePlayer();
    }

}
