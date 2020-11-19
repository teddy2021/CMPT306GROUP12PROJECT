using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Collections.Generic;
using System.Runtime.Serialization;

public static class SaveLoad
{

    public static void SaveState(State state)
    {
        BinaryFormatter formatter = new BinaryFormatter();
        string path = Application.persistentDataPath + "/state.dat";

        FileStream stream = new FileStream(path, FileMode.Create);

        StateData data = new StateData(state);

        formatter.Serialize(stream, data);

        stream.Close();
    }

    public static void SavePlayer(Player player)
    {
        BinaryFormatter formatter = new BinaryFormatter();
        string path = Application.persistentDataPath + "/player.dat";

        FileStream stream = new FileStream(path, FileMode.Create);

        PlayerData data = new PlayerData(player);

        formatter.Serialize(stream, data);

        stream.Close();
    }

    public static void SaveInventory(Inventory inventory)
    {
        BinaryFormatter formatter = new BinaryFormatter();
        string path = Application.persistentDataPath + "/inventory.dat";

        FileStream stream = new FileStream(path, FileMode.Create);

        InventoryData data = new InventoryData(inventory);

        formatter.Serialize(stream, data);

        stream.Close();
    }


    public static StateData LoadState ()
    {
        string path = Application.persistentDataPath + "/state.dat";
        if (File.Exists(path))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);
            stream.Position = 0;

            StateData data = formatter.Deserialize(stream) as StateData;
            stream.Close();

            return data;
        } else
        {
            Debug.Log("state Not found in " + path + ". Creating new state File.");
            //Create mew empty state
            //generate a 20 char string for the floor code
            string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
            char[] stringChars = new char[20];

            System.Random random = new System.Random();

            for (int i = 0; i < stringChars.Length; i++)
            {
                stringChars[i] = chars[random.Next(chars.Length)];
            }

            string floorCode = new string(stringChars);

            State state = GameObject.Find("State").GetComponent<State>();

            //No State so create a default

            state.floorsExplored = 1;
            state.floorCodes.Add(floorCode);

            return new StateData(state);
        }
    }

    public static PlayerData LoadPlayer ()
    {
        string path = Application.persistentDataPath + "/player.dat";
        if (File.Exists(path))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);
            stream.Position = 0;

            PlayerData data = formatter.Deserialize(stream) as PlayerData;
            stream.Close();

            return data;
        } else
        {
            Debug.Log("player Not found in " + path + ". Creating new player File.");
            //Create mew empty player

            Player player = GameObject.Find("Player").GetComponent<Player>();
            
            //No previous load state so create default objects

            player.name = "Darvin";
            player.health = 100;
            player.gold = 0;

            return new PlayerData(player);
        }
    }

    public static InventoryData LoadInventory ()  
    {
        string path = Application.persistentDataPath + "/inventory.dat";
        if (File.Exists(path))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);
            stream.Position = 0;

            InventoryData data = formatter.Deserialize(stream) as InventoryData;
            stream.Close();

            return data;
        } else
        {
            Debug.Log("inventory Not found in " + path + ". Creating new inventory File.");
            //Create mew empty player

            Inventory inventory = GameObject.Find("Inventory").GetComponent<Inventory>();
            
            //No previous load state so create default objects

            inventory.items = new Item[6];

            return new InventoryData(inventory);
        }
    }

    public static void DeleteState() 
    {
        string path3 = Application.persistentDataPath + "/state.dat";

        File.Delete(path3);
    }

    public static void DeletePlayer() 
    {
        string path2 = Application.persistentDataPath + "/player.dat";

        File.Delete(path2);
    }

    public static void DeleteInventory() 
    {
        string path1 = Application.persistentDataPath + "/inventory.dat";

        File.Delete(path1);
    }

}
