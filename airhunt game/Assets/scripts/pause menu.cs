using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class PauseMenu : MonoBehaviour
{
    public GameObject pausePanel;
    public Button invertXButton;
    public Button invertYButton;
    public TextMeshProUGUI invertXText;
    public TextMeshProUGUI invertYText;

    // colors
    private Color onColor = new Color(0.2f, 0.8f, 0.2f);   // green = ON
    private Color offColor = new Color(0.8f, 0.2f, 0.2f);  // red = OFF

    private bool isPaused = false;
    private CameraFromPython cam;

    void Start()
    {
        cam = FindObjectOfType<CameraFromPython>();
        pausePanel.SetActive(false);

        invertXButton.onClick.AddListener(ToggleInvertX);
        invertYButton.onClick.AddListener(ToggleInvertY);

        UpdateButtons();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
            TogglePause();
    }

    void ToggleInvertX()
    {
        cam.invertX = !cam.invertX;
        UpdateButtons();
    }

    void ToggleInvertY()
    {
        cam.invertY = !cam.invertY;
        UpdateButtons();
    }

    void UpdateButtons()
    {
        // update text
        invertXText.text = "Invert X:" + (cam.invertX ? "ON" : "OFF");
        invertYText.text = "Invert Y:" + (cam.invertY ? "ON" : "OFF");

        // update color
        invertXButton.image.color = cam.invertX ? onColor : offColor;
        invertYButton.image.color = cam.invertY ? onColor : offColor;
    }

    public void TogglePause()
    {
        isPaused = !isPaused;
        pausePanel.SetActive(isPaused);
        Time.timeScale = isPaused ? 0f : 1f;
    }

    public void ResumeGame()
    {
        isPaused = false;
        pausePanel.SetActive(false);
        Time.timeScale = 1f;
    }

    public void GoToMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("MainMenu");
    }
    public void ExitGame()
{
    Time.timeScale = 1f;
    Application.Quit();
}
}