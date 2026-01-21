using UnityEngine;

public class CollisionDetection : MonoBehaviour
{
    void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log("Collided with: " + collision.gameObject.name);
    }

    void OnCollisionStay2D(Collision2D collision)
    {
        Debug.Log("Still colliding with: " + collision.gameObject.name);
    }

    void OnCollisionExit2D(Collision2D collision)
    {
        Debug.Log("Stopped colliding with: " + collision.gameObject.name);
    }
}
