using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleSpawner : Singleton<ObstacleSpawner>
{
    [SerializeField]
    private GameObject obstacle;

    [SerializeField]
    private TacticGrid grid;

    [SerializeField]
    private int amount = 10;

    private void OnDestroy()
    {
        GameManager.OnGameStateChanged -= handleOnGameStateChanged;
    }

    private void Awake()
    {
        GameManager.OnGameStateChanged += handleOnGameStateChanged;
    }

    private void handleOnGameStateChanged(GameState state)
    {
        if (state == GameState.SpawningObstacle) // Corrige el estado si es necesario
        {
            Debug.Log("Start Menu");
            SpawnObstacleRandomGridPosition();
        }
    }

    private void SpawnObstacleRandomGridPosition()
    {
        if (grid == null)
        {
            Debug.LogError("Grid is not assigned in the ObstacleSpawner!");
            return;
        }

        if (obstacle == null)
        {
            Debug.LogError("Obstacle prefab is not assigned in the ObstacleSpawner!");
            return;
        }

        HashSet<Vector3> usedPositions = new HashSet<Vector3>(); // Para evitar posiciones repetidas
        for (int i = 0; i < amount; i++)
        {
            Vector3 randomPosition;
            do
            {
                randomPosition = grid.GetRandomGridWorldPosition();
            } while (usedPositions.Contains(randomPosition));

            usedPositions.Add(randomPosition);
            Debug.Log($"Obstacle {i} Position: {randomPosition}");

            GameObject instance = Instantiate(obstacle, randomPosition, Quaternion.identity);
            // Asigna la capa al objeto instanciado
            LayerMask obstacleMask = grid.GetObstacleLayer();
            int layerIndex = LayerMask.NameToLayer("Wall");
            instance.layer = layerIndex;
            instance.name = $"Obstacle {i} x: {randomPosition}";

            // Asigna la capa a todos los hijos
            SetLayerRecursively(instance, layerIndex);
        }
        grid.RefreshPassableTerrain();
    }

    private void SetLayerRecursively(GameObject obj, int layer)
    {
        obj.layer = layer;

        foreach (Transform child in obj.transform)
        {
            SetLayerRecursively(child.gameObject, layer);
        }
    }
}
