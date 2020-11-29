using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceFollow : MonoBehaviour
{

    private GameObject player;
    private float speed = 5F;
    private Rigidbody2D parent;

    // Start is called before the first frame update
    void Start()
    {
        parent = transform.parent.gameObject.GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //Object pickup
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            player = collision.gameObject;
            float step = (speed * Time.deltaTime) / (Vector3.Distance(transform.position, player.transform.position) * Vector3.Distance(transform.position, player.transform.position));
            parent.transform.position = Vector3.MoveTowards(transform.position, player.transform.position, step);
        }
    }
}
