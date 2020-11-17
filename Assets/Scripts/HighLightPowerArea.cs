using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HighLightPowerArea : MonoBehaviour
{

    private Color color = Color.yellow;
    private Vector3 TopLeft;
    private Vector3 TopRight;
    private Vector3 BottomLeft;
    private Vector3 BottomRight;
    private BoxCollider2D area;
    private Vector3 center;
    private Vector3 size;
    private LineRenderer lineArea;

    // Start is called before the first frame update
    void Start()
    {

        if (tag == "SafeLight")
		{
            area = GetComponentInParent<BoxCollider2D>();
		}
		else
		{
            area = GetComponent<BoxCollider2D>();
        }

        lineArea = this.gameObject.AddComponent<LineRenderer>();
        lineArea.material = Resources.Load<Material>("Materials/PowerArea");
        //lineArea.startColor = color;
        //lineArea.endColor = color;
        lineArea.startWidth = 0.05f;
        lineArea.endWidth = 0.05f;
        lineArea.positionCount = 5;
    }

    // Update is called once per frame
    void Update()
    {
        CalcPositions();
        DrawArea();
    }

	void CalcPositions()
	{
        center = area.bounds.center;
        size = area.bounds.extents;
        TopLeft = new Vector3(center.x - size.x, center.y + size.y, center.z);
        TopRight = new Vector3(center.x + size.x, center.y + size.y, center.z);
        BottomLeft = new Vector3(center.x - size.x, center.y - size.y, center.z);
        BottomRight = new Vector3(center.x + size.x, center.y - size.y, center.z);
    }

    void DrawArea()
	{
        if (Input.GetKey("space"))
		{
            lineArea.positionCount = 5;
            lineArea.SetPosition(0, TopLeft);
            lineArea.SetPosition(1, TopRight);
            lineArea.SetPosition(2, BottomRight);
            lineArea.SetPosition(3, BottomLeft);
            TopLeft = new Vector3(center.x - size.x, center.y + size.y + 0.025f, center.z); // to adjust for small error on final line position
            lineArea.SetPosition(4, TopLeft);
        }
		else
		{
            lineArea.positionCount = 0;
        }
	}


}
