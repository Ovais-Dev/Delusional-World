using UnityEngine;

/// <summary>
/// Top-down 2D camera:
/// - Smooth follow target
/// - Clamp to bounds (BoxCollider2D or manual min/max)
/// - Draws gizmos for camera rect, bounds and target
/// Attach to an Orthographic Camera.
/// </summary>
[ExecuteAlways]
[RequireComponent(typeof(Camera))]
public class CameraFollow : MonoBehaviour
{
    [Header("Follow")]
    public Transform target;
    [Tooltip("How quickly the camera moves toward the target (higher = snappier).")]
    public float smoothTime = 0.12f;

    [Header("Bounds (choose one)")]
    [Tooltip("If assigned, camera clamps inside this BoxCollider2D (recommended).")]
    public BoxCollider2D boundsCollider;
    [Tooltip("Manual bounds if no collider assigned.")]
    public Vector2 manualMin = new Vector2(-10, -10);
    public Vector2 manualMax = new Vector2(10, 10);

    [Header("Settings")]
    [Tooltip("Whether to clamp horizontally and vertically.")]
    public bool clampX = true;
    public bool clampY = true;

    [Tooltip("If true, camera will never move if no target is set (useful in editor).")]
    public bool freezeIfNoTarget = false;

    // internals
    Camera _cam;
    Vector3 _velocity;

    private void Awake()
    {
        _cam = GetComponent<Camera>();
        if (!_cam.orthographic)
            Debug.LogWarning("TopDownCamera expects an orthographic camera for correct clamping.");
    }

    private void LateUpdate()
    {
        if (target == null)
        {
            if (freezeIfNoTarget) return;
            // nothing to follow
            return;
        }

        // desired target position (preserve camera z)
        Vector3 desired = new Vector3(target.position.x, target.position.y, transform.position.z);

        // smooth move
        Vector3 smoothed = Vector3.SmoothDamp(transform.position, desired, ref _velocity, smoothTime);

        // compute clamp extents based on orthographic size & aspect
        Vector2 minBound, maxBound;
        if (boundsCollider != null)
        {
            // world-space bounds from collider
            Bounds b = boundsCollider.bounds;
            minBound = b.min;
            maxBound = b.max;
        }
        else
        {
            minBound = manualMin;
            maxBound = manualMax;
        }

        // compute camera half extents in world units
        float vertExtent = _cam.orthographicSize;
        float horizExtent = vertExtent * _cam.aspect;

        // compute allowed camera center range
        float minX = minBound.x + horizExtent;
        float maxX = maxBound.x - horizExtent;
        float minY = minBound.y + vertExtent;
        float maxY = maxBound.y - vertExtent;

        Vector3 clamped = smoothed;

        if (clampX)
        {
            // if bounds smaller than camera view, keep center at midpoint
            if (minX > maxX)
                clamped.x = (minBound.x + maxBound.x) * 0.5f;
            else
                clamped.x = Mathf.Clamp(smoothed.x, minX, maxX);
        }

        if (clampY)
        {
            if (minY > maxY)
                clamped.y = (minBound.y + maxBound.y) * 0.5f;
            else
                clamped.y = Mathf.Clamp(smoothed.y, minY, maxY);
        }

        transform.position = clamped;
    }

    #region Gizmos
    private void OnDrawGizmos()
    {
        if (_cam == null) _cam = GetComponent<Camera>();

        // draw bounds (collider or manual)
        Vector2 minB = manualMin;
        Vector2 maxB = manualMax;
        if (boundsCollider != null)
        {
            Bounds b = boundsCollider.bounds;
            minB = b.min;
            maxB = b.max;
        }

        // bounds rectangle
        Gizmos.color = new Color(0.2f, 0.8f, 1f, 0.12f);
        Vector3 boundsCenter = (Vector3)((minB + maxB) * 0.5f);
        Vector3 boundsSize = new Vector3(maxB.x - minB.x, maxB.y - minB.y, 0.1f);
        Gizmos.DrawCube(boundsCenter, boundsSize);

        Gizmos.color = new Color(0.2f, 0.8f, 1f, 1f);
        Gizmos.DrawWireCube(boundsCenter, boundsSize);

        // draw camera view rect at current position
        float verts = _cam.orthographicSize;
        float horz = verts * _cam.aspect;
        Vector3 camCenter = transform.position;
        Vector3 camSize = new Vector3(horz * 2f, verts * 2f, 0.1f);

        Gizmos.color = new Color(1f, 0.7f, 0.2f, 0.15f);
        Gizmos.DrawCube(new Vector3(camCenter.x, camCenter.y, boundsCenter.z - 0.01f), camSize);
        Gizmos.color = new Color(1f, 0.7f, 0.2f, 1f);
        Gizmos.DrawWireCube(new Vector3(camCenter.x, camCenter.y, boundsCenter.z - 0.01f), camSize);

        // draw target and line
        if (target != null)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawSphere(target.position, 0.12f);
            Gizmos.color = Color.yellow;
            Gizmos.DrawLine(target.position, new Vector3(transform.position.x, transform.position.y, target.position.z));
        }
    }
    #endregion
}
