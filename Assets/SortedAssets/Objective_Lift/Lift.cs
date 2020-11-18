using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Lift : MonoBehaviour
{
    public UnityEvent OnLiftCompleted;
    public UnityEvent OnLiftFailed;
    private bool power;

    private void Start()
    {
        power = GetComponent<PowerGrid>().power;
        if (OnLiftCompleted == null)
            OnLiftCompleted = new UnityEvent();

        if (OnLiftFailed == null)
            OnLiftFailed = new UnityEvent();
    }

    private void OnCollisionEnter2D(Collision2D col)
    {
        // check if it is player

        if (col.gameObject.tag == "Player")
        {
            // check all keys to see if they are grabbed
            bool hasAllKeys = true;
            foreach (var keyObj in GameObject.FindGameObjectsWithTag("Objective_Key"))
            {
                var keyComp = keyObj.GetComponent<Key>();

                if (!keyComp.grabbed)
                    hasAllKeys = false;
            }

            // now check if they have the key or not
            if (hasAllKeys && power)
            {
                OnLiftCompleted.Invoke();
            }
            else
            {
                OnLiftFailed.Invoke();
            }
        }
    }
}
