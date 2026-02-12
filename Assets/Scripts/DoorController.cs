using System.Collections;
using UnityEngine;

public class DoorController : MonoBehaviour
{
    private GameObject doorTextLabel;

    void Start()
    {
        doorTextLabel = transform.Find("Text").gameObject;
    }

    void OnCollisionEnter2D(Collision2D collisionObj)
    {
        if (collisionObj.transform.CompareTag("Player"))
        {
            StartCoroutine(ShowText());
        }
    }

    private IEnumerator ShowText()
    {
        doorTextLabel.SetActive(true);
        yield return new WaitForSeconds(4f);
        doorTextLabel.SetActive(false);
    }
}
