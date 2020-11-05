using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class UpdateIntervalAStar : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        InvokeRepeating("UpdateGraph", 0.1f, 2f);
    }

    void UpdateGraph()
    {
        AstarPath.active.Scan();
    }
}
