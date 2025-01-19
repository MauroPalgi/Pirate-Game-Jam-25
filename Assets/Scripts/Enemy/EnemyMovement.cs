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

    private Rigidbody _rigidbody;
    private PlayerDetection _playerDetectionController;

    private Vector3 _targetDirection; // Cambiado a Vector3 para 3D

    void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _playerDetectionController = GetComponent<PlayerDetection>();
    }

    // private void Update() {
    //     float distance = Vector3.Distance(transform.position, player.position);

    //         // Persigue al jugador si está más lejos que la distancia mínima
    //         if (distance > stopDistance)
    //         {
    //             Vector3 direction = (player.position - transform.position).normalized;
    //             transform.position += direction * speed * Time.deltaTime;

    //             // Opcional: hacer que la cápsula mire hacia el jugador
    //             transform.LookAt(new Vector3(player.position.x, transform.position.y, player.position.z));
    //         }
    // }

    void Update()
    {
        Transform player = _playerDetectionController.PlayerTransform;
        if (player != null)
        {
            float distance = Vector3.Distance(transform.position, player.position);
            // Persigue al jugador si está más lejos que la distancia mínima

            if (_playerDetectionController.PlayerDetected && distance > _stopDistance)
            {
                Vector3 direction = (player.position - transform.position).normalized;
                transform.position += direction * _speed * Time.deltaTime;

                // Opcional: hacer que la cápsula mire hacia el jugador
                transform.LookAt(new Vector3(player.position.x, transform.position.y, player.position.z));
            }
        }
    }

}
