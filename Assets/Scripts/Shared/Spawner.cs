using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Spawner : Singleton<Spawner>
{
    [SerializeField]
    protected GameObject spaw;
    // Start is called before the first frame update


    [SerializeField]
    protected int amount = 10;

    [SerializeField]
    protected GameState stateTrigger = GameState.Starting;


    private void Awake()
    {
        GameManager.OnGameStateChanged += HandleOnGameStateChanged;
    }

    private void OnDestroy()
    {
        GameManager.OnGameStateChanged -= HandleOnGameStateChanged;
    }

    private void HandleOnGameStateChanged(GameState state)
    {
        if (state == stateTrigger)
        {
            StateChangeEventHandler(state);
        }
    }

    protected abstract void StateChangeEventHandler(GameState state);

}