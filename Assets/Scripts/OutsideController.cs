using TMPro;
using UnityEngine;

public class OutsideController : MonoBehaviour
{
    public GameOver gameOver;
    public TextMeshProUGUI endText;
    public SpriteRenderer displayCase;
    public Sprite displayCaseEmpty;

    void OnCollisionEnter2D(Collision2D collisionObj)
    {
        if (collisionObj.transform.CompareTag("Player") && displayCase.sprite == displayCaseEmpty)
        {
            endText.gameObject.SetActive(true);
            gameOver.EndGame();
        }
    }
}
