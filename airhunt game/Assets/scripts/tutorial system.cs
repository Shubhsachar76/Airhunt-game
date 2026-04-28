using UnityEngine;
using TMPro;

public class DialogueSystem : MonoBehaviour
{
    [Header("Dialogue")]
    public string[] dialogueLines;

    [Header("UI")]
    public TMP_Text dialogueText;

    [Header("Controls")]
    public KeyCode nextKey = KeyCode.Space;

    private int currentLine = 0;

    void Start()
    {
        ShowLine();
    }

    void Update()
    {
        if (Input.GetKeyDown(nextKey))
        {
            NextLine();
        }
    }

    public void NextLine()
    {
        currentLine++;

        if (currentLine >= dialogueLines.Length)
        {
            dialogueText.text = "";
            gameObject.SetActive(false);
            return;
        }

        ShowLine();
    }

    void ShowLine()
    {
        if (dialogueLines.Length > 0 && dialogueText != null)
        {
            dialogueText.text = dialogueLines[currentLine];
        }
    }
}
