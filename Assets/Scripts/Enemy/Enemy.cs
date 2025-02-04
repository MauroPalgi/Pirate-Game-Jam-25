using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : GridObject
{
    public static event Action<int> OnEnemyDead;
    [SerializeField]
    private EnemyState currentState = EnemyState.Idle;

    // Variables de estado

    [SerializeField]
    private float chaseRange = 10f;

    [SerializeField]
    private float attackRange = 2f;

    [SerializeField]
    private float patrolSpeed = 2f;

    [SerializeField]
    private float chaseSpeed = 4f;

    private int currentPatrolIndex = 0;
    private bool isWaiting = false;

    private EnemyMovement enemyMovement;

    private void Start()
    {
        enemyMovement = GetComponent<EnemyMovement>();
        StartCoroutine(StateMachine());
    }
    //Dejar solo Idle y chase
    //Simplificar statemachine
    private IEnumerator StateMachine()
    {
        while (true)
        {
            switch (currentState)
            {
                case EnemyState.Idle:
                    IdleState();
                    break;
                case EnemyState.Chase:
                    ChaseState();
                    break;

                case EnemyState.Dead:
                    DeadState();
                    break;
            }
            yield return null;
        }
    }

    private void DeadState()
    {

        //Change sprite to x
        //spawn bloodsplatter
        OnEnemyDead?.Invoke(1);
        Destroy(gameObject);
    }

    private void IdleState()
    {
        // Cambia a patrulla después de un tiempo
        if (!isWaiting)
        {
            isWaiting = true;
            StartCoroutine(WaitThenSwitchState(EnemyState.Chase, 2f)); // Espera 2 segundos
        }
    }


    private void ChaseState()
    {
        enemyMovement.HandleChaceState();
    }



    public void SwitchState(EnemyState newState)
    {
        currentState = newState;
        isWaiting = false;
    }

    private IEnumerator WaitThenSwitchState(EnemyState newState, float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        SwitchState(newState);
    }

    public EnemyState GetCurrentState()
    {
        return currentState;
    }


    private void OnDrawGizmos()
    {
    }


    protected void Awake()
    {
        EnemyMovement.OnPlayerReachead += HandlePlayerReach;

    }

    private void HandlePlayerReach(bool reach)
    {
        SwitchState(EnemyState.Idle);
    }



    private void OnDestroy()
    {
        EnemyMovement.OnPlayerReachead -= HandlePlayerReach;

    }
}

public enum EnemyState
{

    Idle,
    Chase,
    Dead
}
