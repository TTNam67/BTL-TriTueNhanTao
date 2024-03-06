using UnityEngine;

public class ImageMove : MonoBehaviour
{
    private float speed = 7.0f; // Speed of movement
    private float movementRange = 75.0f; // How far the image will move up and down

    private Vector3 startingPosition;

    void Start()
    {
        startingPosition = transform.position; // Store the image's starting position
    }

    void Update()
    {
        float movement = Mathf.Sin(Time.time * speed) * movementRange; // Use sine function for smooth movement
        transform.position = startingPosition + new Vector3(0, movement, 0); // Update y position based on movement
    }
}