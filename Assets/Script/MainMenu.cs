using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public UIDocument uiMainMenu;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        var startbutton = uiMainMenu.rootVisualElement.Q<Button>("StartGame");
        startbutton.clicked += startGame;

        var exitbutton = uiMainMenu.rootVisualElement.Q<Button>("ExitGame");
        exitbutton.clicked += endGame;

    }

    public void startGame()
    {
        Debug.Log("Starting Game");
        SceneManager.LoadScene("Introduction");
    }

    // copied off the internet
    public void endGame() {
        Application.Quit();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
