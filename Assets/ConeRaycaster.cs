using UnityEngine;

public class ConeRaycaster : MonoBehaviour
{
    public Transform shootPoint; // Origin of the rays
    public float rayLength = 10f; // How far rays will go
    public float coneAngle = 150f; // Total angle of the cone
    public float rotateSpeed = 50f; // Speed of rotation
    public LayerMask hitMask; // For filtering raycast hits
    public Transform objectToRotate;

    private float currentAngle = 0f; // Tracks the current rotation within the cone

    void Update()
    {
        // Adjust aiming direction
        float horizontalInput = Input.GetAxis("Horizontal");
        currentAngle += horizontalInput * rotateSpeed * Time.deltaTime;

        // Clamp the aiming within the cone's boundaries
        float halfCone = coneAngle / 2f;
        currentAngle = Mathf.Clamp(currentAngle, -halfCone, halfCone);

        // Calculate current aiming direction
        Vector3 direction = Quaternion.Euler(0, currentAngle, 0) * transform.forward;
        
        if (objectToRotate != null)
        {
            objectToRotate.rotation = Quaternion.LookRotation(direction, Vector3.up);
        }

        // Visualize the ray in the Scene view
        Debug.DrawRay(shootPoint.position, direction * rayLength, Color.yellow,1f);
        
        // Shoot a ray when pressing space
        if (Input.GetKeyDown(KeyCode.Space))
        {
            ShootRay(direction);
        }
    }

    void ShootRay(Vector3 direction)
    {
        RaycastHit hitInfo;
        if (Physics.Raycast(shootPoint.position, direction, out hitInfo, rayLength, hitMask))
        {
            Debug.Log("Hit: " + hitInfo.collider.name);
        }
        else
        {
            Debug.Log("No hit");
        }
    }

    // Draw cone boundaries in the Scene view
    private void OnDrawGizmos()
    {
        if (shootPoint == null) return;

        float halfCone = coneAngle / 2f;

        // Directions for the left and right edges of the cone
        Vector3 leftEdge = Quaternion.Euler(0, -halfCone, 0) * transform.forward;
        Vector3 rightEdge = Quaternion.Euler(0, halfCone, 0) * transform.forward;

        // Draw edges
        Gizmos.color = Color.cyan;
        Gizmos.DrawLine(shootPoint.position, shootPoint.position + leftEdge * rayLength);
        Gizmos.DrawLine(shootPoint.position, shootPoint.position + rightEdge * rayLength);

        // Optional: Visualize cone filling with multiple rays
        int segments = 20; // Number of intermediate lines to represent the cone
        for (int i = 0; i <= segments; i++)
        {
            float stepAngle = Mathf.Lerp(-halfCone, halfCone, i / (float)segments);
            Vector3 segmentDir = Quaternion.Euler(0, stepAngle, 0) * transform.forward;
            Gizmos.DrawLine(shootPoint.position, shootPoint.position + segmentDir * rayLength);
        }
    }
}
