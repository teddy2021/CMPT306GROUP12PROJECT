using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AStarNode : IComparable
{
    public Vector3Int coords { get; private set; }

    // f is defined by F = G + H
    public double F { get; private set; }

    private double _H;

    private double _G;

    // h is the herustic
    public double H
    {
        get { return _H; }

        set
        {
            // set the value
            _H = value;

            // update f
            F = _G + H;
        }
    }

    // g is the cost
    public double G
    {
        get { return _G; }

        set
        {
            // set the value
            _G = value;

            // update f
            F = _G + H;
        }
    }

    // the valid adjacent nodes
    public List<AStarNode> Neighbors { get; private set; }

    public AStarNode Parent { get; set; }

    public void UpdateNeighbors2D(AStarNode[,] walkMap, Vector2Int whereInMap, bool isStatic, bool allowDiags = true)
    {
        if (Neighbors != null && isStatic)
            return;

        Neighbors = new List<AStarNode>();
        var i = whereInMap.x;
        var h = whereInMap.y;
        var z = coords.z;

        try {
            var nnode = walkMap[i - 1, h];

            if (nnode.coords.z == z)
                Neighbors.Add(nnode);
        } catch { }

        try {
            var nnode = walkMap[i + 1, h];

            if (nnode.coords.z == z)
                Neighbors.Add(nnode);
        } catch { }

        try {
            var nnode = walkMap[i, h - 1];

            if (nnode.coords.z == z)
                Neighbors.Add(nnode);
        } catch { }

        try {
            var nnode = walkMap[i, h + 1];

            if (nnode.coords.z == z)
                Neighbors.Add(nnode);
        } catch { }

        if (allowDiags)
        {
            try {
                var node1 = walkMap[i + 1, h];
                var node2 = walkMap[i, h + 1];
                var nnode = walkMap[i + 1, h + 1];

                if (node1.coords.z == z && node2.coords.z == z && nnode.coords.z == z)
                    Neighbors.Add(nnode);
            } catch { }

            try {
                var node1 = walkMap[i - 1, h];
                var node2 = walkMap[i, h + 1];
                var nnode = walkMap[i - 1, h + 1];

                if (node1.coords.z == z && node2.coords.z == z && nnode.coords.z == z)
                    Neighbors.Add(nnode);
            } catch { }

            try {
                var node1 = walkMap[i + 1, h];
                var node2 = walkMap[i, h - 1];
                var nnode = walkMap[i + 1, h - 1];

                if (node1.coords.z == z && node2.coords.z == z && nnode.coords.z == z)
                    Neighbors.Add(nnode);
            } catch { }

            try {
                var node1 = walkMap[i - 1, h];
                var node2 = walkMap[i, h - 1];
                var nnode = walkMap[i - 1, h - 1];

                if (node1.coords.z == z && node2.coords.z == z && nnode.coords.z == z)
                    Neighbors.Add(nnode);
            } catch { }
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

    public Vector2Int ToVector2Int()
    {
        return new Vector2Int(coords.x, coords.y);
    }

    public Vector3Int ToVector3Int()
    {
        return new Vector3Int(coords.x, coords.y, coords.z);
    }

    public override String ToString()
    {
        return String.Format("(coords:{0},f:{1},g:{2},h:{3},hasParent:{4},neighbors:{5})", coords, F, G, H, Parent!=null, Neighbors.Count);
    }

    public int CompareTo(object obj)
    {
        if (obj == null) return 1;

        AStarNode otherAStarNode = obj as AStarNode;
        if (otherAStarNode != null)
            return F.CompareTo(otherAStarNode.F);
        else
           throw new ArgumentException("Object is not an AStarNode");
    }
}
