using UnityEngine;

/// <summary>
/// Handles camera panning and smooth zooming for an isometric view.
/// Supports:
/// - Click-and-drag panning (or touch dragging)
/// - Smooth orthographic zoom with clamped limits
/// </summary>
public class IsometricCameraPan : MonoBehaviour
{
    [Header("Panning Settings")]
    [Tooltip("How fast the camera moves when dragging.")]
    public float panSpeed = 0.1f;

    [Header("Zoom Settings")]
    [Tooltip("How fast zooming responds to scroll input.")]
    public float zoomSpeed = 6f;

    [Tooltip("How smoothly the camera transitions to the new zoom level.")]
    public float zoomSmoothness = 5f;

    [Tooltip("Minimum allowed orthographic size.")]
    public float minZoom = 2f;

    [Tooltip("Maximum allowed orthographic size.")]
    public float maxZoom = 30f;

    /// <summary>Current target zoom value the camera should lerp toward.</summary>
    public float _currentZoom;

    [Header("Camera Reference")]
    [Tooltip("The orthographic camera being controlled.")]
    [SerializeField] public Camera _camera;

    /// <summary>World-space position where the drag started.</summary>
    private Vector3 touchStart;


    void Update()
    {
        // -------------------------------------------------
        // Handle Panning (Click-and-drag or touch-and-drag)
        // -------------------------------------------------

        // Store the initial drag point when the user first clicks
        if (Input.GetMouseButtonDown(0))
        {
            touchStart = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        }

        // While clicking & dragging, move the camera based on world delta
        if (Input.GetMouseButton(0))
        {
            Vector3 dragDelta =
                touchStart - Camera.main.ScreenToWorldPoint(Input.mousePosition);

            // Move the transform by the drag distance multiplied by speed
            transform.position += dragDelta * panSpeed;
        }


        // ------------------
        // Handle Zoom input
        // ------------------

        // Modify the target zoom amount based on scroll wheel input
        // Positive scroll = zoom in; negative scroll = zoom out
        _currentZoom = Mathf.Clamp(
            _currentZoom - Input.mouseScrollDelta.y * zoomSpeed * Time.deltaTime,
            minZoom,
            maxZoom
        );

        // Smoothly transition camera to new zoom level
        _camera.orthographicSize = Mathf.Lerp(
            _camera.orthographicSize,
            _currentZoom,
            zoomSmoothness * Time.deltaTime
        );
    }
}
