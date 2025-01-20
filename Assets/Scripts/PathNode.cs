using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathNode : IEquatable<PathNode>
{
    public int pos_x;
    public int pos_y;
    public float gValue;
    public float hValue;
    public PathNode parentNode;

    public bool visited;

    public PathNode(int x, int y)
    {
        pos_x = x;
        pos_y = y;
    }

    public float fValue => gValue + hValue;

    public bool Equals(PathNode other)
    {
        return other != null && pos_x == other.pos_x && pos_y == other.pos_y;
    }

    public override bool Equals(object obj)
    {
        return obj is PathNode other && Equals(other);
    }

    public override int GetHashCode()
    {
        return pos_x.GetHashCode() ^ pos_y.GetHashCode();
    }
}
