using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleGrid : MonoBehaviour
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
    void Start()
    {

    }
    private void Awake()
    {
        GenerateGrid();

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
        Vector3 rayOrigin = GetWorldPosition(node.pos_x, node.pos_y) + Vector3.up * 10f;

        // Dirección del rayo
        Vector3 rayDirection = Vector3.down;
        Ray ray = new Ray(rayOrigin, rayDirection);

        // Realizar el raycast
        if (
            Physics.Raycast(ray, out RaycastHit hit, 20f, terrainLayer)
            || Physics.Raycast(ray, out hit, 20f, obstacleLayer)
        )
        {
            // Asignar la elevación al nodo
            node.elevation = hit.point.y;

            // Opcional: Dibujar un punto donde el raycast golpea
            Debug.DrawLine(hit.point, hit.point + Vector3.up * 0.5f, Color.green, 2f);
        }
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
    public Vector3 GetWorldPosition(int x, int y, bool elevation = false)
    {
        Vector3 worldPosition = new Vector3(
            x * cellSize,
            elevation == true ? grid[x, y].elevation : 0f,
            y * cellSize
        );
        // + transform.position;
        return worldPosition;
    }
    // Update is called once per frame




    private void OnDrawGizmos()
    {
        DrawNodeCubes();
        // DrawPathLines();
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


    public Vector2Int GetGridPosition(Vector3 worldPosition)
    {
        worldPosition.x += cellSize / 2;
        worldPosition.y += cellSize / 2;
        return new Vector2Int(
            Mathf.FloorToInt(worldPosition.x / cellSize),
            Mathf.FloorToInt(worldPosition.z / cellSize)
        );
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



    public Dictionary<Vector2Int, Vector3> GetRandomSpawnPositionData()
    {
        // Generar coordenadas aleatorias dentro del tamaño del grid
        int x = UnityEngine.Random.Range(0, width);
        int y = UnityEngine.Random.Range(0, length);

        // Crear el diccionario con la posición del grid como clave y su posición en el mundo como valor
        Dictionary<Vector2Int, Vector3> spawnData = new Dictionary<Vector2Int, Vector3>
        {
            { new Vector2Int(x, y), new Vector3(x * cellSize, 0, y * cellSize) }
        };

        return spawnData;
    }


    public LayerMask GetObstacleLayer()
    {
        return obstacleLayer;
    }


    public Vector3 GetRandomGridWorldPosition()
    {
        int x = UnityEngine.Random.Range(0, length);
        int y = UnityEngine.Random.Range(0, width);
        return GetWorldPosition(x, y);
    }



    void Update()
    {

    }
}
