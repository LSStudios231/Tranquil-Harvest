using UnityEngine;

public class Camera : MonoBehaviour
{
    public Transform target; // Reference to the player's transform
    public float smoothSpeed = 0.125f; // Speed at which the camera follows the player
    public Vector3 offset; // Offset from the player's position

    private void FixedUpdate()
    {
        if (target != null)
        {
            Vector3 desiredPosition = target.position + offset; // Calculate the desired position for the camera
            desiredPosition.z = transform.position.z; // Maintain the camera's original z-position
            Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed); // Smoothly interpolate between current position and desired position
            transform.position = smoothedPosition; // Set the camera's position to the smoothed position
        }
    }
}

