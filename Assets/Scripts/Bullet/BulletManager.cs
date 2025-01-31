using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class BulletManager : MonoBehaviour
{   
    public GameObject playerObject;

    //cheeky sound to play when hitting a wall
    private AudioSource audioSource;
    public AudioClip ricochetSound;
    public AudioClip hitWallSound;
    public AudioClip hitEnemySound;
    public AudioClip gunshotSound;

    public GameObject bulletSprite;

    public LayerMask collisionLayers;
    public string obstacleMask;
    public string enemyMask;

    private Vector3 startingDirection;
    private Vector3 currentDirection;
    private LineRenderer lineRenderer;

    public float bulletSpeed = 0.2f;
    public float bulletSize;
    public float coneAngle = 45f;
    public bool controllingBullet = true;

    private float currentAngle = 0f;
    public float rotateSpeed = 50f;
    public int numberOfRicochets;

    private Vector3 hitPoint;


    void Start(){
        startingDirection = transform.forward;
        currentDirection = startingDirection;
        audioSource = GetComponent<AudioSource>();

        lineRenderer = gameObject.AddComponent<LineRenderer>();
        lineRenderer.positionCount = 2;
        lineRenderer.startWidth = lineRenderer.endWidth = 0.05f;
        lineRenderer.material = new Material(Shader.Find("Sprites/Default"));
        lineRenderer.startColor = lineRenderer.endColor = Color.red;
    }

    void Update(){
        float spriteAngle = Mathf.Atan2(startingDirection.y, startingDirection.x) * Mathf.Rad2Deg;
        bulletSprite.transform.rotation = Quaternion.Euler(0,0,spriteAngle);
        if(controllingBullet == true){
            Vector3 tempDir = HandleAiming(currentDirection);

            Vector3 startPos = transform.position;
            Vector3 endPos = startPos + tempDir * 5f;

            lineRenderer.SetPosition(0, startPos);
            lineRenderer.SetPosition(1, endPos);
            if(Input.GetKeyDown(KeyCode.Space)){
                if(numberOfRicochets > 0){
                    audioSource.PlayOneShot(ricochetSound);
                }else{
                    audioSource.PlayOneShot(gunshotSound);
                }
                FireHitscanBullet(tempDir);
                controllingBullet = false;
            }
        }
    }

    Vector3 HandleAiming(Vector3 currentDir)
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        currentAngle += horizontalInput * rotateSpeed * Time.deltaTime;
        currentAngle = Mathf.Clamp(currentAngle, -coneAngle, coneAngle);
        Vector3 direction = Quaternion.Euler(0, currentAngle, 0) * currentDir;
        return direction;
    }

    void FireHitscanBullet(Vector3 direction)
    {
        Vector3 origin = transform.position;
        float maxDistance = Mathf.Infinity;

        //sorting hits by distance
        RaycastHit[] hits = Physics.RaycastAll(origin, direction, maxDistance, collisionLayers);
        System.Array.Sort(hits, (a, b) => a.distance.CompareTo(b.distance));
        

        foreach(RaycastHit hit in hits){
            string collisionMask = LayerMask.LayerToName(hit.collider.gameObject.layer);
            Debug.Log($"collisionMask: {collisionMask}");
            if (collisionMask == enemyMask){
                Debug.Log("+PIERCED");
                HandleEnemyCollision(hit);
                continue;
            }
            else if (collisionMask == obstacleMask){
                hitPoint = hit.point; // Bullet stops here
                HandleCollision(hit);
                break; // Stop checking further hits
            }
        }
        // Move the bullet visually to the hit point
        float travelTime = Vector3.Distance(origin, hitPoint) / bulletSpeed;
        transform.DOMove(hitPoint, travelTime).SetEase(Ease.Linear).OnComplete(() =>{transform.position = hitPoint;});
    }

    void HandleCollision(RaycastHit hit){
        
        Vector3 hitNormal = hit.normal;
        Debug.Log($"current DIRECTION: {currentDirection}");
        currentDirection = hitNormal.normalized; // Reflect direction
        Debug.Log($"new DIRECTION: {currentDirection}");
        StartCoroutine(EnableAimingAfterHit());
    }

    void HandleEnemyCollision(RaycastHit hit){
        StartCoroutine(DestroyEnemyAfterBulletHit(hit.collider.gameObject));
    }

    private IEnumerator DestroyEnemyAfterBulletHit(GameObject enemy){
        yield return new WaitForSeconds(Vector3.Distance(transform.position, hitPoint) / bulletSpeed);
        audioSource.PlayOneShot(hitEnemySound);
        Enemy enScript = enemy.GetComponent<Enemy>();
        if(enScript != null){
            enScript.SwitchState(EnemyState.Dead);
        }
    }

    private IEnumerator EnableAimingAfterHit(){
        yield return new WaitForSeconds(Vector3.Distance(transform.position, hitPoint) / bulletSpeed);
        Debug.Log("Hit a wall, the direction has been reflected");
        numberOfRicochets++;
        audioSource.PlayOneShot(hitWallSound);
        controllingBullet = true;
    }
}
