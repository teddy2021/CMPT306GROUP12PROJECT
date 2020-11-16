using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;

using UnityEngine;
using UnityEngine.Tilemaps;

public class DeleteTileOnClick : MonoBehaviour
{
    new Camera camera;

    public Tilemap tilemap;


	[Range(0.01f, 2.00f)]
	[SerializeField] private float DeleteTime;
    [SerializeField] private Texture2D[] cursor_circle;

    private bool destroying;
    private Vector3Int location;
    private Coroutine deletion;


    IEnumerator Delete(){
        Debug.Log("Deleting tile at " + location);
		for(int i = 0; i < 17; i += 1){
            Cursor.SetCursor(cursor_circle[i], new Vector2(0,0), CursorMode.Auto); // Animate cursor (has 7 frames)
			yield return new WaitForSeconds(DeleteTime/17.0f); // Pause for 1/17th of desletion time

		}
		tilemap.SetTile(location, null);
		tilemap.RefreshTile(location + new Vector3Int(-1, 0, 0));
		tilemap.RefreshTile(location + new Vector3Int(0, -1, 0));
		tilemap.RefreshTile(location + new Vector3Int(1, 0, 0));
		tilemap.RefreshTile(location + new Vector3Int(0, 1, 0));
        Cursor.SetCursor(cursor_circle[17], new Vector2(8,8), CursorMode.Auto);
        StopCoroutine(deletion);
    }

    // Start is called before the first frame update
    void Start()
    {
        destroying = false;
        Cursor.SetCursor(cursor_circle[17], new Vector2(8,8), CursorMode.Auto);
        camera = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetMouseButton(0) && !destroying){ // Begin destruction of a tile
            Vector3 world = camera.ScreenToWorldPoint(Input.mousePosition);
            location = tilemap.WorldToCell(world);
            if(tilemap.HasTile(location)){
                deletion = StartCoroutine(Delete());
                destroying = true;
            }
        }

        if(destroying && Vector3Int.Distance(tilemap.WorldToCell( // Destroying a tile, but dragged to new tile
            camera.ScreenToWorldPoint(
                Input.mousePosition)), location) > 0.75){
                    StopCoroutine(deletion);
                    location = tilemap.WorldToCell(camera.ScreenToWorldPoint(Input.mousePosition));
                    deletion = StartCoroutine(Delete());
                }

        if(Input.GetMouseButtonUp(0) && destroying){ // Cancel destruction of a tile
            StopCoroutine(deletion);
            Cursor.SetCursor(cursor_circle[17], new Vector2(8,8), CursorMode.Auto);
            destroying = false;
        }
    }

}