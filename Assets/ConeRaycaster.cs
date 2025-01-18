using UnityEngine;

public class ConeRaycaster : MonoBehaviour
{
    public Transform shootPoint; // Origin of the rays
    public float rayLength = 10f; // How far rays will go
    public float coneAngle = 150f; // Total angle of the cone
    public float rotateSpeed = 50f; // Speed of rotation
    public LayerMask hitMask; // For filtering raycast hits
    public Transform objectToRotate;

    public GameObject projectilePrefab; // Prefab for the projectile
    public float projectileSpeed = 20f; // Speed of the projectile

    private float currentAngle = 0f; // Tracks the current rotation within the cone
    private bool canShoot = true;
    void Update()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        currentAngle += horizontalInput * rotateSpeed * Time.deltaTime;

        float halfCone = coneAngle / 2f;
        currentAngle = Mathf.Clamp(currentAngle, -halfCone, halfCone);

        Vector3 direction = Quaternion.Euler(0, currentAngle, 0) * transform.forward;

        if (objectToRotate != null)
        {
            objectToRotate.rotation = Quaternion.LookRotation(direction, Vector3.up);
        }

        Debug.DrawRay(shootPoint.position, direction * rayLength, Color.yellow, 1f);

        if (Input.GetKeyDown(KeyCode.Space))
        {
            ShootRay(direction);
        }
    }

    void ShootRay(Vector3 direction){
        if(canShoot == true){
            if (projectilePrefab == null){
                Debug.LogWarning("Projectile prefab is not assigned!");
                return;
            }

            GameObject projectile = Instantiate(projectilePrefab, shootPoint.position, Quaternion.identity);
            canShoot = false;
            Rigidbody projectileRigidbody = projectile.GetComponent<Rigidbody>();
            if (projectileRigidbody != null){
                projectileRigidbody.velocity = direction.normalized * projectileSpeed;
            }
            else{
                Debug.LogWarning("Projectile prefab does not have a Rigidbody component!");
            }
        }
    }

    public void ResetShoot(){
        canShoot = true;
    }

}
