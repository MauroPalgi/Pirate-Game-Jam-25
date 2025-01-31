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

    [SerializeField]
    float YAxis = 0.1f;

    private List<Vector3> _pathToDraw = null;

    public void SetPathToDraw(List<Vector3> path)
    {
        _pathToDraw = path;
    }

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
        _pathToDraw = new List<Vector3>();
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
                Node newNode = new Node(x, y);
                CalculateNodeElevation(newNode);
                CalculateNodePassableTerrain(newNode);
                grid[x, y] = newNode;
            }
        }
    }

    private void CalculateNodeElevation(Node node)
    {
        // Origen del rayo
        Vector3 rayOrigin = GetWorldPosition(node.pos_x, node.pos_y) + Vector3.up * 30f;

        // Dirección del rayo
        Vector3 rayDirection = Vector3.down;
        Ray ray = new Ray(rayOrigin, rayDirection);

        // Realizar el raycast
        if (
            Physics.Raycast(ray, out RaycastHit hit, 10f, terrainLayer)
            || Physics.Raycast(ray, out hit, 20f, obstacleLayer)
        )
        {
            // Asignar la elevación al nodo
            node.elevation = hit.point.y * 10f;

            // Opcional: Dibujar un punto donde el raycast golpea
            Debug.DrawLine(hit.point, hit.point + Vector3.up * 0.5f, Color.green, 2f);
        }
    }

    private void CalculateGridElevation()
    {
        for (int y = 0; y < width; y++)
        {
            for (int x = 0; x < length; x++)
            {
                Node currentNode = grid[x, y];
                CalculateNodeElevation(currentNode);
            }
        }
    }

    public void RefreshPassableTerrain()
    {
        CheckGridPassableTerrain(true);
    }

    private void CalculateNodePassableTerrain(Node node, bool elevation = false)
    {
        Vector3 worldPos = GetWorldPosition(node.pos_x, node.pos_y, elevation);
        bool passable = !Physics.CheckBox(
            worldPos,
            Vector3.one / 2,
            Quaternion.identity,
            obstacleLayer
        );
        node.passable = passable;
    }

    private void CheckGridPassableTerrain(bool elevation = false)
    {
        for (int y = 0; y < width; y++)
        {
            for (int x = 0; x < length; x++)
            {
                Node currenNode = grid[x, y];
                CalculateNodePassableTerrain(currenNode, elevation);
            }
        }
    }

    private void DrawNodeCubes()
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
                    }
                    Gizmos.DrawCube(pos, Vector3.one * cellSize * 0.1f);
                }
            }
        }
    }

    private void DrawPathLines()
    {
        if (_pathToDraw != null)
        {
            for (int i = 0; i < _pathToDraw.Count - 1; i++)
            {
                Vector3 wpNode = _pathToDraw[i];
                Vector3 nextWpNode = _pathToDraw[i + 1];
                Gizmos.color = Color.black;
                Gizmos.DrawLine(wpNode, nextWpNode);
            }
        }
    }

    private void OnDrawGizmos()
    {
        DrawNodeCubes();
        DrawPathLines();
    }

    public Vector3 GetWorldPosition(int x, int y, bool elevation = false)
    {
        Vector3 worldPosition = new Vector3(
            x * cellSize,
            YAxis,
            y * cellSize
        );
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
        int x = UnityEngine.Random.Range(0, width + 1);
        int y = UnityEngine.Random.Range(0, length+ 1);

        // Crear el diccionario con la posición del grid como clave y su posición en el mundo como valor
        Dictionary<Vector2Int, Vector3> spawnData = new Dictionary<Vector2Int, Vector3>
        {
            { new Vector2Int(x, y), new Vector3(x * cellSize, 0, y * cellSize) }
        };

        return spawnData;
    }

    public Vector3 GetRandomGridWorldPosition()
    {
        int x = UnityEngine.Random.Range(0, length);
        int y = UnityEngine.Random.Range(0, width);
        return GetWorldPosition(x, y, true);
    }

    public Vector2Int GetRandomPosition()
    {
        int x = UnityEngine.Random.Range(0, length );
        int y = UnityEngine.Random.Range(0, width);
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

    public Node GetNodeByWorldPosition(Transform transform)
    {
        Vector2Int gridPosition = GetGridPosition(transform.position);
        return grid[gridPosition.x, gridPosition.y];
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

    public GridPath GeneratePatrolPath(Transform transform)
    {
        Node node = GetNodeByWorldPosition(transform);
        List<Node> nodes = new List<Node>();
        List<Vector3> worldPos = new List<Vector3>();
        int steps = 5;
        PatrolDirections direction = PatrolDirections.Horizontal;
        for (int i = 0; i < steps; i++)
        {
            if (direction == PatrolDirections.Horizontal)
            {
                Node currentNode = grid[node.pos_x + i, node.pos_y];
                if (currentNode.passable == true)
                {
                    nodes.Add(currentNode);
                    worldPos.Add(GetWorldPosition(currentNode.pos_x, currentNode.pos_y));
                }
            }
            if (direction == PatrolDirections.Vertical)
            {
                Node currentNode = grid[node.pos_x, node.pos_y + 1];
                if (currentNode.passable)
                {
                    nodes.Add(currentNode);
                    worldPos.Add(GetWorldPosition(currentNode.pos_x, currentNode.pos_y));
                }
            }
        }
        return new GridPath(nodes, worldPos);
    }
}

public enum PatrolDirections
{
    Horizontal,
    Vertical,
}
