using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Spawner : MonoBehaviour
{
    [SerializeField]
    protected List<GameObject> items;

    [SerializeField]
    protected GameManager gameManager;

    // Start is called before the first frame update
    [SerializeField]
    protected int amount = 10;

    [SerializeField]
    protected GameState stateTrigger = GameState.Starting;

    public void Awake()
    {
        GameManager.OnGameStateChanged += HandleOnGameStateChanged;
    }

    private void OnDestroy()
    {
        GameManager.OnGameStateChanged -= HandleOnGameStateChanged;
    }

    private void HandleOnGameStateChanged(GameState state)
    {
        StateChangeEventHandler(state);
    }

    protected abstract void StateChangeEventHandler(GameState state);
}
