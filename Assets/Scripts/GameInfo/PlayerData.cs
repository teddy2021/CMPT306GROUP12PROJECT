using UnityEngine;
using UnityEngine.UI;
using System.IO;
using System.Collections.Generic;
using System.Runtime.Serialization;

[System.Serializable]
public class PlayerData
{ 

    public int playerHealth;
    public string playerName;
    public int gold;

    public PlayerData (Player player)
    {

        playerHealth = player.health;
        playerName = player.name;
        gold = player.gold;
    }

}
