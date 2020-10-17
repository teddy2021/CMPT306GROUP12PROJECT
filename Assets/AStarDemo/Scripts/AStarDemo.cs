using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class AStarDemo : MonoBehaviour
{
    private Tilemap walkMap;
    new Camera camera;
    private AStar astar;

    private Vector3Int start;
    private Vector3Int goal;

    // Start is called before the first frame update
    void Start()
    {
        walkMap = gameObject.GetComponent<Tilemap>();
        astar = gameObject.GetComponent<AStar>();

        camera = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButton(0))
        {
            Vector3 world = camera.ScreenToWorldPoint(Input.mousePosition);
            Vector3Int gridPos = walkMap.WorldToCell(world);

            Debug.Log("start:" + gridPos);
            start = gridPos;
        }

        if (Input.GetMouseButton(1))
        {
            Vector3 world = camera.ScreenToWorldPoint(Input.mousePosition);
            Vector3Int gridPos = walkMap.WorldToCell(world);

            Debug.Log("goal:" + gridPos);
            goal = gridPos;
        }

        if (Input.GetMouseButton(2))
        {
            List<Vector2Int> path = astar.FindPath(new Vector2Int(start.x, start.y), new Vector2Int(goal.x, goal.y));

            Debug.Log(path.Count);
        }
    }
}
