using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WanderingSprite : MonoBehaviour
{
    public Vector2 movementSpeed = new Vector2(5f, 5f); // Speed in x and y directions
    public float rotationSpeed = 100f; // Rotation speed in degrees per second

    private Vector2 direction;

    private void Start()
    {
        // Set a random initial direction
        direction = Random.insideUnitCircle.normalized;
    }

    private void Update()
    {
        // Move the sprite
        Vector3 movement = new Vector3(direction.x, direction.y, 0) * Time.deltaTime * movementSpeed.magnitude;
        transform.position += movement;

        // Rotate the sprite
        transform.Rotate(Vector3.forward * rotationSpeed * Time.deltaTime);

        // Check for collisions with screen boundaries
        CheckBounds();
    }

    private void CheckBounds()
    {
        // Get the screen bounds in world space
        Vector3 spritePosition = Camera.main.WorldToViewportPoint(transform.position);

        // Reverse direction if hitting screen boundaries
        if (spritePosition.x <= 0f || spritePosition.x >= 1f)
        {
            direction.x = -direction.x;
        }
        if (spritePosition.y <= 0f || spritePosition.y >= 1f)
        {
            direction.y = -direction.y;
        }

        // Keep the sprite within the screen bounds
        spritePosition.x = Mathf.Clamp01(spritePosition.x);
        spritePosition.y = Mathf.Clamp01(spritePosition.y);
        transform.position = Camera.main.ViewportToWorldPoint(spritePosition);
    }
}
