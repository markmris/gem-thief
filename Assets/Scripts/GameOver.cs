using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOver : MonoBehaviour
{
    public void EndGame()
    {
        StartCoroutine(LoadMainMenu());
    }

    IEnumerator LoadMainMenu()
    {
        yield return new WaitForSeconds(2);

        SceneManager.LoadScene("MainMenu");
    }
}
