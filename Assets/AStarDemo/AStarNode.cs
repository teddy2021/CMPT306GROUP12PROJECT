using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AStarNode
{
    public Vector3Int coords { get; private set; }

    // f is defined by F = G + H
    public double F { get; private set; }

    // h is the herustic
    public double H
    {
        get { return H; }

        set
        {
            // set the value
            H = value;

            // update f
            F = G + H;
        }
    }

    // g is the cost
    public double G
    {
        get { return G; }

        set
        {
            // set the value
            G = value;

            // update f
            F = G + H;
        }
    }

    // the valid adjacent nodes
    public List<AStarNode> Neighbors { get; private set; }

    public AStarNode Parent { get; set; }

    public void UpdateNeighbors (AStarNode[,] walkMap, bool isStatic, bool allowDiags = true)
    {
        if (Neighbors != null && isStatic)
            return;

        Neighbors = new List<AStarNode>();
        var x = coords.x;
        var y = coords.y;
        var z = coords.z;
        var maxX = walkMap.GetUpperBound(0);
        var maxY = walkMap.GetUpperBound(1);

        // check -x
        if (x > 0)
        {
            var nnode = walkMap[x - 1, y];

            if (nnode.coords.z == z)
                Neighbors.Add(nnode);
        }

        // check +x
        if (x < maxX)
        {
            var nnode = walkMap[x + 1, y];

            if (nnode.coords.z == z)
                Neighbors.Add(nnode);
        }

        // check -y
        if (y < 0)
        {
            var nnode = walkMap[x, y - 1];

            if (nnode.coords.z == z)
                Neighbors.Add(nnode);
        }

        // check +y
        if (y < maxY)
        {
            var nnode = walkMap[x, y + 1];

            if (nnode.coords.z == z)
                Neighbors.Add(nnode);
        }

        if (allowDiags)
        {
            // check -x -y
            if (x > 0 && y > 0)
            {
                var nnode = walkMap[x - 1, y - 1];

                if (nnode.coords.z == z)
                    Neighbors.Add(nnode);
            }

            // check +x -y
            if (x < maxX && y > 0)
            {
                var nnode = walkMap[x + 1, y - 1];

                if (nnode.coords.z == z)
                    Neighbors.Add(nnode);
            }

            // check +x +y
            if (x < maxX && y < maxY)
            {
                var nnode = walkMap[x + 1, y + 1];

                if (nnode.coords.z == z)
                    Neighbors.Add(nnode);
            }

            // check -x +y
            if (x > 0 && y < maxY)
            {
                var nnode = walkMap[x - 1, y + 1];

                if (nnode.coords.z == z)
                    Neighbors.Add(nnode);
            }
        }
    }

    public AStarNode (Vector3Int loc, double h = 0.0, double g = 0.0)
    {
        coords = loc;

        H = h;
        G = g;
    }

    public AStarNode (int x, int y, int z, double h = 0.0, double g = 0.0)
    {
        coords = new Vector3Int(x, y, z);

        H = h;
        G = g;
    }
}
