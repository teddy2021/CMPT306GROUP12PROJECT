using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{

    public Slider slider;
    public Text text;

    private int maxhealth;

    public void SetHealth(int health)
    {
        text.text = health.ToString() + "/" + maxhealth.ToString();
        slider.value = health;
    }

    public void SetMaxHealth(int health)
    {
        this.maxhealth = health;

        slider.maxValue = maxhealth;
        slider.value = maxhealth;
        text.text = maxhealth.ToString() + "/" + maxhealth.ToString();
        
    }
}
