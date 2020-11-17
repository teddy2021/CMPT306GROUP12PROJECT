using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

public class PowerGrid : MonoBehaviour
{

	
	public GameObject GloabalLight;

	private GameObject source = null;
	private Light2D safeZoneLight;
	private CircleCollider2D safeZoneCollider;
	private bool power = false;
	private LineRenderer powerLine;
	private SpriteRenderer sprite;

    void Start()
    {
		sprite = GetComponent<SpriteRenderer>();
		sprite.sortingOrder = -Mathf.RoundToInt(transform.position.y - 1);      //for drawing the powerline in the correct sort order (so player appears behind it)

		power = false;
		safeZoneLight = GetComponentInChildren<Light2D>();
		safeZoneCollider = GetComponentInChildren<CircleCollider2D>();
		safeZoneLight.intensity = GloabalLight.GetComponent<Light2D>().intensity;
		safeZoneCollider.enabled = false;

		//Set values for powerLine 
		powerLine = this.gameObject.AddComponent<LineRenderer>();
		powerLine.material = Resources.Load<Material>("Materials/Wire");
		powerLine.startWidth = 0.05f;
		powerLine.endWidth = 0.05f;
		powerLine.positionCount = 0;
		
	}

    // Update is called once per frame
    void Update()
    {
		safeLight();
		sourcePower();
		powerLineDraw();
    }

	private void powerLineDraw()
	{
		if (source != null)
		{
			if (source.tag == "PowerGrid")
			{
				Vector3 offset = new Vector3(-0.1f, 0.9084f, 0);			//offset is used to position starts and ends of lines properly on powerGrid sprite
				powerLine.positionCount = 2;
				powerLine.SetPosition(0, this.transform.position + offset);
				powerLine.SetPosition(1, source.transform.position + offset);
			}
			//for cases where powerPole is conencted to furnace
			if(source.tag == "PowerGenerator")
			{
				Vector3 offset = new Vector3(-0.1f, 0.9084f, 0);
				Vector3 offset2 = new Vector3(0.05f, 0.5f, 0);
				powerLine.positionCount = 2;
				powerLine.SetPosition(0, this.transform.position + offset);
				powerLine.SetPosition(1, source.transform.position + offset2);
			}
			if(powerLine.GetPosition(0).y < powerLine.GetPosition(1).y)
			{
				powerLine.sortingOrder = -Mathf.RoundToInt(transform.position.y);	//for drawing the powerline in the correct sort order (so player appears behind it)
			}

		}
		else
		{
			powerLine.positionCount = 0;
		}
	}

	private void sourcePower()
	{
		if (source != null)
		{
			//GameObject o = sourceList[i];

			if (source.tag == "PowerGenerator")
			{
				if (source.GetComponent<PowerSourceUnit>().power)
				{
					power = true;
				}
				else
				{
					power = false;
				}
			}
			if (source.tag == "PowerGrid")
			{
				if (source.GetComponent<PowerGrid>().power)
				{
					power = true;
				}
				else
				{
					power = false;
				}
			}
		}
		else
		{
			power = false;
		}
	}

	private void safeLight()
	{
		if (power)
		{
			safeZoneLight.intensity = 0.8f;
			safeZoneCollider.enabled = true;
		}
		else
		{
			safeZoneLight.intensity = GloabalLight.GetComponent<Light2D>().intensity;
			safeZoneCollider.enabled = false;
		}
	}

	private void OnTriggerStay2D(Collider2D collision)
	{
		if (collision.tag == "PowerGenerator")
		{
			source = collision.gameObject;
		}
		if (collision.tag == "PowerGrid")
		{
			if (collision.gameObject.GetComponent<PowerGrid>().power && !power)
			{
				source = collision.gameObject;
			}
			
		}
	}


}
