using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;

public class TacticGrid : MonoBehaviour
{
    Node[,] grid;

    [SerializeField]
    private int width = 10;

    [SerializeField]
    private int length = 24;

    [SerializeField]
    private float cellSize = 3f;

    [SerializeField]
    LayerMask obstacleLayer;

    [SerializeField]
    LayerMask terrainLayer;

    [SerializeField]
    bool showGizmoLabel;

    public int GetLength()
    {
        return length;
    }

    public int GetWidth()
    {
        return width;
    }

    private void Awake()
    {
        GenerateGrid();
    }

    public LayerMask GetObstacleLayer()
    {
        return obstacleLayer;
    }

    private void GenerateGrid()
    {
        grid = new Node[length, width];

        for (int y = 0; y < width; y++)
        {
            for (int x = 0; x < length; x++)
            {
                grid[x, y] = new Node();
            }
        }

        CalculateElevation();
        CheckPassableTerrain();
    }

    private void CalculateElevation()
    {
        for (int y = 0; y < width; y++)
        {
            for (int x = 0; x < length; x++)
            {
                // Origen del rayo
                Vector3 rayOrigin = GetWorldPosition(x, y) + Vector3.up * 10f;
                // Dirección del rayo
                Vector3 rayDirection = Vector3.down;
                Ray ray = new Ray(rayOrigin, rayDirection);

                // Dibujar el raycast (línea desde el origen hasta el límite del rayo)
                // Debug.DrawLine(rayOrigin, rayOrigin + rayDirection * 20f, Color.red, 10f);

                // Realizar el raycast
                RaycastHit hit;
                if (Physics.Raycast(ray, out hit, 20f, terrainLayer))
                {
                    grid[x, y].elevation = hit.point.y;

                    // Opcional: Dibujar un punto donde el raycast golpea
                    // Debug.DrawLine(hit.point, hit.point + Vector3.up * 0.5f, Color.green, 2f);
                }
                if (Physics.Raycast(ray, out hit, 20f, obstacleLayer))
                {
                    grid[x, y].elevation = hit.point.y;

                    // Opcional: Dibujar un punto donde el raycast golpea
                    // Debug.DrawLine(hit.point, hit.point + Vector3.up * 0.5f, Color.green, 2f);
                }
            }
        }
    }

    public void RefreshPassableTerrain()
    {
        CheckPassableTerrain(true);
    }

    private void CheckPassableTerrain(bool elevation = false)
    {
        for (int y = 0; y < width; y++)
        {
            for (int x = 0; x < length; x++)
            {
                Vector3 worldPos = GetWorldPosition(x, y, elevation);
                bool passable = !Physics.CheckBox(
                    worldPos,
                    Vector3.one / 2,
                    Quaternion.identity,
                    obstacleLayer
                );
                grid[x, y].passable = passable;
            }
        }
    }

    private void OnDrawGizmos()
    {
        if (grid == null)
        {
            for (int y = 0; y < width; y++)
            {
                for (int x = 0; x < length; x++)
                {
                    Vector3 pos = GetWorldPosition(x, y);
                    Gizmos.color = Color.green;
                    Gizmos.DrawCube(pos, Vector3.one * cellSize * 0.1f);
                }
            }
        }
        else
        {
            for (int y = 0; y < width; y++)
            {
                for (int x = 0; x < length; x++)
                {
                    Vector3 pos = GetWorldPosition(x, y, true);
                    Gizmos.color = grid[x, y].passable ? Color.green : Color.red;
                    if (showGizmoLabel)
                    {
                        UnityEditor.Handles.color = Color.white; // Color del texto
                        string nodeInformation =
                            "isP: "
                            + grid[x, y].passable
                            + "\n"
                            + "w x: "
                            + pos.x
                            + " "
                            + " - w y: "
                            + pos.z
                            + "\n"
                            + "g x: "
                            + x
                            + " - g y: "
                            + y;
                        UnityEditor.Handles.Label(pos + Vector3.up * 0.3f, nodeInformation);
                    }
                    Gizmos.DrawCube(pos, Vector3.one * cellSize * 0.1f);
                }
            }
        }
    }

    public Vector3 GetWorldPosition(int x, int y, bool elevation = false)
    {
        Vector3 worldPosition =
            new Vector3(x * cellSize, elevation == true ? grid[x, y].elevation : 0f, y * cellSize);
        // + transform.position;
        return worldPosition;
    }

