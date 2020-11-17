using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Key : MonoBehaviour
{
    void OnGrabAnimComplete()
    {
        gameObject.GetComponent<SpriteRenderer>().enabled = false; // hide the key after the anim is done
        transform.GetChild(0).GetComponent<Animator>().Play("Light_Fade"); // fade the light
    }

    public bool grabbed = false;

    // frame when an entity collides with us
    void OnTriggerEnter2D(Collider2D col)
    {
        var anim = gameObject.GetComponent<Animator>();

        // check if it is the player
        if (col.gameObject.tag == "Player" && !grabbed /*anim.GetCurrentAnimatorStateInfo(0).IsName("Key_Idle")*/)
        {
            grabbed = true;

            // play the grab animation
            anim.Play("Key_Grab");
        }
    }
}
