using UnityEngine;

public class IsometricCameraPan : MonoBehaviour
{
    public float panSpeed = 0.1f; // Adjust this value to control pan speed

    public float zoomSpeed = 6;
    public float zoomSmoothness = 5;
    public float minZoom = 2;
    public float maxZoom = 30;

    public float _currentZoom;

    [SerializeField] public Camera _camera;

    private Vector3 touchStart;


    void Update()
    {
        // Check for touch input (or mouse input for testing in editor)
        if (Input.GetMouseButtonDown(0)) // Left mouse button for testing, or Input.touchCount > 0 for actual touch
        {
            touchStart = Camera.main.ScreenToWorldPoint(Input.mousePosition); // Or Input.GetTouch(0).position
        }

        if (Input.GetMouseButton(0)) // Left mouse button held down, or Input.GetTouch(0).phase == TouchPhase.Moved
        {
            Vector3 direction = touchStart - Camera.main.ScreenToWorldPoint(Input.mousePosition); // Calculate movement direction
            transform.position += direction * panSpeed; // Apply movement to camera
        }

        // Zoom
        _currentZoom = Mathf.Clamp(_currentZoom - Input.mouseScrollDelta.y * zoomSpeed * Time.deltaTime, minZoom, maxZoom);
        _camera.orthographicSize = Mathf.Lerp(_camera.orthographicSize, _currentZoom, zoomSmoothness * Time.deltaTime);

    }
}