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
                break;
            case GameState.SpawningLevel:
                break;
            case GameState.SpawningEnemies:
                break;
            case GameState.SpawningObstacles:
                break;
            case GameState.RestartSpawners:
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(newState), newState, null);
        }
        OnGameStateChanged?.Invoke(newState);
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
    SpawningEnemies,  
    SpawningObstacles,  
    RestartSpawners,
}
