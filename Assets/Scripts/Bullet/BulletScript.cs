using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Unity.Mathematics;

public class BulletScript : MonoBehaviour
{
    //cheeky sound to play when hitting a wall
    private AudioSource audioSource;
    public AudioClip soundClip;

    public GameObject bulletSprite;

    public string layerMask;
    private Rigidbody rb;
    private BoxCollider bulletCollider;
    private Vector3 startingDirection;
    public float bulletVelocity;
    public float coneAngle = 45f;
    public bool controllingBullet = true;

    private float currentAngle = 0f;
    public float rotateSpeed = 50f;
    public int numberOfRicochets;
    void Start(){
        startingDirection = transform.forward;

        //getting the components
        rb = GetComponent<Rigidbody>();
        bulletCollider = GetComponent<BoxCollider>();
        audioSource = GetComponent<AudioSource>();
    }

    void Update(){
        float spriteAngle = Mathf.Atan2(startingDirection.y, startingDirection.x) * Mathf.Rad2Deg;
        Debug.Log(spriteAngle);
        bulletSprite.transform.rotation = Quaternion.Euler(0,0,spriteAngle);
        if(controllingBullet == true){
            Vector3 tempDir = HandleAiming();
            if(Input.GetKeyDown(KeyCode.Space)){
                Debug.Log("Trying to launch the bullet");
                LaunchSelf(tempDir,bulletVelocity);
                controllingBullet = false;
            }
        }
    }



    private void OnCollisionEnter(Collision collision){
        string collisionMask = LayerMask.LayerToName(collision.gameObject.layer);
        if(collisionMask == layerMask){
            numberOfRicochets++;
            audioSource.PlayOneShot(soundClip);
            // Stop the bullet momentarily
            if (rb != null){
                //stops bullet and calculates its reverse direction
                rb.velocity = Vector3.zero;
                Vector3 hitNormal = collision.contacts[0].normal;
                Vector3 ricochetDirection = Vector3.Reflect(startingDirection, hitNormal);

                VisualizeCone(ricochetDirection,coneAngle);
                

                //mini movement just to separate the bullet from the wall 
                Vector3 displacement = hitNormal * 0.2f;
                transform.position += displacement;

                //   :3
                startingDirection = ricochetDirection.normalized;
                controllingBullet = true;
                //TO RANDOMIZE A DIRECTION TO RICOCHET

                //Vector3 newDirection = Quaternion.Euler(0,Random.Range(-coneAngle,coneAngle),0) * oppositeDirection;
                //startingDirection = newDirection;
                //LaunchSelf(newDirection.normalized,bulletVelocity);
            }
        }
    }
    //chatgepeteado
    private void VisualizeCone(Vector3 centerDirection, float coneAngle)
    {
        float visualizationDistance = 5f; // Adjust how far you want the lines to go

        // Edge 1 of the cone
        Vector3 edge1 = Quaternion.Euler(0, -coneAngle, 0) * centerDirection;
        Debug.DrawLine(transform.position, transform.position + edge1 * visualizationDistance, Color.red, 2f);

        // Edge 2 of the cone
        Vector3 edge2 = Quaternion.Euler(0, coneAngle, 0) * centerDirection;
        Debug.DrawLine(transform.position, transform.position + edge2 * visualizationDistance, Color.red, 2f);

        // Center line of the cone
        Debug.DrawLine(transform.position, transform.position + centerDirection * visualizationDistance, Color.blue, 2f);

        // Optional: Additional lines for finer visualization
        int steps = 5; // Increase this for smoother visualization
        for (int i = 1; i < steps; i++)
        {
            float stepAngle = Mathf.Lerp(-coneAngle, coneAngle, i / (float)steps);
            Vector3 stepDirection = Quaternion.Euler(0, stepAngle, 0) * centerDirection;
            Debug.DrawLine(transform.position, transform.position + stepDirection * visualizationDistance, Color.green, 2f);
        }
    }

    public void LaunchSelf(Vector3 newDirection, float velocity){
        if(rb == null ) return;
        rb.velocity = newDirection.normalized * velocity;
    }

    Vector3 HandleAiming(){
        VisualizeCone(startingDirection,coneAngle);
        float horizontalInput = Input.GetAxis("Horizontal");
        currentAngle += horizontalInput * rotateSpeed * Time.deltaTime;

        currentAngle = Mathf.Clamp(currentAngle, -coneAngle, coneAngle);

        Vector3 direction = Quaternion.Euler(0, currentAngle, 0) * startingDirection;
        Debug.DrawRay(transform.position, direction * 10f, Color.yellow);
        return direction;
        
    }

}
