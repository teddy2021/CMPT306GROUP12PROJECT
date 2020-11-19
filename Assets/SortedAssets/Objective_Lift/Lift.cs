using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Lift : MonoBehaviour
{
    public UnityEvent OnLiftCompleted;
    public UnityEvent OnLiftFailed;
    private PowerGrid powerGrid;

    private void Start()
    {
        powerGrid = GetComponent<PowerGrid>();
        if (OnLiftCompleted == null)
            OnLiftCompleted = new UnityEvent();

        if (OnLiftFailed == null)
            OnLiftFailed = new UnityEvent();

        this.HasAllKeys();
    }

    private void OnCollisionEnter2D(Collision2D col)
    {
        // check if it is player

        if (col.gameObject.tag == "Player")
        {
            // check all keys to see if they are grabbed
            bool hasAllKeys = HasAllKeys();
            

            // now check if they have the key or not
            if (hasAllKeys && powerGrid.power)
            {
                OnLiftCompleted.Invoke();
            }
            else
            {
                OnLiftFailed.Invoke();
            }
        }
    }

    private bool HasAllKeys()
    {
        int keysFound = 0;
        int allKeys = 0;
        foreach (var keyObj in GameObject.FindGameObjectsWithTag("Objective_Key"))
        {
            var keyComp = keyObj.GetComponent<Key>();

            if (keyComp.grabbed)
            {
                keysFound++;
            }
            allKeys++;
        }

        GameController.KeysCollected = keysFound;
        GameController.KeysCollected = allKeys;

        if (keysFound == allKeys)
        {
            return true;
        } else
        {
            return false;
        }
    }
}
