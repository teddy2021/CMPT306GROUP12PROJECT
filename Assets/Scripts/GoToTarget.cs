using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoToTarget : MonoBehaviour
{
    public GameObject target;
    public bool freeze = false;

    private PathWalker2D pathWalker;
    private ScriptableMovement2D movement;

    // Start is called before the first frame update
    void Start()
    {
        pathWalker = GetComponent<PathWalker2D>();
        movement = GetComponent<ScriptableMovement2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!movement.hasGoal && !freeze)
        {
            pathWalker.GoToLocation(target.transform.position);
        }
    }
}