    public bool CheckBoundry(Vector2Int positionOnGrid)
    {
        return positionOnGrid.x >= 0
            && positionOnGrid.x < length
            && positionOnGrid.y >= 0
            && positionOnGrid.y < width;
    }

    public Vector2Int GetGridPosition(Vector3 worldPosition)
    {
        worldPosition.x += cellSize / 2;
        worldPosition.y += cellSize / 2;
        return new Vector2Int(
            Mathf.FloorToInt(worldPosition.x / cellSize),
            Mathf.FloorToInt(worldPosition.z / cellSize)
        );
    }

    public void PlaceObject(Vector2Int positionOnGrid, GridObject gridObject)
    {
        if (CheckBoundry(positionOnGrid))
        {
            grid[positionOnGrid.x, positionOnGrid.y].gridObject = gridObject;
        }
        else { }
    }

    public GridObject GetPlacedObject(Vector2Int gridPosition)
    {
        return CheckBoundry(gridPosition) ? grid[gridPosition.x, gridPosition.y].gridObject : null;
    }

    public void LogAllNodes()
    {
        for (int y = 0; y < width; y++)
        {
            for (int x = 0; x < length; x++) { }
        }
    }

    public bool CheckBoundry(int x, int y)
    {
        return CheckBoundry(new Vector2Int(x, y));
    }

    public bool CheckWalkable(int pos_x, int pos_y)
    {
        return grid[pos_x, pos_y].passable;
    }

    internal List<Vector3> ConvertPathNodesToWorlPosition(List<PathNode> path)
    {
        List<Vector3> worldPosition = new List<Vector3>();

        for (int i = 0; i < path.Count; i++)
        {
            worldPosition.Add(GetWorldPosition(path[i].pos_x, path[i].pos_y, true));
        }
        return worldPosition;
    }

    public Dictionary<Vector2Int, Vector3> GetRandomSpawnPositionData()
    {
        // Generar coordenadas aleatorias dentro del tamaño del grid
        int x = UnityEngine.Random.Range(0, width);
        int y = UnityEngine.Random.Range(0, length);

        // Depuración para verificar los valores
        Debug.Log("Grid Width: " + width);
        Debug.Log("Grid Length: " + length);
        Debug.Log("Random X: " + x);
        Debug.Log("Random Y: " + y);

        // Crear el diccionario con la posición del grid como clave y su posición en el mundo como valor
        Dictionary<Vector2Int, Vector3> spawnData = new Dictionary<Vector2Int, Vector3>
        {
            { new Vector2Int(x, y), new Vector3(x * cellSize, 0, y * cellSize) }
        };

        return spawnData;
    }

    public Vector3 GetRandomGridWorldPosition()
    {
        int x = UnityEngine.Random.Range(0, width);
        int y = UnityEngine.Random.Range(0, length);
        Debug.Log(width);
        Debug.Log(length);
        Debug.Log("x " + x);
        Debug.Log("y " + y);
        return GetWorldPosition(x, y);
    }

    public Vector2Int GetRandomPosition()
    {
        int x = UnityEngine.Random.Range(0, width);
        int y = UnityEngine.Random.Range(0, length);
        return new Vector2Int(x, y);
    }

    public HashSet<Vector3> GetOcupiedGridHashSet()
    {
        HashSet<Vector3> ocupiedNodes = new HashSet<Vector3>();
        for (int y = 0; y < width; y++)
        {
            for (int x = 0; x < length; x++)
            {
                if (grid[x, y].passable == false)
                {
                    ocupiedNodes.Add(GetWorldPosition(x, y));
                }
            }
        }
        return ocupiedNodes;
    }

    public Vector3 GetClosestGridNodePosition(Vector3 position)
    {
        // Tamaño de cada celda de la cuadrícula

        // Redondear la posición del mundo a la posición más cercana de la cuadrícula
        float x = Mathf.Round(position.x / cellSize) * cellSize;
        float y = Mathf.Round(position.y / cellSize) * cellSize;
        float z = Mathf.Round(position.z / cellSize) * cellSize;

        // Retorna la posición ajustada
        return new Vector3(x, y, z);
    }
}
