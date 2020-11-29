using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

public class HeadLamp : MonoBehaviour
{
    public float smooth = 5.0f;
    Quaternion target;
    public Transform dwarfPosition;
    public Light2D headLamp;


    public float timer = 20;
    public float headLampTimer;
    public bool headLampIsOn = false;

    public LampBar lampBar;


	private void Start()
	{
        headLampTimer = timer;
        headLampIsOn = true;
        lampBar.SetMaxHealth((int)headLampTimer);
	}

	// Update is called once per frame
	void Update()
    {
		if (headLampIsOn)
		{
            if(headLampTimer > 0)
			{
                lampBar.SetHealth((int)(headLampTimer+0.5));
                headLampTimer -= Time.deltaTime;
                headLamp.intensity -= Mathf.Lerp(0.0f,1.0f,Time.deltaTime/timer);
                
			}
			else
			{
                lampBar.SetHealth((int)headLampTimer);
                headLampTimer = 0;
                headLampIsOn = false;
                headLamp.intensity = 0;
                
            }
		}
    }


    private void OnTriggerStay2D(Collider2D collision)
    {

    }
    // My cool comment
    // Attach script to main camera

    void FixedUpdate()
    {
        calculatePosition();
    }




    public void calculatePosition()
	{
        float X = Input.GetAxisRaw("Horizontal");           // reads left and right inputs
        float Y = Input.GetAxisRaw("Vertical");             // reads up and down inputs

        //down
        if (X == 0 && Y == -1)
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

    public void IncreaseTimer(float amount = 0)
    {
        headLampTimer += amount;
        GameController.StartGame("next floor", GameController.FloorLevel++);
    }
}
