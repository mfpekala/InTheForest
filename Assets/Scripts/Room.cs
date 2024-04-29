using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Room : MonoBehaviour
{
    private float roomWidth = 20.0f;
    private float roomHeight = 10.0f;
    public int currentRoom = 0;
    public List<Vector2> roomCenters = new List<Vector2>();

    // Flag to control whether to draw room locations or not
    public bool DEBUG = true;

    void Start()
    {
        roomCenters.Add(new Vector2(0.0f, 5.0f));
        roomCenters.Add(new Vector2(-39.1f, 30.1f));
        roomCenters.Add(new Vector2(-1.2f, 44.3f));
        roomCenters.Add(new Vector2(-9.99f, 66.1f));
        roomCenters.Add(new Vector2(-12.99f, 97.7f));
    }

    private int GetRoom(Vector2 pos)
    {
        int closestRoomIndex = 0;
        float closestDistance = Mathf.Infinity;

        for (int i = 0; i < roomCenters.Count; i++)
        {
            // Calculate the distance between the current room center and the given position
            float distance = Vector2.Distance(roomCenters[i], pos);

            // Check if the current room center is closer than the previous closest one
            if (distance < closestDistance)
            {
                closestRoomIndex = i;
                closestDistance = distance;
            }
        }

        // Return the index of the closest room center
        return closestRoomIndex;
    }

    public void Update()
    {
        currentRoom = GetRoom(new Vector2(transform.position.x, transform.position.y));
    }

    public Vector2 RandomPoint()
    {
        float x = Random.Range(-roomWidth / 2.0f, roomWidth / 2.0f);
        float y = Random.Range(-roomHeight / 2.0f, roomHeight / 2.0f);
        Vector2 center = roomCenters[currentRoom];
        return new Vector2(center.x + x, center.y + y);
    }

    public bool ContainsPoint(Vector2 pos)
    {
        Vector2 center = roomCenters[currentRoom];
        float halfWidth = roomWidth / 2.0f;
        float halfHeight = roomHeight / 2.0f;

        // Check if pos.x is within the x-boundaries of the room
        bool withinXBounds = pos.x >= center.x - halfWidth && pos.x <= center.x + halfWidth;

        // Check if pos.y is within the y-boundaries of the room
        bool withinYBounds = pos.y >= center.y - halfHeight && pos.y <= center.y + halfHeight;

        // Return true if pos is within both x and y boundaries, otherwise return false
        return withinXBounds && withinYBounds;
    }

    // // OnDrawGizmos is called when the script is loaded or a value is changed in the inspector
    // private void OnDrawGizmos()
    // {
    //     if (DEBUG)
    //     {
    //         // Draw room locations
    //         Gizmos.color = Color.blue;
    //         foreach (Vector2 center in roomCenters)
    //         {
    //             Gizmos.DrawWireCube(center, new Vector3(roomWidth, roomHeight, 0));
    //         }
    //     }
    // }
}
