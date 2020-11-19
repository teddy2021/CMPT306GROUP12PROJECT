using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class PauseMenu : MonoBehaviour
{
    public GameObject pauseMenuUI;

    public TextMeshProUGUI floorText;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (GameController.GameIsPaused)
            {
                Resume();
            } else
            {
                Pause();
            }
            
        }
        floorText.text = GameController.FloorLevel.ToString();
    }

    public void Resume ()
    {
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1f;
        GameController.GameIsPaused = false;
    }

    void Pause ()
    {
        pauseMenuUI.SetActive(true);
        Time.timeScale = 0f;
        GameController.GameIsPaused = true;
    }

    public void LoadMenu (string sceneName="MainMenu")
    {
        Time.timeScale = 1f;
        //SceneManager.LoadScene("MainMenu");
    }

    public void QuitGame ()
    {
        UnityEngine.Debug.Log("Quitting Game...");
        Application.Quit();
    }
}
