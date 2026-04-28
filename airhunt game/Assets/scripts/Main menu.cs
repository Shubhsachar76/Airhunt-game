using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void Training()
    {
        SceneManager.LoadScene("Training"); 
    }
    public void Playgame()
    {
        SceneManager.LoadScene("GAME"); 
    }
    public void ExitGame()
    {
        Application.Quit();
    }
}