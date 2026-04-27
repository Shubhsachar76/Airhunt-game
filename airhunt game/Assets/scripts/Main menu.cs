using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void StartGame()
    {
        SceneManager.LoadScene("Training"); 
    }

    public void ExitGame()
    {
        Application.Quit();
    }
}