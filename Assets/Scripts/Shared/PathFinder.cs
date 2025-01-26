using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(TacticGrid))]
public class PathFinder : MonoBehaviour
{
    [SerializeField]
    public TacticGrid gridMap;

    [SerializeField]
    PathNode[,] pathNodes;

    void Start()
    {
        Init();
    }

    public void Init()
    {
        if (gridMap == null)
        {
            gridMap = GetComponent<TacticGrid>();
        }

        pathNodes = new PathNode[gridMap.GetLength(), gridMap.GetWidth()];
        for (int y = 0; y < gridMap.GetWidth(); y++)
        {
            for (int x = 0; x < gridMap.GetLength(); x++)
            {
                pathNodes[x, y] = new PathNode(x, y);
            }
        }
    }

    public List<PathNode> FindPath(int startX, int startY, int endX, int endY)
    {
        PathNode startNode = pathNodes[startX, startY];
        PathNode endNode = pathNodes[endX, endY];

        List<PathNode> openList = new List<PathNode> { startNode };
        List<PathNode> closedList = new List<PathNode>();

        while (openList.Count > 0)
        {
            PathNode currentNode = openList[0];
            for (int i = 1; i < openList.Count; i++)
            {
                if (
                    openList[i].fValue < currentNode.fValue
                    || (
                        openList[i].fValue == currentNode.fValue
                        && openList[i].hValue < currentNode.hValue
                    )
                )
                {
                    currentNode = openList[i];
                }
            }

            openList.Remove(currentNode);
            closedList.Add(currentNode);

            if (currentNode.Equals(endNode))
            {
                return RetracePath(startNode, endNode);
            }

            foreach (PathNode neighbour in GetNeighbours(currentNode))
            {
                if (
                    closedList.Contains(neighbour)
                    || !gridMap.CheckWalkable(neighbour.pos_x, neighbour.pos_y)
                )
                    continue;

                float movementCost = currentNode.gValue + CalculateDistance(currentNode, neighbour);

                if (!openList.Contains(neighbour) || movementCost < neighbour.gValue)
                {
                    neighbour.gValue = movementCost;
                    neighbour.hValue = CalculateDistance(neighbour, endNode);
                    neighbour.parentNode = currentNode;

                    if (!openList.Contains(neighbour))
                    {
                        openList.Add(neighbour);
                    }
                }
            }
        }
        return null;
    }

    private IEnumerable<PathNode> GetNeighbours(PathNode node)
    {
        // Debug.Log("Nodo" + node.ToString());
        List<PathNode> neighbours = new List<PathNode>();
        for (int x = -1; x <= 1; x++)
        {
            for (int y = -1; y <= 1; y++)
            {
                if (x == 0 && y == 0)
                    continue;

                int checkX = node.pos_x + x;
                int checkY = node.pos_y + y;

                if (gridMap.CheckBoundry(checkX, checkY))
                {
                    neighbours.Add(pathNodes[checkX, checkY]);
                }
            }
        }
        return neighbours;
    }

    public int CalculateDistance(PathNode currentNode, PathNode target)
    {
        int distX = Mathf.Abs(currentNode.pos_x - target.pos_x);
        int distY = Mathf.Abs(currentNode.pos_y - target.pos_y);
        return 14 * Mathf.Min(distX, distY) + 10 * Mathf.Abs(distX - distY);
    }

    public List<PathNode> RetracePath(PathNode startNode, PathNode endNode)
    {
        List<PathNode> path = new List<PathNode>();
        PathNode currentNode = endNode;

        while (currentNode != startNode)
        {
            path.Add(currentNode);
            currentNode = currentNode.parentNode;
        }

        path.Reverse();
        return path;
    }
}
