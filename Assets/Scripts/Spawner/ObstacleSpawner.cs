using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;

public class ObstacleSpawner : Spawner
{
    [SerializeField]
    private TacticGrid grid;

    private void SpawnObstacleRandomGridPosition()
    {
        if (grid == null)
        {

            return;
        }

        if (items == null)
        {

            return;
        }

        HashSet<Vector3> usedPositions = grid.GetOcupiedGridHashSet(); // Para evitar posiciones repetidas

        for (int i = 0; i < amount; i++)
        {
            bool positionAdded = false;
            Vector3 randomPosition = new Vector3();
            while (!positionAdded)
            {
                randomPosition = grid.GetRandomGridWorldPosition() + new Vector3(0, 2, 0);
                if (!usedPositions.Contains(randomPosition))
                {
                    usedPositions.Add(randomPosition);
                    positionAdded = true;
                }
            }

            int rondomItmesIndex = UnityEngine.Random.Range(0, items.Count);

            GameObject obstacleParent = GameObject.Find("Obstacle");
            if (obstacleParent == null)
            {
                obstacleParent = new GameObject("Obstacle"); // Si no existe, lo creamos
            }

            GameObject instance = Instantiate(
                items[rondomItmesIndex],
                randomPosition,
                Quaternion.identity
            );
            // Asigna la capa al objeto instanciado
            LayerMask obstacleMask = grid.GetObstacleLayer();
            int layerIndex = LayerMask.NameToLayer("Wall");
            instance.layer = layerIndex;
            instance.name = $"Obstacle {i} x: {randomPosition}";
            instance.transform.SetParent(obstacleParent.transform);

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

    public void DestroyAllObstacle()
    {
        GameObject enemiesParent = GameObject.Find("Obstacle");

        if (enemiesParent != null)
        {
            Destroy(enemiesParent);

        }
        else
        {

        }
    }

    protected override void StateChangeEventHandler(GameState state)
    {
        if (state == GameState.RestartSpawners || state == GameState.SpawningObstacles)
        {
            DestroyAllObstacle();
            SpawnObstacleRandomGridPosition();
        }
    }
}
