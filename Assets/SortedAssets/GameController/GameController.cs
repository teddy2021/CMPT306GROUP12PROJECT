﻿using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class GameController : MonoBehaviour
{
    public static int FloorLevel = 1;
    public static bool GameIsPaused = false;
    public static int TotalKeys;
    public static int KeysCollected;


    public static void StartGame(string reason, int newFloor = 1)
    {
        FloorLevel = newFloor;

        Place_PowerPole_Furnace objects = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Place_PowerPole_Furnace>();
        objects.clear_objects();

        SyncedMapCreator mapmaker = GameObject.FindGameObjectWithTag("MapMaking").GetComponent<SyncedMapCreator>();
        mapmaker.fixedSizeIncrease();
        //mapmaker.MaxKeys = Mathf.Max(1,FloorLevel/4);
        mapmaker.reinit();


        SetFloorText();
        KeysCollected = 0;
        //GameObject.Find("Key_Panel").SetActive(false);
    }
    void Update()
    {
    }

    public static void EndGame(string reason)
    {
        if (reason == "Death")
        {
            Debug.Log("Player Died");
        }
        Debug.Log(reason);
        SceneManager.LoadScene("Scenes/MainGame/EndMenu");
    }

    public static void SetKeyText()
    {
        int keysFound = 0;
        int allKeys = 0;
        foreach (var keyObj in GameObject.FindGameObjectsWithTag("Objective_Key"))
        {
            var keyComp = keyObj.GetComponent<Key>();

            if (keyComp.grabbed)
            {
                keysFound++;
            }
            allKeys++;
            if (keysFound > 0)
            {
                //GameObject.Find("Key_Panel").SetActive(true);
            }
        }
        GameController.KeysCollected = keysFound;
        GameController.TotalKeys = allKeys;
        TextMeshProUGUI keysRatio = GameObject.FindGameObjectsWithTag("Key_Ratio")[0].GetComponent<TextMeshProUGUI>();
        keysRatio.text = GameController.KeysCollected.ToString() + "/" + GameController.TotalKeys.ToString();
    }

    public static void SetFloorText()
    {
        TextMeshProUGUI floor = GameObject.FindGameObjectsWithTag("Floor_Num")[0].GetComponent<TextMeshProUGUI>();
        floor.text = ""+GameController.FloorLevel.ToString();
        KeysCollected = 0;
        Debug.Log("Floor Incremented "+floor.text);
    }

    public static void ShowHint(GameObject panel)
    {
        panel.SetActive(true);
    }

    public static void HideHint(GameObject panel)
    {
        panel.SetActive(false);
    }
}