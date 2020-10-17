using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;

public class AStar : MonoBehaviour
{
    private AStarNode[,] nodeMap;

    private Tilemap walkMap;

    [Tooltip("Is the Tilemap static?")]
    public bool isStatic = true;

    [Tooltip("Should we allow diagonal paths?")]
    public bool allowDiags = true;

    public enum DistMode
    {
        Manhattan = 0,
        Distance = 1,
        DistanceSq = 2
    };

    [Tooltip("What type of Distance Metric should we use?")]
    public DistMode CurrentDistMode = DistMode.DistanceSq;

    private void Start()
    {
        walkMap = gameObject.GetComponent<Tilemap>();

        CreateNodeMap();

        var columns = nodeMap.GetUpperBound(0) + 1;
        var rows = nodeMap.GetUpperBound(1) + 1;

        for (int i = 0; i < columns; i++)
        {
            for (int h = 0; h < rows; h++)
            {
                AStarNode node = nodeMap[i, h];

                node.UpdateNeighbors(nodeMap, new Vector2Int(i, h), isStatic, allowDiags);
            }
        }
    }

    private void CreateNodeMap()
    {
        if (nodeMap != null && isStatic)
            return;

        if (walkMap == null)
        {
            Debug.LogError("WalkMap was null on AStar Object");
            return;
        }

        walkMap.CompressBounds();

        var bounds = walkMap.cellBounds;

        nodeMap = new AStarNode[bounds.size.x, bounds.size.y];

        // thank you https://bitbucket.org/Sniffle6/tilemaps-with-astar/src/master/Assets/GridManager.cs
        for (int x = bounds.xMin, i = 0; i < (bounds.size.x); x++, i++)
        {
            for (int y = bounds.yMin, h = 0; h < (bounds.size.y); y++, h++)
            {
                if (walkMap.HasTile(new Vector3Int(x, y, 0)))
                {
                    // we defined the z part of the node to be '0' when the tile is walkable
                    nodeMap[i, h] = new AStarNode(x, y, 0);
                }
                else
                {
                    // else it will be '1' when it is not walkable
                    nodeMap[i, h] = new AStarNode(x, y, 1);
                }
            }
        }
    }

    private double GetDistance(Vector2Int a, Vector2Int b)
    {
        switch (CurrentDistMode)
        {
            case DistMode.Manhattan:
                var dx = Math.Abs(a.x - b.x);
                var dy = Math.Abs(a.y - b.y);
                return 1 * (dx + dy);

            case DistMode.Distance:
                return Vector2Int.Distance(a, b);

            case DistMode.DistanceSq:
                return (a - b).sqrMagnitude;

            default:
                return 1;
        }
    }

    public List<Vector2Int> FindPath(Vector2Int start, Vector2Int goal)
    {
        List<Vector2Int> answerPath = new List<Vector2Int>();
        CreateNodeMap();

        // make sure we have a nodemap
        if (nodeMap == null)
        {
            Debug.LogError("NodeMap was null for AStar.");
            return answerPath;
        }

        var columns = nodeMap.GetUpperBound(0) + 1;
        var rows = nodeMap.GetUpperBound(1) + 1;
        AStarNode snode = null;
        AStarNode gnode = null;

        // init all nodes
        for (int i = 0; i < columns; i++)
        {
            for (int h = 0; h < rows; h++)
            {
                AStarNode node = nodeMap[i, h];

                node.UpdateNeighbors(nodeMap, new Vector2Int(i, h), isStatic, allowDiags);
                node.H = 0;
                node.G = 0;
                node.Parent = null;

                var nodeV2D = node.ToVector2Int();

                if (start == nodeV2D)
                    snode = node;

                if (goal == nodeV2D)
                    gnode = node;
            }
        }

        // we didn't find a start or goal node.
        if (snode == null || gnode == null)
        {
            Debug.LogError("Starting or ending node was not found for AStar.");
            return answerPath;
        }

        var OpenSet = new List<AStarNode>();
        var ClosedSet = new HashSet<AStarNode>();

        OpenSet.Add(snode);

        while (OpenSet.Count > 0)
        {
            // pop lowest f score node
            OpenSet.Sort();
            AStarNode current = OpenSet.First();
            OpenSet.Remove(current);

            var currentV2D = current.ToVector2Int();
            var goalV2D = gnode.ToVector2Int();

            // if this is the goal node.
            if (current == gnode)
            {
                // build the path
                AStarNode tempNode = current;

                answerPath.Add(tempNode.ToVector2Int());

                while (tempNode.Parent != null)
                {
                    tempNode = tempNode.Parent;

                    answerPath.Add(tempNode.ToVector2Int());
                }

                break; // break out of loop
            }

            // now expand on the node's neighbors
            foreach (AStarNode neighbor in current.Neighbors)
            {
                var neighborV2D = neighbor.ToVector2Int();

                // check to see if neighbor is in the closed set
                if (ClosedSet.Contains(neighbor))
                    continue;

                //gets a temp comparison integer for seeing if a route is shorter than our current path
                var tempG = current.G + GetDistance(currentV2D, neighborV2D);

                bool newPath = false;

                //Checks if the neighboor we are checking is within the openset
                if (OpenSet.Contains(neighbor))
                {
                    if (tempG < neighbor.G) //The distance to the end goal from this neighboor is shorter so we need a new path
                    {
                        neighbor.G = tempG;
                        newPath = true;
                    }
                }
                else//if its not in openSet or closed set, then it IS a new path and we should add it too openset
                {
                    neighbor.G = tempG;
                    newPath = true;
                    OpenSet.Add(neighbor);
                }

                if (newPath)//if it is a newPath caclulate the H and F and set current to the neighboors previous
                {
                    neighbor.H = GetDistance(neighborV2D, goalV2D);
                    neighbor.Parent = current;
                }
            }

            // add node to closed set
            ClosedSet.Add(current);
        }

        return answerPath;
    }
}
