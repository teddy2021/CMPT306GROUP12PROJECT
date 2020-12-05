using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FurnaceBar : MonoBehaviour
{

    public Slider slider;
    public Text text;
    public Image fill;

    private int currentCoal;

    public void SetCoal(float coalTimer)
    {
        text.text = "coal: "+currentCoal.ToString();
        slider.value = coalTimer;

        
        if(currentCoal > 0)
		{
            fill.color = new Color(0.854f, 0.325f, 0.008f, 0.5f);
        }
		else
		{
            fill.color = new Color(0, 0, 0, 0.5f);
		}
        
    }

    public void SetCurrentCoal(int curCoal, float coalTimer)
    {
        this.currentCoal = curCoal;


        SetCoal(coalTimer);
    }
}
