using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridObject : MonoBehaviour
{

    public TacticGrid targetGrid;
    public Vector2Int positionOnGrid;

    private void Start()
    {
        Init();
    }

    private void Init()
    {
        positionOnGrid = targetGrid.GetGridPosition(transform.position);
        targetGrid.PlaceObject(positionOnGrid, this);
        Vector3 pos = targetGrid.GetWorldPosition(positionOnGrid.x, positionOnGrid.y, true);
        transform.position = pos;
    }
}
