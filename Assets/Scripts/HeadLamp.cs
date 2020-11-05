using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeadLamp : MonoBehaviour
{
    public float smooth = 5.0f;
    Quaternion target;
    public Transform dwarfPosition;

    // Update is called once per frame
    void Update()
    {
        
    }

    // Attach script to main camera

    void FixedUpdate()
    {
        float X = Input.GetAxisRaw("Horizontal");           // reads left and right inputs
        float Y = Input.GetAxisRaw("Vertical");             // reads up and down inputs

        //down
        if(X == 0 && Y == -1)
		{
            target = Quaternion.Euler(0, 0, 180);
            transform.position = dwarfPosition.position + new Vector3(0.03f, 0.4f, 0);
        }
        //down/right
        if (X == 1 && Y == -1)
        {
            target = Quaternion.Euler(0, 0, 225);
            transform.position = dwarfPosition.position + new Vector3(0.03f, 0.33f, 0);
        }
        //right
        if (X == 1 && Y == 0)
        {
            target = Quaternion.Euler(0, 0, 270);
            transform.position = dwarfPosition.position + new Vector3(0.3f, 0.3f, 0);

        }
        //up/right
        if (X == 1 && Y == 1)
        {
            target = Quaternion.Euler(0, 0, 315);
            transform.position = dwarfPosition.position + new Vector3(0.2f, 0.4f, 0);

        }
        //up
        if (X == 0 && Y == 1)
        {
            target = Quaternion.Euler(0, 0, 0);
            transform.position = dwarfPosition.position + new Vector3(-0.08f, 0.45f, 0);
        }
        //up/left
        if (X == -1 && Y == 1)
        {
            target = Quaternion.Euler(0, 0, 45);
            transform.position = dwarfPosition.position + new Vector3(-0.2f, 0.4f, 0);
        }
        //left
        if (X == -1 && Y == 0)
        {
            target = Quaternion.Euler(0, 0, 90);
            transform.position = dwarfPosition.position + new Vector3(-0.3f, 0.3f, 0);
        }
        //down/left
        if (X == -1 && Y == -1)
        {
            target = Quaternion.Euler(0, 0, 135);
            transform.position = dwarfPosition.position + new Vector3(-0.03f, 0.33f, 0);
        }
        transform.rotation = Quaternion.Slerp(transform.rotation, target, Time.deltaTime * smooth);
    }
}
