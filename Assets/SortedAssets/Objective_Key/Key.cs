using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Key : MonoBehaviour
{
    void Start ()
    {
        //GameObject.FindGameObjectsWithTag("Key_Panel")[0].SetActive(false);
    }
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
            GameController.SetKeyText();
            // play the grab animation
            anim.Play("Key_Grab");
        }
    }
}
