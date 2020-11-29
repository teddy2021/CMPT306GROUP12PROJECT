using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupOres : MonoBehaviour
{

    private GameObject player;        
    private Inventory inventory;                // player inventory

    private AudioSource pickupSound;

    public AudioClip[] pickupSounds;

    // Start is called before the first frame update
    void Start()
    {
        pickupSound = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {

    }

   


    //Object pickup
    private void OnTriggerStay2D(Collider2D collision)
	{
		if(collision.tag == "Player")
		{
            player = collision.gameObject;
            inventory = player.GetComponent<Inventory>();
            if (this.tag == "Coal")
            {
                pickupSound.clip = pickupSounds[Random.Range(0, pickupSounds.Length)];
                pickupSound.Play(0);

                inventory.items[0].quantity += 1;
                Destroy(this.gameObject);
            }
            if (this.tag == "Copper")
            {
                pickupSound.clip = pickupSounds[Random.Range(0, pickupSounds.Length)];
                pickupSound.Play(0);

                inventory.items[1].quantity += 1;
                Destroy(this.gameObject);
            }
            if (this.tag == "Iron")
			{
                pickupSound.clip = pickupSounds[Random.Range(0, pickupSounds.Length)];
                pickupSound.Play(0);

                inventory.items[2].quantity += 1;
                Destroy(this.gameObject);
            }
            inventory.UpdateUI();
        }
	}
}
