using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

public class PlayerHeadlampCollider : MonoBehaviour
{
    private HeadLamp headLampObject;
    private Light2D headLampLight;
    
    // Start is called before the first frame update
    void Start()
    {
        headLampObject = GetComponentInChildren<HeadLamp>();
        headLampLight = GetComponentInChildren<Light2D>();
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.tag == "SafeLight")
        {
            headLampObject.headLampIsOn = true;
            headLampObject.headLampTimer = headLampObject.timer;
            headLampLight.intensity = 1;
        }
    }
}
