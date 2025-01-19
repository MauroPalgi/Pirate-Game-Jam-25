using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node
{
    public GridObject gridObject;
    public float elevation;
    public bool passable;

    public override string ToString()
    {
        string objDescription = gridObject != null ? gridObject.ToString() : "None";
        return $"Elevation: {elevation:F2}, Passable: {passable}, GridObject: {objDescription}";
    }

}
