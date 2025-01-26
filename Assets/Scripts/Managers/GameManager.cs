using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    // Start is called before the first frame update
    public GameState State { get; private set; }
    public static event Action<GameState> OnGameStateChanged;
    void Start() => ChangeState(GameState.Starting);

    public void ChangeState(GameState newState)
    {
        State = newState;
        switch (newState)
        {
            case GameState.Starting:
                break;
            case GameState.SpawningGrid:
                break;
            case GameState.SpawningPlayer:
                break;
            case GameState.SpawningEnemies:
                break;
            case GameState.SpawningObstacle:
                HandleSpawnObtacles();
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(newState), newState, null);
        }
        OnGameStateChanged?.Invoke(newState);
    }

    private void HandleSpawnObtacles()
    {
        // Debug.Log("handle spawning");
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            ChangeState(GameState.SpawningObstacle);
        }
    }
}

public enum GameState
{
    Starting,
    SpawningGrid,
    SpawningPlayer,
    SpawningEnemies,
    SpawningObstacle,
}
