using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    [SerializeField]
    private float _speed;
    [SerializeField]
    private float _rotationSpeed;

    private Rigidbody _rigidbody;
    private PlayerDetection _playerDetectionController;

    private Vector3 _targetDirection; // Cambiado a Vector3 para 3D

    void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _playerDetectionController = GetComponent<PlayerDetection>();
    }

    void FixedUpdate()
    {
        UpdateTargetDirection();
        // RotateTowardsTarget();
        // SetVelocity();
    }

    private void UpdateTargetDirection()
    {
        if (_playerDetectionController.PlayerDetected)
        {
            // Obtén la dirección hacia el jugador (en 3D)
            _targetDirection = _playerDetectionController.DirectionToPlayer.normalized;
        }
        else
        {
            // Sin dirección objetivo
            _targetDirection = Vector3.zero;
        }
    }

    private void RotateTowardsTarget()
    {
        if (_targetDirection == Vector3.zero)
        {
            return;
        }

        // Calcula la rotación hacia el objetivo
        Quaternion targetRotation = Quaternion.LookRotation(_targetDirection, Vector3.up);
        _rigidbody.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, _rotationSpeed * Time.deltaTime);
    }

    private void SetVelocity()
    {
        if (_targetDirection == Vector3.zero)
        {
            // Detener al enemigo si no hay objetivo
            _rigidbody.velocity = Vector3.zero;
        }
        else
        {
            // Mueve al enemigo hacia adelante en la dirección actual
            _rigidbody.velocity = transform.forward * _speed;
        }
    }
}
