using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CameraMover : MonoBehaviour
{
    public Room room;
    public InputController inputController;
    public float moveSpeed = 5f; // Adjust this to control the speed of movement
    public float zoomSpeed = 1f; // Adjust this to control the speed of zooming
    public float minZoom = 5f; // Minimum zoom level
    public float maxZoom = 10f; // Maximum zoom level

    private bool isMoving = false; // Flag to prevent changing the goal while moving
    private Vector3 hackPlacement = new Vector3(-13.1f, 50.7f, -10.0f);
    private float hackZoom = 22.7f;

    void Start()
    {
        MoveToGoal();
    }

    public void Update()
    {
        if (Laws.Instance.inMenu)
        {
            transform.position = hackPlacement;
            Camera.main.orthographicSize = hackZoom;
            return;
        }
        // Check if not moving and user input is allowed
        if (!isMoving)
        {
            MoveToGoal();
        }
    }

    private void MoveToGoal()
    {
        if (Vector2.Distance(transform.position, room.roomCenters[Laws.Instance.currentRoom]) < 0.001f)
        {
            return; // If so, do nothing
        }

        StartCoroutine(MoveCoroutine(room.roomCenters[Laws.Instance.currentRoom]));
    }

    private IEnumerator MoveCoroutine(Vector2 goalPos)
    {
        isMoving = true;
        // TODO: fix this
        // inputController.hidden = true;

        // Zoom out
        yield return ZoomCoroutine(maxZoom, zoomSpeed);

        // Move towards goalPos
        Vector2 startPos = new Vector2(transform.position.x, transform.position.y);
        float startTime = Time.time;
        float journeyLength = Vector2.Distance(startPos, goalPos);
        while (Vector2.Distance(new Vector2(transform.position.x, transform.position.y), goalPos) > 0.001f)
        {
            float distCovered = (Time.time - startTime) * moveSpeed;
            float fracJourney = distCovered / journeyLength;
            Vector2 newPos = Vector2.Lerp(startPos, goalPos, Mathf.SmoothStep(0f, 1f, fracJourney));
            transform.position = new Vector3(newPos.x, newPos.y, transform.position.z); // Keep the z-axis unchanged
            yield return null;
        }

        // Zoom in
        yield return ZoomCoroutine(minZoom, zoomSpeed);

        inputController.hidden = false;
        isMoving = false;
    }

    private IEnumerator ZoomCoroutine(float targetZoom, float speed)
    {
        float t = 0f;
        while (t < 0.1f)
        {
            t += Time.deltaTime * speed;
            // Vector2 newPos = Vector2.Lerp(from, to, t);
            // transform.position = new Vector3(newPos.x, newPos.y, transform.position.z); // Keep the z-axis unchanged
            Camera.main.orthographicSize = Mathf.Lerp(Camera.main.orthographicSize, targetZoom, t);
            yield return null;
        }
    }
}
