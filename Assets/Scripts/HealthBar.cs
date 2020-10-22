using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{

    public Slider slider;
    public Text text;

    public GameObject segmentLine_prefab;
    private List<GameObject> segmentLines;
    public GameObject grid_layout;

    public void SetHealth(int health)
    {
        slider.value = health;
        int num_segments = segmentLines.Count;

        int segment_diff = Math.Max((health / 10 ), 0) - num_segments;

        if (segment_diff > 0)
        {
            for (int i = 0; i < segment_diff; i++)
            {
                GameObject new_segment = Instantiate(segmentLine_prefab);
                new_segment.transform.parent = grid_layout.transform;
                segmentLines.Add(new_segment);
            }
        }
        else if (segment_diff < 0)
        {
            for (int i = 0; i < Mathf.Abs(segment_diff); i++)
            {
                GameObject to_remove = segmentLines[segmentLines.Count - 1];
                segmentLines.Remove(to_remove);
                Destroy(to_remove);
            }
        }
    }

    public void SetLevel(int level)
    {
        grid_layout.GetComponent<GridLayoutGroup>().padding.left = (250 / (level)) - 2;

        Vector2 newspace = new Vector2((250f / (level)) - 2f, 0);
        grid_layout.GetComponent<GridLayoutGroup>().spacing = newspace;


        slider.maxValue = level * 10;
        slider.value = level * 10;
        text.text = level.ToString();
        segmentLines = new List<GameObject>();

        for (int i = 0; i < level - 1; i++)
        {
            GameObject new_segment = Instantiate(segmentLine_prefab);
            new_segment.transform.SetParent(grid_layout.transform);
            segmentLines.Add(new_segment);
        }

        
    }
}
