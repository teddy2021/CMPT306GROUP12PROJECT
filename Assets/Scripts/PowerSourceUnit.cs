using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

public class PowerSourceUnit : MonoBehaviour
{
    new Camera camera;
    private Animator animator;

    //for determining inventory
    private GameObject player;               // player
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
    private SpriteRenderer sprite;

    //safe zone components
    private Light2D safeZoneLight;
    private CircleCollider2D safeZoneCollider;
    public GameObject GloabalLight;

    public FurnaceBar fb;

    // Start is called before the first frame update
    void Start()
    {
        sprite = GetComponent<SpriteRenderer>();
        sprite.sortingOrder = -Mathf.RoundToInt(transform.position.y + 20);

        coalTimer = timer;
        camera = Camera.main;
        power = false;
        animator = GetComponent<Animator>();
        player = GameObject.FindWithTag("Player");
        inventory = player.GetComponent<Inventory>();

        //safe zone components
        safeZoneLight = GetComponentInChildren<Light2D>();
        safeZoneCollider = GetComponentInChildren<CircleCollider2D>();
        safeZoneLight.intensity = GloabalLight.GetComponent<Light2D>().intensity;
        safeZoneCollider.enabled = false;

    }

    // Update is called once per frame
    void Update()
    {
        fb.SetCurrentCoal(coal[0].quantity, coalTimer/timer);
        if (coal[0].quantity > 0)
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
        safeLight();
    }


    private void safeLight()
    {
        if (power)
        {
            safeZoneLight.intensity = 0.6f;
            safeZoneCollider.enabled = true;
        }
        else
        {
            safeZoneLight.intensity = GloabalLight.GetComponent<Light2D>().intensity;
            safeZoneCollider.enabled = false;
        }
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

	
	void OnMouseOver()
	{
		if (Input.GetKey(KeyCode.LeftControl) && !GameController.GameIsPaused)
		{
            Cursor.SetCursor(furnaceCursor, new Vector2(8, 8), cursorMode);
            if (Input.GetMouseButtonUp(0))
		    {
                if(inventory.items[0].quantity > 0)
			    {
                    coal[0].quantity += 1;
                    inventory.items[0].quantity -= 1;
                    inventory.UpdateUI();
			    }
            
            }
		}
		else
		{
            Cursor.SetCursor(defaultCursor, Vector2.zero, cursorMode);
        }
	}
	private void OnMouseExit()
	{
        Cursor.SetCursor(defaultCursor, Vector2.zero, cursorMode);
    }
}
