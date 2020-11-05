using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class DeleteTileOnClick : MonoBehaviour
{
    new Camera camera;

    public Tilemap tilemap;

    // Start is called before the first frame update
    void Start()
    {
        camera = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButton(0))
        {
            Vector3 world = camera.ScreenToWorldPoint(Input.mousePosition);


            tilemap.SetTile(tilemap.WorldToCell(world), null);
        }
    }
}