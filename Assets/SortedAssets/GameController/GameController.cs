using UnityEngine;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    public static int FloorLevel = 0;
    public static bool GameIsPaused = false;

    public static void StartGame(string reason)
    {
        FloorLevel++;
        if (reason == "beginning")
        {
            FloorLevel = 1;
        }
        SceneManager.LoadScene("Scenes/MainGame/Game");
    }

    public static void EndGame(string reason)
    {
        SceneManager.LoadScene("Scenes/MainGame/EndMenu");
    }
}