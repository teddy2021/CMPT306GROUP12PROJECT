using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class SkillPoints : MonoBehaviour
{
    public GameObject skillPointsUI;

    public void LoadMenu()
    {
        skillPointsUI.SetActive(true);
    }

}
