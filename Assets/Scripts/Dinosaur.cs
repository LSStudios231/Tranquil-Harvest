using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dinosaur : MonoBehaviour
{
    public float moveSpeed = 2f; // Speed of the NPC
    public float minMoveTime = 1f; // Minimum time the NPC will move in one direction
    public float maxMoveTime = 3f; // Maximum time the NPC will move in one direction
    public float minPauseTime = 0.5f; // Minimum pause time between movements
    public float maxPauseTime = 2f; // Maximum pause time between movements

    private float moveTime; // The amount of time the NPC will move in the current direction
    private float moveTimer; // Timer to track how long the NPC has been moving in the current direction
    private float pauseTime; // The amount of time the NPC will pause before moving again
    private bool isPaused; // Is the NPC currently paused?
    private Vector2 moveDirection; // The direction the NPC is currently moving

    private Animator animator; // Reference to the Animator component

    private void Start()
    {
        animator = GetComponent<Animator>(); // Get the Animator component
        SetRandomDirection();
    }

    private void Update()
    {
        if (isPaused)
        {
            // If paused, decrement the timer and check if it's time to move again
            pauseTime -= Time.deltaTime;
            if (pauseTime <= 0f)
            {
                isPaused = false;
                SetRandomDirection();
            }
        }
        else
        {
            // Move the NPC in the current direction
            transform.Translate(moveDirection * moveSpeed * Time.deltaTime);

            // Update the move timer
            moveTimer += Time.deltaTime;

            // If the move timer exceeds the move time, pause movement
            if (moveTimer >= moveTime)
            {
                StartPause();
            }
        }

        // Update animation parameter
        animator.SetBool("isMoving", !isPaused);
    }

    private void SetRandomDirection()
    {
        // Choose a random direction (up, down, left, right)
        int direction = Random.Range(0, 4);
        switch (direction)
        {
            case 0:
                moveDirection = Vector2.up;
                break;
            case 1:
                moveDirection = Vector2.down;
                break;
            case 2:
                moveDirection = Vector2.left;
                break;
            case 3:
                moveDirection = Vector2.right;
                break;
        }

        // Choose a random move time between the minimum and maximum
        moveTime = Random.Range(minMoveTime, maxMoveTime);

        // Reset the move timer
        moveTimer = 0f;
    }

    private void StartPause()
    {
        // Set the NPC to be paused and choose a random pause duration
        isPaused = true;
        pauseTime = Random.Range(minPauseTime, maxPauseTime);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // On collision, start a pause and change direction
        StartPause();
        SetRandomDirection();
    }
}
