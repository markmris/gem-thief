using UnityEngine;

public class ShadeController : MonoBehaviour
{
    private PlayerController playerController;
    
    void Start()
    {
        playerController = GameObject.Find("Player").GetComponent<PlayerController>();
    }

    void OnTriggerEnter2D(Collider2D collisionObj)
    {
        if (collisionObj.transform.CompareTag("Player"))
        {
            playerController.visible = false;
        }
    }

    void OnTriggerExit2D(Collider2D collisionObj)
    {
        if (collisionObj.transform.CompareTag("Player"))
        {
            playerController.visible = true;
        }
    }
}
