using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridPath
{
    private List<Node> _nodes = new List<Node>();
    private List<Vector3> _worldPosition = new List<Vector3>();

    public List<Vector3> GetWorldPositions()
    {
        return _worldPosition;
    }
    // Constructor
    public GridPath(List<Node> nodes, List<Vector3> worldPosition)
    {
        _nodes = nodes;
        _worldPosition = worldPosition;
    }

    public void LogPath()
    {
        for (int i = 0; i < _nodes.Count; i++)
        {
            Debug.Log(_nodes[i].ToString());
        }
    }
}
