using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Spawner : Singleton<Spawner>
{
    [SerializeField]
    protected List<GameObject> items;
    // Start is called before the first frame update


    [SerializeField]
    protected int amount = 10;

    [SerializeField]
    protected GameState stateTrigger = GameState.Starting;


    protected override void Awake()
    {
        base.Awake();
        GameManager.OnGameStateChanged += HandleOnGameStateChanged;
    }

    private void OnDestroy()
    {
        GameManager.OnGameStateChanged -= HandleOnGameStateChanged;
    }

    private void HandleOnGameStateChanged(GameState state)
    {
        Debug.Log(state);
        if (state == stateTrigger || state == GameState.RestartSpawners)
        {
            StateChangeEventHandler(state);
        }
    }

    protected abstract void StateChangeEventHandler(GameState state);

}