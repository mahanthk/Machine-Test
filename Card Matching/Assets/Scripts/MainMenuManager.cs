using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class MainMenuManager : MonoBehaviour
{
    [SerializeField]
    private GameObject instructionsPanel;
    [SerializeField]
    private GameObject mainScreen;

    public void LoadGame()
    {
        SceneManager.LoadScene("Game");
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void ViewInstructionPanel()
    {
        mainScreen.SetActive(false);
        instructionsPanel.SetActive(true);
    }

    public void CloseInstructionPlanel()
    {
        instructionsPanel.SetActive(false);
        mainScreen.SetActive(true);
    }
}
