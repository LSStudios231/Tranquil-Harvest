using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreeController : MonoBehaviour
{
    public void ChopDownTree()
    {
        // Add logic to handle the tree being chopped down, e.g., playing an animation, destroying the tree object, etc.
        Debug.Log("Tree chopped down!");
        Destroy(gameObject); // Destroys the tree object
    }
}
