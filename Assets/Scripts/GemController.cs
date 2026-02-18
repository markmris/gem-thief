using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GemController : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;
    public Sprite displayCaseEmpty;
    public Image gemImage;
    public TextMeshProUGUI taskText;
    public AudioSource sfx;

    void Start()
    {
        spriteRenderer = transform.GetComponent<SpriteRenderer>();
    }

    void OnCollisionEnter2D(Collision2D collisionObj)
    {
        if (collisionObj.transform.CompareTag("Player") && spriteRenderer.sprite != displayCaseEmpty)
        {
            spriteRenderer.sprite = displayCaseEmpty;
            StartCoroutine(ShowDiamond());
            // sfx.Play();
        }
    }

    IEnumerator ShowDiamond()
    {
        taskText.gameObject.SetActive(false);
        gemImage.gameObject.SetActive(true);
        yield return new WaitForSeconds(2);
        gemImage.gameObject.SetActive(false);
        taskText.gameObject.SetActive(true);
        taskText.text = "Task: Leave the museum";
    }
}
