using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public GameManager gameManager;
    public int currentLevel = 0;
    public int enemiesKilled = 0;
    private int enemyAmount;

    void ChangeLevel(){

    }




    //event shit
    protected void Awake()
    {
        Enemy.OnEnemyDead += HandleEnemyDying;
        EnemySpawner.OnEnemySpawnerStart += HandleEnemyAmount;
    }
    private void OnDestroy()
    {
        Enemy.OnEnemyDead -= HandleEnemyDying;
        EnemySpawner.OnEnemySpawnerStart -= HandleEnemyAmount;
    }
    private void HandleEnemyDying(int i){
        enemiesKilled = enemiesKilled + i;
        if(enemiesKilled >= enemyAmount){
            currentLevel++;
            ChangeLevel();
        }
    }
    private void HandleEnemyAmount(int i){
        enemyAmount = i;
    }
}
