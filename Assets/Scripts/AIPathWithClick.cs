using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using Pathfinding;

public class AIPathWithClick : MonoBehaviour
{
    new Camera camera;
    public AIDestinationSetter aIDestinationSetter;
    GameObject goalObj;

    // Start is called before the first frame update
    void Start()
    {
        camera = Camera.main;

        goalObj = new GameObject();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButton(0))
        {
            Vector3 world = camera.ScreenToWorldPoint(Input.mousePosition);


            goalObj.transform.position = world;

            aIDestinationSetter.target = goalObj.transform;
        }
    }
}