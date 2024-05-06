using UnityEngine;

public class PathFollower : MonoBehaviour
{
   // private float[] path = {3f, 5f, 2f, 1f, 4f, 5f, 4f, 2f, 3f, 1f, 5f, 1f, 3f, 4f, 2f}; // Define the path as an array of floats
    private float[] path = {118.11f, 196.85f, 78.7402f, 39.3701f, 157.48f, 196.85f, 157.48f, 78.7402f, 118.11f, 39.3701f, 196.85f, 39.3701f, 118.11f, 157.48f, 78.7402f};
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
        Debug.Log($"{targetDistance/39.37} meters");

        // Calculate the direction from the origin to the target distance
        // Vector3 direction = transform.position.z.normalized * targetDistance;
        Vector3 direction =  new Vector3(transform.position.x, transform.position.y, targetDistance-(2*39.37f));

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