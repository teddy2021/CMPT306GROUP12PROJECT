using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupOres : MonoBehaviour
{

    private GameObject player;                   // player
    private Inventory inventory;                // player inventory
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

	private void OnTriggerStay2D(Collider2D collision)
	{
		if(collision.tag == "Player")
		{
            player = collision.gameObject;
            inventory = player.GetComponent<Inventory>();
            if (this.tag == "Coal")
            {
                inventory.items[0].quantity += 1;
                Destroy(this.gameObject);
            }
            if (this.tag == "Copper")
            {
                inventory.items[1].quantity += 1;
                Destroy(this.gameObject);
            }
            if (this.tag == "Iron")
			{
                inventory.items[2].quantity += 1;
                Destroy(this.gameObject);
            }
            inventory.UpdateUI();
        }
	}
}
