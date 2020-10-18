using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Tilemaps;

public class PathWalker2D : MonoBehaviour
{
    [Tooltip("The tilemap with the AStar component.")]
    public Tilemap walkMap;

    [Tooltip("How close to each path node will be acceptable before moving to the next one?")]
    public double closeToNode = 1.0;

    private AStar2D astar;
    private ScriptableMovement2D mover;

    // Start is called before the first frame update
    void Start()
    {
        mover = gameObject.GetComponent<ScriptableMovement2D>();
        astar = walkMap.GetComponent<AStar2D>();
    }

    private double DistanceSquared(Vector3 a, Vector3 b)
    {
        return (a - b).sqrMagnitude;
    }

    public Vector3Int GetNearestValidTile(Vector3 world)
    {
        var ourTile = walkMap.WorldToCell(world);

        var closestTile = ourTile;
        var closestDist = 99999999999.9;

        foreach (var pos in walkMap.cellBounds.allPositionsWithin)
        {
            if (!walkMap.HasTile(pos))
                continue;

            if (pos.z != 0) // filter out bad tiles.
                continue;

            var tempDist = DistanceSquared(pos, ourTile);

            if (tempDist > closestDist)
                continue;

            closestDist = tempDist;
            closestTile = pos;
        }
        
        return closestTile;
    }

    public void GoToLocation(Vector3 goal)
    {
        StopAllCoroutines();
        StartCoroutine(GoToWorldLocation(goal));
    }

    public IEnumerator GoToWorldLocation(Vector3 goal)
    {
        // get the tiles we need.
        var goalTile = GetNearestValidTile(goal);
        var startTile = GetNearestValidTile(transform.position);

        // get the path
        var pathTiles = astar.FindPath(new Vector2Int(startTile.x, startTile.y), new Vector2Int(goalTile.x, goalTile.y));

        // convert the path to world positions
        var path = new List<Vector3>();
        foreach (var pathTile in pathTiles)
        {
            path.Add(walkMap.GetCellCenterWorld(new Vector3Int(pathTile.x, pathTile.y, 0)));
        }

        while (path.Count > 0)
        {
            var pathLoc = path.First();
            path.Remove(pathLoc);

            // tell the unit to move to the path node
            mover.SetGoal(pathLoc, closeToNode);

            // wait for the movement to complete...
            yield return new WaitUntil(() => mover.hasGoal == false);
        }


        // now just directly to the goal
        mover.SetGoal(goal, closeToNode);
        yield return new WaitUntil(() => mover.hasGoal == false);
    }
}
