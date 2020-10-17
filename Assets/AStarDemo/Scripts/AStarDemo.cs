using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class AStarDemo : MonoBehaviour
{
    private Tilemap walkMap;
    new Camera camera;
    private AStar astar;

    private Vector3Int goal;

    public GameObject obj;
    private ScriptableMovement2D moveObj;

    // Start is called before the first frame update
    void Start()
    {
        walkMap = gameObject.GetComponent<Tilemap>();
        astar = gameObject.GetComponent<AStar>();

        camera = Camera.main;

        moveObj = obj.GetComponent<ScriptableMovement2D>();

        moveObj.hasGoalChanged.AddListener(() =>
        {
            Debug.Log("done");
        });
    }

    public Vector3 CellToWorld(Vector3Int where)
    {
        var answer = walkMap.CellToWorld(where);

        answer += walkMap.tileAnchor;

        return answer;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButton(0))
        {
            Vector3 world = camera.ScreenToWorldPoint(Input.mousePosition);
            Vector3Int gridPos = walkMap.WorldToCell(world);

            Debug.Log("start:" + gridPos);

            goal = gridPos;
        }

        obj.GetComponent<ScriptableMovement2D>().SetGoal(new Vector2(goal.x, goal.y));

        /*if (Input.GetMouseButton(1))
        {
            Vector3 world = camera.ScreenToWorldPoint(Input.mousePosition);
            Vector3Int gridPos = walkMap.WorldToCell(world);

            Debug.Log("goal:" + gridPos);
            goal = gridPos;
        }

        if (Input.GetMouseButton(2))
        {
            path = astar.FindPath(new Vector2Int(start.x, start.y), new Vector2Int(goal.x, goal.y));

            Debug.Log(path.Count);
        }

        if (path != null)
        {
            var tempVec = CellToWorld(start);

            foreach (Vector2Int node in path)
            {
                var node3 = new Vector3Int(node.x, node.y, 0);
                var place = CellToWorld(node3);

                Debug.DrawLine(tempVec, place);

                tempVec = place;
            }
        }*/
    }
}
