using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
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
        Debug.Log(usedPositions.Count);

        for (int i = 0; i < amount; i++)
        {
            bool positionAdded = false;
            Vector3 randomPosition = new Vector3();
            while (!positionAdded)
            {
                randomPosition = grid.GetRandomGridWorldPosition();
                if (!usedPositions.Contains(randomPosition))
                {
                    usedPositions.Add(randomPosition);
                    positionAdded = true;
                }
            }

            Debug.Log(usedPositions.Count);

            Debug.Log($"Enemy {i} Position: {randomPosition}");
            Debug.Log(spaw);
            GameObject instance = Instantiate(spaw, randomPosition, Quaternion.identity);
            EnemyMovement enemyMovement = instance.GetComponent<EnemyMovement>();

            // Validar que se encontró el componente y asignar el targetGrid
            if (enemyMovement != null)
            {
                enemyMovement.SetTargetGrid(grid);
            }

            // Asigna la capa al objeto instanciado
            LayerMask obstacleMask = grid.GetObstacleLayer();
            int layerIndex = LayerMask.NameToLayer("Enemy");
            instance.layer = layerIndex;
            instance.name = $"Enemy {i} x: {randomPosition}";

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
