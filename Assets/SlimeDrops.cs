using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlimeDrops : MonoBehaviour
{

    public GameObject[] drops;

    void SlimeDrop()
    {
        for (int i = 0; i < Random.Range(5, 10); i += 1)
        {
            Instantiate(drops[Random.Range(0, drops.Length)],gameObject.transform.position, Quaternion.identity);
        }

        // when the death anim completes, delete this object.
        Destroy(gameObject); // remove us

    }
}
