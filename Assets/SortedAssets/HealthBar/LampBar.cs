using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LampBar : MonoBehaviour
{

    public Slider slider;
    public Image fill;

    private int maxhealth;

    public void SetHealth(int health)
    {
        slider.value = health;
    }

    public void SetMaxHealth(int health)
    {
        this.maxhealth = health;

        slider.maxValue = maxhealth;

        SetHealth(health);
    }

}
