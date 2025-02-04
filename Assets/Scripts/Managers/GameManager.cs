using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    // Start is called before the first frame update
    [SerializeField]
    public GameState State = GameState.Starting;
    public TacticGrid Grid;

    /*
        public LevelManager levelManager;
        public MenuManager menuManager;
        public EnemySpawner enemySpawner;
        public ObstacleSpawner obstacleSpawner;
        public GameObject Player;
        */
    public static event Action<GameState> OnGameStateChanged;

    public void ChangeState(GameState newState)
    {
        State = newState;
        switch (newState)
        {
            case GameState.Starting:
                HandleStarting();
                break;
            case GameState.SpawningLevel:
                HandleSpawningLevel();
                break;
            case GameState.SpawningPlayer:
                break;
            case GameState.SpawningEnemies:
                break;
            case GameState.SpawningObstacle:
                break;
            case GameState.RestartSpawners:
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(newState), newState, null);
        }
        OnGameStateChanged?.Invoke(newState);
    }

    private void HandleSpawningLevel()
    {
        Debug.Log("HandleSpawningLevel");
    }

    void HandleStarting()
    {
        Debug.Log("HandleSpawningLevel");

        /*
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        GameObject bulletSpawn = GameObject.FindGameObjectWithTag("bulletSpawn");
        GameObject bullet = GameObject.FindGameObjectWithTag("Bullet");
        player.transform.position = new Vector3(UnityEngine.Random.Range(5, 66), 2.6f, 2f);
        bullet.transform.position = bulletSpawn.transform.position;*/
    }

    private IEnumerator WaitThenSwitchState(GameState newState, float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        SwitchState(newState);
    }

    public void SwitchState(GameState newState)
    {
        State = newState;
    }
}

public enum GameState
{
    Starting,
    SpawningLevel,
    SpawningGrid,
    SpawningPlayer,
    SpawningEnemies,
    SpawningObstacle,
    RestartSpawners,
}
