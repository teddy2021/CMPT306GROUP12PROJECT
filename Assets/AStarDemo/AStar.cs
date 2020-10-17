using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class AStar : MonoBehaviour
{
    private AStarNode[,] nodeMap;

    public Tilemap walkMap;

    public bool isStatic = true;

    public bool allowDiags = true;

    private void CreateNodeMap()
    {
        if (nodeMap != null && isStatic)
            return;

        walkMap.CompressBounds();

        var bounds = walkMap.cellBounds;

        nodeMap = new AStarNode[bounds.size.x, bounds.size.y];

        // thank you https://bitbucket.org/Sniffle6/tilemaps-with-astar/src/master/Assets/GridManager.cs
        for (int x = bounds.xMin, i = 0; i < (bounds.size.x); x++, i++)
        {
            for (int y = bounds.yMin, j = 0; j < (bounds.size.y); y++, j++)
            {
                if (walkMap.HasTile(new Vector3Int(x, y, 0)))
                {
                    // we defined the z part of the node to be '0' when the tile is walkable
                    nodeMap[i, j] = new AStarNode(x, y, 0);
                }
                else
                {
                    // else it will be '1' when it is not walkable
                    nodeMap[i, j] = new AStarNode(x, y, 1);
                }
            }
        }
    }

    public List<Vector2Int> FindPath (Vector2Int start, Vector2Int goal)
    {
        CreateNodeMap();

        var columns = nodeMap.GetUpperBound(0) + 1;
        var rows = nodeMap.GetUpperBound(1) + 1;
        AStarNode snode = null;
        AStarNode gnode = null;

        // init all nodes
        for (int x = 0; x < columns; x++)
        {
            for (int y = 0; y < rows; y++)
            {
                AStarNode node = nodeMap[x, y];

                node.UpdateNeighbors(nodeMap, isStatic, allowDiags);
                node.H = 0;
                node.G = 0;
                node.Parent = null;

                if (x == start.x && y == start.y)
                    snode = node;

                if (x == goal.x && y == goal.y)
                    gnode = node;
            }
        }

        return null;
    }
}
