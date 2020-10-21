using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class mouseZoom : MonoBehaviour
{

    // attach script to main camera

    public float zoomSpeed = 1;
    private float targetOrtho;
    public float smoothSpeed = 2.0f;
    public float minZoom = 1.0f;
    public float maxZoom = 20.0f;

    // grab the orthographic size of the camera as it is now
    void Start()
    {
        targetOrtho = Camera.main.orthographicSize;
    }

    // Recieve scroll wheel input and adjust the camera's orthographicSize according to the direction of the scroll
    void Update()
    {
        float scroll = Input.GetAxis("Mouse ScrollWheel");

        if(scroll != 0.0f)
		{
            targetOrtho -= scroll * zoomSpeed;
            targetOrtho = Mathf.Clamp(targetOrtho, minZoom, maxZoom);
		}

        Camera.main.orthographicSize = Mathf.MoveTowards(Camera.main.orthographicSize, targetOrtho, smoothSpeed * Time.deltaTime);
    }
}
