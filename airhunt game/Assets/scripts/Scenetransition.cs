using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class SceneTransition : MonoBehaviour
{
    public string nextSceneName;
    public CanvasGroup flashCanvas;   // Assign black UI image with CanvasGroup
    public float flashSpeed = 0.08f;
    public int flashCount = 4;

    public void StartTransition()
    {
        StartCoroutine(FlashAndLoad());
    }

    IEnumerator FlashAndLoad()
    {
        for (int i = 0; i < flashCount; i++)
        {
            // Fade to black
            flashCanvas.alpha = 1;
            yield return new WaitForSeconds(flashSpeed);

            // Fade back
            flashCanvas.alpha = 0;
            yield return new WaitForSeconds(flashSpeed * 0.7f);

            flashSpeed *= 0.8f; // Faster each flash
        }

        // Final black hold
        flashCanvas.alpha = 1;
        yield return new WaitForSeconds(0.2f);

        SceneManager.LoadScene(nextSceneName);
    }
}