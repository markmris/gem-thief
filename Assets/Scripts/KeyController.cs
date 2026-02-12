using UnityEngine;

public class KeyController : MonoBehaviour
{
    public GameObject connectedDoor;

    void OnCollisionEnter2D(Collision2D collisionObj)
    {
        if (collisionObj.transform.CompareTag("Player"))
        {
            Destroy(connectedDoor);
            Destroy(gameObject);
        }
    }
}
