using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class GameController : MonoBehaviour
{
    public static int FloorLevel = 0;
    public static bool GameIsPaused = false;
    public static int TotalKeys;
    public static int KeysCollected;

    public static void StartGame(string reason)
    {
        FloorLevel++;
        KeysCollected = 0;
        SceneManager.LoadScene("Scenes/MainGame/Game");
    }

    public static void EndGame(string reason)
    {
        SceneManager.LoadScene("Scenes/MainGame/EndMenu");
    }
}