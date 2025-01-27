using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : GridObject
{
    [SerializeField]
    private State currentState = State.Idle;

    // Variables de estado

    [SerializeField]
    private Transform player;

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

    private IEnumerator StateMachine()
    {
        while (true)
        {
            switch (currentState)
            {
                case State.Idle:
                    IdleState();
                    break;

                case State.Patrol:
                    PatrolState();
                    break;

                case State.Chase:
                    ChaseState();
                    break;

                case State.Attack:
                    AttackState();
                    break;
            }
            yield return null;
        }
    }

    private void IdleState()
    {
        // Cambia a patrulla después de un tiempo
        if (!isWaiting)
        {
            isWaiting = true;
            StartCoroutine(WaitThenSwitchState(State.Patrol, 2f)); // Espera 2 segundos
        }
    }

    private void PatrolState() { }

    private void ChaseState()
    {
        if (player == null)
            return;

        // Moverse hacia el jugador
        transform.position = Vector3.MoveTowards(
            transform.position,
            player.position,
            chaseSpeed * Time.deltaTime
        );

        // Cambiar a estado de ataque si está cerca del jugador
        if (Vector3.Distance(transform.position, player.position) < attackRange)
        {
            SwitchState(State.Attack);
        }
        // Regresar a patrulla si el jugador está fuera de rango
        else if (Vector3.Distance(transform.position, player.position) > chaseRange)
        {
            SwitchState(State.Patrol);
        }
    }

    private void AttackState()
    {
        // Realizar el ataque (puedes agregar tu lógica de ataque aquí)
        Debug.Log("Atacando al jugador");

        // Cambiar a persecución si el jugador se aleja
        if (Vector3.Distance(transform.position, player.position) > attackRange)
        {
            SwitchState(State.Chase);
        }
    }

    private void SwitchState(State newState)
    {
        currentState = newState;
        isWaiting = false;
    }

    private IEnumerator WaitThenSwitchState(State newState, float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        SwitchState(newState);
    }

    public State GetCurrentState()
    {
        return currentState;
    }
}

public enum State
{
    Idle,
    Patrol,
    Chase,
    Attack
}
