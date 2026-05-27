using UnityEngine;

public class InsideCameraFollow : MonoBehaviour
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
        cam = GetComponent<Camera>();
    }

    private void LateUpdate()
    {
        if (target == null) return;
        if (backgroundRenderer == null) return;
        if (cam == null) return;

        float cameraHalfHeight = cam.orthographicSize;
        float cameraHalfWidth = cameraHalfHeight * cam.aspect;

        Bounds bounds = backgroundRenderer.bounds;

        float minX = bounds.min.x + cameraHalfWidth;
        float maxX = bounds.max.x - cameraHalfWidth;
        float minY = bounds.min.y + cameraHalfHeight;
        float maxY = bounds.max.y - cameraHalfHeight;

        float clampedX = minX > maxX
            ? bounds.center.x
            : Mathf.Clamp(target.position.x, minX, maxX);

        float clampedY = minY > maxY
            ? bounds.center.y
            : Mathf.Clamp(target.position.y, minY, maxY);

        Vector3 desiredPosition = new Vector3(clampedX, clampedY, transform.position.z);

        transform.position = Vector3.Lerp(
            transform.position,
            desiredPosition,
            followSpeed * Time.deltaTime
        );
    }
}