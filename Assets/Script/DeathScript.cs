using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;
using System.Diagnostics; // Required for Process
using System.IO; // Required for Path


public class DeathScript : MonoBehaviour
{
    public UIDocument uiDeathScreen;
    public AudioSource deathMusic;
    public AudioSource victoryMusic;

    private Label lblTitle;
    private Label lblDuration;
    private Label lblKills;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        var startbutton = uiDeathScreen.rootVisualElement.Q<Button>("MainMenuButton");
        startbutton.clicked += toMainMenu;

        var exitbutton = uiDeathScreen.rootVisualElement.Q<Button>("RestartButton");
        exitbutton.clicked += restartGame;

        PlayerMouseView.ignoreMouse = true;
        Time.timeScale = 0;
        uiDeathScreen.rootVisualElement.style.display = DisplayStyle.Flex;
        UnityEngine.Cursor.lockState = CursorLockMode.None;
        UnityEngine.Cursor.visible = true;

        lblKills = uiDeathScreen.rootVisualElement.Q<Label>("KillsValue");
        lblDuration = uiDeathScreen.rootVisualElement.Q<Label>("DurationValue");
        lblTitle = uiDeathScreen.rootVisualElement.Q<Label>("Title");

        int gameDuration = Timer.remainingMin * 60 + Timer.remainingSec;
        int leftDuration = Timer.GameLength*60 - gameDuration;
        string playTime = (Mathf.FloorToInt(leftDuration/60)).ToString() + ":" + (Mathf.FloorToInt(leftDuration % 60)).ToString();

        lblTitle.text = GameManager.title;
        lblDuration.text = playTime;
        lblKills.text = Mathf.Max(int.Parse(GameManager.kills), HealthSliderManager.kills).ToString();
        HealthSliderManager.kills = 0;

        switch (GameManager.Instance.gameState)
        {
            case GameManager.State.Win:
                victoryMusic.Play();
                break;
            case GameManager.State.Lose:
                deathMusic.Play();
                break;
        }
    }

    public void restartGame()
    {
        Application.Quit();
    }

    public void toMainMenu()
    {
        //SceneManager.LoadScene("MainMenu");
        string path = Path.Combine(Application.dataPath, "../Junkyard Shift.exe");
        // Start a new instance
        Process.Start(path);

        // Close this instance
        Application.Quit();

    }
}
