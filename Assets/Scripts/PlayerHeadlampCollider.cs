using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

public class PlayerHeadlampCollider : MonoBehaviour
{
    private HeadLamp headLampObject;
    private Light2D headLampLight;
    private PlayerDamage damage;
    
    // Start is called before the first frame update
    void Start()
    {
        headLampObject = GetComponentInChildren<HeadLamp>();
        headLampLight = GetComponentInChildren<Light2D>();
        damage = GetComponent<PlayerDamage>();
    }

    void Update()
    {
        if (!headLampObject.headLampIsOn && damage.health >0)
        {
            damage.tryDoDamage(25, new Vector2(0, 0));
        }

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
