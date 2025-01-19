using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDetection : MonoBehaviour
{
    // Start is called before the first frame update

    public bool PlayerDetected { get; private set; }
    public Vector2 DirectionToPlayer { get; private set; }

    [SerializeField]
    private float _playerDetectionDistance;

    private Transform _player;
    void Awake()
    {
        ConeRaycaster player = FindObjectOfType<ConeRaycaster>();
        _player = player.transform;
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 enemyToPlayerVector = _player.position - transform.position;

        DirectionToPlayer = enemyToPlayerVector.normalized;
        Debug.Log(enemyToPlayerVector.magnitude);
        Debug.Log(_playerDetectionDistance);

        if (enemyToPlayerVector.magnitude <= _playerDetectionDistance)
        {

            PlayerDetected = true;
        }
        else
        {

            PlayerDetected = false;
        }
    }
}