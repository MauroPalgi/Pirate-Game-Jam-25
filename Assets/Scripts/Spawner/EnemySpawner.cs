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

    public List<GameObject> enemiesObject = null;

    public int enemyAmount = 0;

    public int GetEnemyAmount()
    {
        return amount;
    }

    private void Update()
    {
        if (enemiesObject != null)
        {
            enemyAmount = enemiesObject.Count();
        }
    }

    private void SpawnEnemyRandomGridPosition()
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
                randomPosition = grid.GetRandomGridWorldPosition();
                if (!usedPositions.Contains(randomPosition))
                {
                    usedPositions.Add(randomPosition);
                    positionAdded = true;
                }
            }





            GameObject enemiesParent = GameObject.Find("Enemies");
            if (enemiesParent == null)
            {
                enemiesParent = new GameObject("Enemies"); // Si no existe, lo creamos
            }




            int randomItemsIndex = UnityEngine.Random.Range(0, items.Count);

            GameObject instance = Instantiate(
                items[randomItemsIndex],
                randomPosition,
                Quaternion.identity
            );

            enemiesObject.Add(instance);

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

        }
        else
        {

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

    protected override void StateChangeEventHandler(GameState state)
    {

        if (state == GameState.RestartSpawners)
        {

            DestroyAllEnemies();
            SpawnEnemyRandomGridPosition();
        }
        else
        {
            SpawnEnemyRandomGridPosition();
        }
    }
}
