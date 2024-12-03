using UnityEngine;

public class CameraFollower : MonoBehaviour
{
    [SerializeField] private Transform cameraTarget; // Assign the CameraTarget in the Inspector
    [SerializeField] private float _followDuration = 0.5f; // Duration for the camera to move to the target

    private Vector3 _velocity; // Used for smooth damp

    private void LateUpdate()
    {
        // Calculate the desired position
        Vector3 desiredPosition = cameraTarget.position + new Vector3(0,0, -10);

        // Smoothly interpolate to the desired position based on the follow duration
        transform.position = Vector3.SmoothDamp(
            transform.position,
            desiredPosition,
            ref _velocity,
            _followDuration
        );
    }
}
