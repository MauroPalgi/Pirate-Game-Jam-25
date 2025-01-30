using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDetection : MonoBehaviour
{
    // Start is called before the first frame update

    public bool PlayerDetected { get; private set; }
    public Vector2 DirectionToPlayer { get; private set; }

    [SerializeField]
    public GameObject Player;

    public Transform PlayerTransform { get; private set; }

    [SerializeField]
    private float PlayerDetectionDistance;

    private Transform _player;

    void Awake()
    {
        PlayerTransform = Player.transform;
        _player = PlayerTransform;
    }

    // Update is called once per frame
    void Update()
    {
        // Vector entre el enemigo y el jugador
        Vector3 enemyToPlayerVector = _player.position - transform.position;

        // Dirección hacia el jugador normalizada
        DirectionToPlayer = enemyToPlayerVector.normalized;

        // Detectar si el jugador está dentro del rango (usando sqrMagnitude para eficiencia)
        PlayerDetected =
            enemyToPlayerVector.sqrMagnitude <= PlayerDetectionDistance * PlayerDetectionDistance;
    }
}
