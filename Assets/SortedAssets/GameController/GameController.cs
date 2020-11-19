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
        //GameObject.Find("Key_Panel").SetActive(false);
        SceneManager.LoadScene("Scenes/MainGame/Game");
    }

    public static void EndGame(string reason)
    {
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
}