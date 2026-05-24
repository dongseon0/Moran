using UnityEngine;

public class CameraFollowClamp2D : MonoBehaviour
{
    [Header("Target")]
    [SerializeField] private Transform target;

    [Header("Map Bounds")]
    [SerializeField] private SpriteRenderer backgroundRenderer;

    [Header("Follow")]
    [SerializeField] private float followSpeed = 10f;

    private Camera cam;

    private void Awake()
    {
        cam = Camera.main;
    }

    private void LateUpdate()
    {
        if (target == null || backgroundRenderer == null || cam == null)
            return;

        Vector3 targetPosition = target.position;

        float cameraHalfHeight = cam.orthographicSize;
        float cameraHalfWidth = cameraHalfHeight * cam.aspect;

        Bounds bounds = backgroundRenderer.bounds;

        float minX = bounds.min.x + cameraHalfWidth;
        float maxX = bounds.max.x - cameraHalfWidth;
        float minY = bounds.min.y + cameraHalfHeight;
        float maxY = bounds.max.y - cameraHalfHeight;

        float clampedX = Mathf.Clamp(targetPosition.x, minX, maxX);
        float clampedY = Mathf.Clamp(targetPosition.y, minY, maxY);

        Vector3 desiredPosition = new Vector3(clampedX, clampedY, transform.position.z);

        transform.position = Vector3.Lerp(
            transform.position,
            desiredPosition,
            followSpeed * Time.deltaTime
        );
    }
}