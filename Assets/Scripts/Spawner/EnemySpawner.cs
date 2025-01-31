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

    public static event Action<int> OnEnemySpawnerStart;

    protected override void StateChangeEventHandler(GameState state)
    {
        Debug.Log($"estado {state}");
        if(state == GameState.RestartSpawners){
            Debug.Log($"HOLAAAAAA");
            DestroyAllEnemies();
            SpawnEnemyRandomGridPosition();
        }else{
            SpawnEnemyRandomGridPosition();
        }
    }

    void Start()
    {
        OnEnemySpawnerStart?.Invoke(amount);
    }
    private void SpawnEnemyRandomGridPosition()
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

            Debug.Log($"Obstacle {i} Position: {randomPosition}");

            GameObject enemiesParent = GameObject.Find("Enemies");
            if (enemiesParent == null)
            {
                enemiesParent = new GameObject("Enemies"); // Si no existe, lo creamos
            }

            Debug.Log(usedPositions.Count);
            Debug.Log($"Obstacle {i} Position: {randomPosition}");

            int randomItemsIndex = UnityEngine.Random.Range(0, items.Count);

            GameObject instance = Instantiate(items[randomItemsIndex], randomPosition, Quaternion.identity);

            // Asigna la capa al objeto instanciado
            int layerIndex = LayerMask.NameToLayer("Enemy");
            instance.layer = layerIndex;
            instance.name = $"Enemy {i} x: {randomPosition}";

            // Hacer que el enemigo sea hijo del GameObject "Enemies"
            instance.transform.SetParent(enemiesParent.transform);

            // Obtener el script de movimiento y asignar la grilla
            EnemyMovement enemyMovement = instance.GetComponent<EnemyMovement>();
            if (enemyMovement != null)
            {
                enemyMovement.SetTargetGrid(grid);
            }

            instance.layer = layerIndex;
            instance.name = $"Enemy {i} x: {randomPosition}";

            // Asigna la capa a todos los hijos
            SetLayerRecursively(instance, layerIndex);
        }

        // grid.RefreshPassableTerrain();
    }


    public void DestroyAllEnemies()
    {
        GameObject enemiesParent = GameObject.Find("Enemies");

        if (enemiesParent != null)
        {
            Destroy(enemiesParent);
            Debug.Log("Todos los enemigos han sido eliminados.");
        }
        else
        {
            Debug.Log("No hay enemigos para eliminar.");
        }
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
