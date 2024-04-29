using System.Collections.Generic;
using UnityEngine;

public class Ant : MonoBehaviour
{
    public List<Vector2> points;
    public float moveSpeed = 5f;
    public float stoppingDistance = 5f;

    private int currentPointIndex = 0;

    void Update()
    {
        if (points.Count == 0)
        {
            Laws.Instance.health -= 1;
            if (Laws.Instance.health <= 0)
            {
                Application.Quit();
            }
            Destroy(gameObject);
            return;
        }

        Vector2 targetPoint = points[currentPointIndex];
        Vector2 moveDirection = (targetPoint - (Vector2)transform.position).normalized;

        transform.position += new Vector3(moveDirection.x * Time.deltaTime * moveSpeed, moveDirection.y * Time.deltaTime * moveSpeed, 0.0f);

        float angle = Mathf.Atan2(moveDirection.y, moveDirection.x) * Mathf.Rad2Deg - 90.0f;
        transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);

        float distanceToTarget = Vector2.Distance(transform.position, targetPoint);
        if (distanceToTarget <= stoppingDistance)
        {
            points.RemoveAt(currentPointIndex);
            if (points.Count == 0)
                return;

            currentPointIndex = 0;
        }
    }
}
