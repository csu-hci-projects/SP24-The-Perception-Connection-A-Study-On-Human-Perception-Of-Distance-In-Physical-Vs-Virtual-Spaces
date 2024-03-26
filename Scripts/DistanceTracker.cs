using UnityEngine;
using System.Text;

public class DistanceTracker : MonoBehaviour
{
    private bool isTracking = false;
    private Vector3 originalPosition;
    private float totalDistance = 0f;
    private StringBuilder distanceLog = new StringBuilder();
    private int distanceCount = 0;
    private const int MAX_DISTANCES = 15; // Maximum number of distances to log

    void Update()
    {
        // Check if the 'B' key is pressed
        if (Input.GetKeyDown(KeyCode.B))
        {
            // Toggle tracking mode
            isTracking = !isTracking;

            if (isTracking)
            {
                // Store the original position when tracking starts
                originalPosition = transform.position;
            }
            else
            {
                // Output the total distance traveled to the console
                Debug.Log($"Total distance traveled: {totalDistance:F2} meters");

                // Output the logged distances in CSV format if the maximum count is reached
                if (distanceCount >= MAX_DISTANCES)
                {
                    Debug.Log($"Logged distances (CSV format):\n{distanceLog}");
                    distanceLog.Clear();
                    distanceCount = 0;
                }

                totalDistance = 0f;
            }
        }

        // If tracking, update the total distance traveled
        if (isTracking)
        {
            UpdateTotalDistance();
        }
    }

    void UpdateTotalDistance()
    {
        // Calculate the distance traveled since the last frame
        float distance = Vector3.Distance(transform.position, originalPosition);

        // Add the distance to the total
        totalDistance += distance;

        // Log the distance in CSV format
        distanceLog.Append($"{distance:F2},");
        distanceCount++;

        // Reset the original position for the next frame
        originalPosition = transform.position;
    }
}