using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraScript : MonoBehaviour
{
    private WaterMapGenerator waterMapGenerator;

    [SerializeField]
    private Camera cam;
    private float targetZoom;
    public float zoomFactor;
    public float zoomSize;
    public float ZoomMin;
    public float Speed;

    private Vector3 dragOrigin;
    private Vector3 targetPosition;
    private float mapMinY, mapMaxY, mapMinX, mapMaxX;
    private MeshRenderer mapRenderer;
    private GameObject mapObjectA;
    private float mapWidth;

    private void Start()
    {
        waterMapGenerator = FindAnyObjectByType<WaterMapGenerator>();
        cam = Camera.main;
        targetZoom = cam.orthographicSize;

        // Find the sprite renderer in the scene and set the camera bounds based on its size
        mapRenderer = FindObjectOfType<MeshRenderer>();
        mapMinY = mapRenderer.transform.position.y - mapRenderer.bounds.size.y / 2f;
        mapMaxY = mapRenderer.transform.position.y + mapRenderer.bounds.size.y / 2f;
        mapMinX = mapRenderer.transform.position.x - mapRenderer.bounds.size.x / 2f;
        mapMaxX = mapRenderer.transform.position.x + mapRenderer.bounds.size.x / 2f;
        mapObjectA = mapRenderer.gameObject;
        mapWidth = mapRenderer.bounds.size.x;
    }

    private void Update()
    {
        PanCamera();
        ScrollZoom();
        ClampCameraToMap();
    }

    private void PanCamera()
    {
        if (Input.GetKeyDown(KeyCode.Mouse2))
        {
            dragOrigin = cam.ScreenToWorldPoint(Input.mousePosition);
        }

        if (Input.GetKey(KeyCode.Mouse2))
        {
            Vector3 difference = dragOrigin - cam.ScreenToWorldPoint(Input.mousePosition);
            targetPosition = cam.transform.position + difference;
            cam.transform.position = ClampCamera(targetPosition);
        }
    }

    private Vector3 ClampCamera(Vector3 targetPosition)
    {
        float camHeight = cam.orthographicSize;
        float camWidth = cam.orthographicSize * cam.aspect;

        float minY = mapMinY + camHeight;
        float maxY = mapMaxY - camHeight;
        float minX = mapMinX + camWidth;
        float maxX = mapMaxX - camWidth;

        float newY = Mathf.Clamp(targetPosition.y, minY, maxY);
        float newX = Mathf.Clamp(targetPosition.x, minX, maxX);

        return new Vector3(newX, newY, targetPosition.z);
    }

    private void ScrollZoom()
    {
        float scrollData = Input.GetAxis("Mouse ScrollWheel");

        // Get the mouse position in world space
        Vector3 mouseWorldPositionBeforeZoom = cam.ScreenToWorldPoint(Input.mousePosition);
        mouseWorldPositionBeforeZoom.z = 0; // Ensure the z-coordinate is zero for 2D

        // Calculate the target zoom level
        targetZoom -= scrollData * zoomFactor;
        targetZoom = Mathf.Clamp(targetZoom, ZoomMin, zoomSize);

        // Update the camera's orthographic size
        cam.orthographicSize = Mathf.Lerp(cam.orthographicSize, targetZoom, Time.deltaTime * Speed);

        // Get the mouse position in world space after zoom
        Vector3 mouseWorldPositionAfterZoom = cam.ScreenToWorldPoint(Input.mousePosition);
        mouseWorldPositionAfterZoom.z = 0; // Ensure the z-coordinate is zero for 2D

        // Calculate the difference in camera position needed to keep the mouse world position the same
        Vector3 positionOffset = mouseWorldPositionBeforeZoom - mouseWorldPositionAfterZoom;
        cam.transform.position += positionOffset;

        // Clamp the camera position to stay within bounds
        cam.transform.position = ClampCamera(cam.transform.position);
    }

    private void ClampCameraToMap()
    {
        cam.transform.position = ClampCamera(cam.transform.position);
    }
}
