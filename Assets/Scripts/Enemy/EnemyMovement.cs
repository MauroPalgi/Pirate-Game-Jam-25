using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    [SerializeField]
    private float _speed;

    [SerializeField]
    private float _rotationSpeed;

    [SerializeField]
    private float _stopDistance = 0.5f;

    [SerializeField]
    private TacticGrid _targetGrid;
    private Rigidbody _rigidbody;
    private PlayerDetection _playerDetectionController;
    private PathFinder _pathFinder;
    private Vector3 _targetDirection; // Cambiado a Vector3 para 3D
    private List<PathNode> _playerPath;
    private Transform _player;

    private GridPath _patrolPath = null;

    [SerializeField]
    private Enemy _enemyState;

    public void SetTargetGrid(TacticGrid targetGrid)
    {
        this._targetGrid = targetGrid;
    }

    void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _playerDetectionController = GetComponent<PlayerDetection>();
        _pathFinder = _targetGrid.GetComponent<PathFinder>();
        SnapIntoGrid();
    }

    private void OnDrawGizmos()
    {
        if (_playerDetectionController != null && _playerPath != null)
        {
            List<Vector3> wpNodes = _targetGrid.ConvertPathNodesToWorlPosition(_playerPath);

            for (int i = 0; i < wpNodes.Count - 1; i++)
            {
                Vector3 wpNode = wpNodes[i];
                Vector3 nextWpNode = wpNodes[i + 1];
                Gizmos.color = Color.black;
                Gizmos.DrawLine(wpNode, nextWpNode);
            }
        }
    }

    // Método para iniciar el movimiento por el camino
    public void FollowPath()
    {
        StopAllCoroutines(); // Detener cualquier movimiento previo
        StartCoroutine(MoveAlongPath());
    }

    // Corutina para mover al enemigo a través de los nodos del camino
    private IEnumerator MoveAlongPath()
    {
        foreach (PathNode node in _playerPath)
        {
            // Obtener la posición del mundo del nodo
            Vector3 targetPosition = _targetGrid.GetWorldPosition(node.pos_x, node.pos_y);

            // Mover al enemigo hacia el nodo
            while (Vector3.Distance(transform.position, targetPosition) >= 1f && !node.visited)
            {
                // transform.position = Vector3.MoveTowards(
                //     transform.position,
                //     targetPosition,
                //     _speed * Time.deltaTime
                // );

                transform.position = targetPosition;

                if (Vector3.Distance(transform.position, targetPosition) < 1.5f)
                {
                    node.visited = true;
                }
                yield return new WaitForSeconds(10f); // Esperar al siguiente frame
                yield return null; // Esperar al siguiente frame
            }
        }
    }

    public float GetDistanceToPlayer()
    {
        return _player != null ? Vector3.Distance(transform.position, _player.position) : -1f;
    }

    private void MoveAndLookAtPlayer()
    {
        if (_player != null)
        {
            float distance = Vector3.Distance(transform.position, _player.position);
            // Persigue al jugador si está más lejos que la distancia mínima

            if (_playerDetectionController.PlayerDetected && distance > _stopDistance)
            {
                Vector3 direction = (_player.position - transform.position).normalized;
                transform.position += direction * _speed * Time.deltaTime;
            }
        }
    }

    private void SnapIntoGrid()
    {
        Vector3 gridWorldPos = _targetGrid.GetClosestGridNodePosition(transform.position);

        transform.position = gridWorldPos;
    }

    private void LookAtPlayer()
    {
        // Opcional: hacer que la cápsula mire hacia el jugador
        transform.LookAt(new Vector3(_player.position.x, transform.position.y, _player.position.z));
    }

    private void MoveTowardsPlayer()
    {
        FollowPath();
    }

    private void CalculatePlayerPath()
    {
        if (_playerDetectionController != null && _playerPath == null)
        {
            Transform player = _playerDetectionController.PlayerTransform;
            Vector2Int worldPos = _targetGrid.GetGridPosition(transform.position);
            Vector2Int playerWorldPos = _targetGrid.GetGridPosition(player.position);
            _playerPath = _pathFinder.FindPath(
                worldPos.x,
                worldPos.y,
                playerWorldPos.x,
                playerWorldPos.y
            );
        }
    }

    private void Update()
    {
        if (_enemyState.GetCurrentState() == State.Patrol)
        {
            if (_patrolPath == null)
            {
                _patrolPath = _targetGrid.GeneratePatrolPath(transform);
                _patrolPath.LogPath();
                _targetGrid.SetPathToDraw(_patrolPath.GetWorldPositions());
            }
        }
    }

    private void LoadPlayerTranform()
    {
        if (_playerDetectionController != null)
        {
            _player = _playerDetectionController.PlayerTransform;
        }
    }
}
