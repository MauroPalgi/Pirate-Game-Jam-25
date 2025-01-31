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
            Debug.LogError("Grid is not assigned in the ObstacleSpawner!");
            return;
        }

        if (items == null)
        {
            Debug.LogError("Obstacle prefab is not assigned in the ObstacleSpawner!");
            return;
        }

        HashSet<Vector3> usedPositions = grid.GetOcupiedGridHashSet(); // Para evitar posiciones repetidas

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


            int rondomItmesIndex = UnityEngine.Random.Range(0, items.Count);

            GameObject instance = Instantiate(items[rondomItmesIndex], randomPosition, Quaternion.identity);
            // Asigna la capa al objeto instanciado
            LayerMask obstacleMask = grid.GetObstacleLayer();
            int layerIndex = LayerMask.NameToLayer("Wall");
            instance.layer = layerIndex;
            instance.name = $"Obstacle {i} x: {randomPosition}";

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





    protected override void StateChangeEventHandler(GameState state)
    {
        SpawnObstacleRandomGridPosition();
    }
}
