using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBarSlime : MonoBehaviour
{

    public Slider slider;
    public Text text;
    public Image fill;

    private int maxhealth;

    public void SetHealth(int health)
    {
        text.text = health.ToString() + "/" + maxhealth.ToString();
        slider.value = health;

        float healthRatio = (float)health / maxhealth;

        fill.color = new Color(1 - healthRatio, healthRatio, 0.1f, 1);
    }

    public void SetMaxHealth(int health)
    {
        this.maxhealth = health;

        slider.maxValue = maxhealth;

        SetHealth(health);
    }

}
