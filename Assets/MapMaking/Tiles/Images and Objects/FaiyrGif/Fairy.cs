using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fairy : MonoBehaviour
{

    private GameObject player;
    private AudioSource pickupSound;
    public AudioClip[] pickupSounds;
    public GameObject parent;
    // Start is called before the first frame update
    void Start()
    {
        pickupSound = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            player = collision.gameObject;

            player.GetComponent<PlayerDamage>().doHeal(35);
            player.GetComponentInChildren<HeadLamp>().doPower(20f);

            pickupSound.clip = pickupSounds[Random.Range(0, pickupSounds.Length)];
            pickupSound.Play(0);

            Destroy(parent);

        }
    }
}
