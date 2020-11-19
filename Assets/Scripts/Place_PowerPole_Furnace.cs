using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Place_PowerPole_Furnace : MonoBehaviour
{
    new Camera camera;
    public GameObject powergrid;
    public GameObject furnace;
    public GridLayout grid;
    private string itemSelected;

    //for determining inventory
    public GameObject player;                   // player
    private Inventory inventory;                // player inventory

    public Texture2D defaultCursor;
    public Texture2D powerPoleCursor;
    public Texture2D furnaceCursor;
    private Texture2D currentCursor;
    public CursorMode cursorMode = CursorMode.Auto;
    public Vector2 hotSpot = Vector2.zero;

    // Start is called before the first frame update
    void Start()
    {
        currentCursor = powerPoleCursor;
        itemSelected = "PowerPoles";
        camera = Camera.main;
        inventory = player.GetComponent<Inventory>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!GameController.GameIsPaused)
        {
            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                itemSelected = "PowerPoles";
                currentCursor = powerPoleCursor;
            }
            if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                itemSelected = "furnaces";
                currentCursor = furnaceCursor;
            }

            if (Input.GetKey("space"))
            {
                Cursor.SetCursor(currentCursor, new Vector2(8, 8), cursorMode);
                if (Input.GetMouseButtonUp(1))
                {
                    switch (itemSelected)
                    {
                        case "PowerPoles":
                            if (inventory.items[3].quantity > 0)
                            {
                                Vector3 mousePos = new Vector3();
                                mousePos.x = camera.ScreenToWorldPoint(Input.mousePosition).x;
                                mousePos.y = camera.ScreenToWorldPoint(Input.mousePosition).y;
                                mousePos.z = 0;
                                Vector3 cellPostion = grid.LocalToCell(mousePos);
                                Instantiate(powergrid, grid.CellToLocalInterpolated(cellPostion), Quaternion.identity);
                                inventory.items[3].quantity -= 1;
                                inventory.UpdateUI();
                            }
                            break;

                        case "furnaces":
                            if (inventory.items[4].quantity > 0)
                            {
                                Vector3 mousePos = new Vector3();
                                mousePos.x = camera.ScreenToWorldPoint(Input.mousePosition).x;
                                mousePos.y = camera.ScreenToWorldPoint(Input.mousePosition).y;
                                mousePos.z = 0;
                                Vector3 cellPostion = grid.LocalToCell(mousePos);
                                Instantiate(furnace, grid.CellToLocalInterpolated(cellPostion), Quaternion.identity);
                                inventory.items[4].quantity -= 1;
                                inventory.UpdateUI();
                            }
                            break;
                    }
                }
            }
        }
        if (Input.GetKeyUp("space"))
        {
            Cursor.SetCursor(defaultCursor, Vector2.zero, cursorMode);
        }
        else
        {
            Cursor.SetCursor(defaultCursor, Vector2.zero, cursorMode);
        }
    }
}
