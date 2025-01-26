using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using UnityEngine;

public class EnemySpawner : Spawner
{
    [SerializeField]
    private TacticGrid grid;

    protected override void StateChangeEventHandler(GameState state)
    {
        SpawnEnemyRandomGridPosition();
    }

    private void SpawnEnemyRandomGridPosition()
    {
        if (grid == null)
        {
            Debug.LogError("Grid is not assigned in the ObstacleSpawner!");
            return;
        }

        if (spaw == null)
        {
            Debug.LogError("Obstacle prefab is not assigned in the ObstacleSpawner!");
            return;
        }

        HashSet<Vector3> usedPositions = grid.GetOcupiedGridHashSet(); // Para evitar posiciones repetidas

        for (int i = 0; i < amount; i++)
        {
            bool positionAdded = false;
            Dictionary<Vector2Int, Vector3> randomPosition = null;
            while (!positionAdded)
            {
                randomPosition = grid.GetRandomSpawnPositionData();
                if (!usedPositions.Contains(randomPosition.First().Value))
                {
                    usedPositions.Add(randomPosition.First().Value);
                    positionAdded = true;
                }
            }

            Debug.Log($"Enemy {i} Position: {randomPosition}");
            GameObject instance = Instantiate(
                spaw,
                randomPosition.First().Value,
                Quaternion.identity
            );
            EnemyMovement enemyMovement = instance.GetComponent<EnemyMovement>();

            // Validar que se encontró el componente y asignar el targetGrid
            if (enemyMovement != null)
            {
                enemyMovement.SetTargetGrid(grid);
            }

            // Asigna la capa al objeto instanciado
            LayerMask obstacleMask = grid.GetObstacleLayer();
            Debug.Log("Obstacle mask " + obstacleMask);
            int layerIndex = LayerMask.NameToLayer("Enemy");
            instance.layer = layerIndex;
            instance.name =
                $"Enemy {i} - {randomPosition.First().Key.ToString()} - {randomPosition.First().Value.ToString()}";

            // Asigna la capa a todos los hijos
            SetLayerRecursively(instance, layerIndex);
        }

        // grid.RefreshPassableTerrain();
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
