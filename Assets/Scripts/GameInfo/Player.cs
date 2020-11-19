using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class Player : MonoBehaviour
{
    public int health;
    public new string name;
    public int gold;

    public Player(string playerName = "Darvin", int playerHealth = 100)
    {
        health = playerHealth;
        name = playerName;
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
        gold = data.gold;

        Debug.Log(name);
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
