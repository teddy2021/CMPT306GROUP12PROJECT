using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AStarNode
{
    public Vector2Int coords { get; private set; }

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

    public AStarNode (Vector2Int loc, double h = 0.0, double g = 0.0)
    {
        coords = loc;

        H = h;
        G = g;
    }

    public AStarNode (int x, int y, double h = 0.0, double g = 0.0)
    {
        coords = new Vector2Int(x, y);

        H = h;
        G = g;
    }
}
