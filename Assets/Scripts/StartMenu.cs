using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartMenu : MonoBehaviour
{
    public GameObject startMenu;

    public void StartGame ()
    {
        SceneManager.LoadScene("Game");
    }

    public void EndGame ()
    {
        Application.Quit();
    }
}
