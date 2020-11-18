using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerSourceUnit : MonoBehaviour
{
    new Camera camera;
    private Animator animator;

    //for determining inventory
    public GameObject player;               // player
    private Inventory inventory;            // player inventory
    public Item[] coal = new Item[1];       //coal stored in furnace

    public bool power = false;
    public float timer = 10;
    public float coalTimer;

    //cursor variables
    public Texture2D furnaceCursor;
    public Texture2D defaultCursor;
    public CursorMode cursorMode = CursorMode.Auto;
    public Vector2 hotSpot = Vector2.zero;

    // Start is called before the first frame update
    void Start()
    {
        coalTimer = timer;
        camera = Camera.main;
        power = false;
        animator = GetComponent<Animator>();
        inventory = player.GetComponent<Inventory>();

    }

    // Update is called once per frame
    void Update()
    {
        if(coal[0].quantity > 0)
		{
            if (coalTimer > 0)
            {
                coalTimer -= Time.deltaTime;
            }
            if (coalTimer <= 0)
            {
                coal[0].quantity -= 1;      //remove one coal
                coalTimer = timer;
            }
            power = true;
		}
		else
		{
            power = false;
		}
        furnaceAnimations();
    }

    private void furnaceAnimations()
	{
        if (power)
        {
            animator.SetBool("Power", true);
        }
        else
        {
            animator.SetBool("Power", false);
        }
    }

	private void OnMouseEnter()
	{
		Cursor.SetCursor(furnaceCursor, new Vector2(8, 8), cursorMode);
	}
	void OnMouseOver()
	{
        
		if (Input.GetMouseButtonUp(0))
		{
            if(inventory.items[0].quantity > 0)
			{
                coal[0].quantity += 1;
                inventory.items[0].quantity -= 1;
			}
            
        }
	}
	private void OnMouseExit()
	{
        Cursor.SetCursor(defaultCursor, Vector2.zero, cursorMode);
    }
}
