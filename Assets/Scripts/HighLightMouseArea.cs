using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HighLightMouseArea : MonoBehaviour
{
    new Camera camera;
    private BoxCollider2D area;
    private LineRenderer lineArea;
    private Color color = Color.yellow;

    private Vector3 TopLeft;
    private Vector3 TopRight;
    private Vector3 BottomLeft;
    private Vector3 BottomRight;

    private Vector3 center;
    private Vector3 size;

    public GridLayout grid;

    // Start is called before the first frame update
    void Start()
    {
        camera = Camera.main;
        area = GetComponent<BoxCollider2D>();
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
        Vector3 mousePos = new Vector3();
        mousePos.x = camera.ScreenToWorldPoint(Input.mousePosition).x;
        mousePos.y = camera.ScreenToWorldPoint(Input.mousePosition).y;
        mousePos.z = 0;

        Vector3 cellPostion = grid.LocalToCell(mousePos);
        mousePos = grid.CellToLocalInterpolated(cellPostion);

        center = area.bounds.center;
        size = area.bounds.extents;
        TopLeft = new Vector3(mousePos.x + (center.x - size.x), mousePos.y + (center.y + size.y), mousePos.z);
        TopRight = new Vector3(mousePos.x + (center.x + size.x), mousePos.y + (center.y + size.y), mousePos.z);
        BottomLeft = new Vector3(mousePos.x + (center.x - size.x), mousePos.y + (center.y - size.y), mousePos.z);
        BottomRight = new Vector3(mousePos.x + (center.x + size.x), mousePos.y + (center.y - size.y), mousePos.z);

        if (Input.GetKey("space"))
        {
            lineArea.positionCount = 5;
            lineArea.SetPosition(0, TopLeft);
            lineArea.SetPosition(1, TopRight);
            lineArea.SetPosition(2, BottomRight);
            lineArea.SetPosition(3, BottomLeft);
            TopLeft = new Vector3(mousePos.x + (center.x - size.x), mousePos.y + (center.y + size.y) + 0.025f, mousePos.z);// to adjust for small error on final line position
            lineArea.SetPosition(4, TopLeft);
        }
        else
        {
            lineArea.positionCount = 0;
        }
    }
}
