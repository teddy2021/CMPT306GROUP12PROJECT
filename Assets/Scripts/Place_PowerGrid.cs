using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Place_PowerGrid : MonoBehaviour
{
    new Camera camera;
    public GameObject powergrid;
    public GridLayout grid;

    //for determining inventory
    public GameObject player;                   // player
    private Inventory inventory;                // player inventory

    // Start is called before the first frame update
    void Start()
    {
        camera = Camera.main;
        inventory = player.GetComponent<Inventory>();
    }

    // Update is called once per frame
    void Update()
    {
        // if player has poles in inventory
        if (inventory.items[3].quantity > 0)
		{
            if (Input.GetMouseButtonUp(1) && Input.GetKey("space"))
            {
                Vector3 mousePos = new Vector3();
                mousePos.x = camera.ScreenToWorldPoint(Input.mousePosition).x;
                mousePos.y = camera.ScreenToWorldPoint(Input.mousePosition).y;
                mousePos.z = 0;
                Vector3 cellPostion = grid.LocalToCell(mousePos);
                Instantiate(powergrid, grid.CellToLocalInterpolated(cellPostion), Quaternion.identity);
                inventory.items[3].quantity -= 1;
            }
        }
    }
}
