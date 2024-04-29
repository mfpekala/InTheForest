using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AntSpawner : MonoBehaviour
{
    public bool DEBUG = false; // Flag for debugging
    public GameObject antPrefab; // The specific Ant object/prefab to use
    public Vector2 batchRateRange; // Pair of floats representing batch rate range
    public Vector2Int batchCountRange; // Pair of integers representing batch count range
    public List<Vector2> points = new List<Vector2>(); // List of 2D points
    public float smudgeRadius; // Single float representing smudge radius
    public Vector2 antSpeedRange; // Pair of floats representing ant speed range

    private void Start()
    {
        StartCoroutine(SpawnAnts());
    }

    private void OnDrawGizmos()
    {
        if (DEBUG)
        {
            Gizmos.color = Color.white;
            for (int i = 0; i < points.Count - 1; i++)
            {
                Gizmos.DrawLine(points[i], points[i + 1]);
            }

            foreach (Vector2 point in points)
            {
                Gizmos.DrawSphere(point, 0.4f);
            }
        }
    }

    private IEnumerator SpawnAnts()
    {
        while (true)
        {
            yield return new WaitForSeconds(Random.Range(batchRateRange.x, batchRateRange.y));

            int numAnts = Random.Range(batchCountRange.x, batchCountRange.y);

            for (int i = 0; i < numAnts; i++)
            {
                List<Vector2> randomizedPoints = new List<Vector2>(points);

                for (int j = 0; j < randomizedPoints.Count; j++)
                {
                    randomizedPoints[j] += new Vector2(Random.Range(-smudgeRadius, smudgeRadius), Random.Range(-smudgeRadius, smudgeRadius));
                }

                float antSpeed = Random.Range(antSpeedRange.x, antSpeedRange.y);

                GameObject newAnt = Instantiate(antPrefab, randomizedPoints[0], Quaternion.identity);
                Ant antProps = newAnt.GetComponent<Ant>();
                antProps.points = randomizedPoints;
                antProps.moveSpeed = antSpeed;
                antProps.stoppingDistance = 0.15f;
            }
        }
    }
}