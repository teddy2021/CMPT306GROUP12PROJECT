using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CraftingMenu : MonoBehaviour
{
    public static bool crafting = false;
    public GameObject craftingMenuUI;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) || Input.GetKeyDown(KeyCode.I))
        {
            if (crafting)
            {
                Resume();
            }
            else
            {
                Craft();
            }
        }
    }

    public void Resume()
    {
        craftingMenuUI.SetActive(false);
        crafting = false;
    }

    public void Craft()
    {
        craftingMenuUI.SetActive(true);
        crafting = true;
    }
}
