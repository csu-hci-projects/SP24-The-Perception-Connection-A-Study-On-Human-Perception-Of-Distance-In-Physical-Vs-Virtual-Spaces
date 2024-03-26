using UnityEngine;

public class PathFollower : MonoBehaviour
{
    private float[] path = { 3f, 5f, 2f, 1f, 4f, 5f, 2f, 3f, 1f, 5f, 1f, 3f, 4f, 2f }; // Define the path as an array of floats
    private int currentIndex = 0; // Keep track of the current index in the path
    private bool pathCompleted = false; // Flag to track if the path has been completed

    void Update()
    {
        // Check if the 'M' key is pressed
        if (Input.GetKeyDown(KeyCode.M))
        {
            MoveToNextDistance();
        }
    }

    void MoveToNextDistance()
    {
        // Get the next distance from the path
        float targetDistance = path[currentIndex];
        Debug.Log($"{targetDistance} meters");

        // Calculate the direction from the origin to the target distance
        Vector3 direction = transform.position.normalized * targetDistance;

        // Set the new position of the cube
        transform.position = direction;

        // Update the current index for the next distance in the path
        currentIndex = (currentIndex + 1) % path.Length;

        // Check if we've completed the path
        if (currentIndex == 0 && !pathCompleted)
        {
            Debug.Log("Path complete");
            pathCompleted = true;
        }
    }
}