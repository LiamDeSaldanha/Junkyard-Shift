using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    public enum State { Win, Lose, Continue }
    public GameObject Player;
    public GameObject Base;
    public Slider playerHealth;
    public Slider baseHealth;
    public GameObject boss;
    public State gameState;
    public int enemiesKilled = 0;

    public static string title = "";
    public static string duration = "";
    public static string kills = "";

    public void triggerAwake()
    {
        Awake();
    }

    public static void newInstance()
    {
        Instance = null;
    }

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    void Start()
    {

        playerHealth = Player.GetComponentInChildren<Slider>();
        gameState = State.Continue;
        if (Base != null)
        {
            baseHealth = Base.GetComponentInChildren<Slider>();

        }

        boss = GameObject.Find("Boss");
    }


    public void Win()
    {
        gameState = State.Win;
        duration = Timer.remainingMin.ToString() + ":" + Timer.remainingSec.ToString();
        kills = enemiesKilled.ToString();
    }

    public void YouDied()
    {
        gameState = State.Lose;
        duration = Timer.remainingMin.ToString() + ":" + Timer.remainingSec.ToString();
        kills = enemiesKilled.ToString();
    }


    public void restartPlayer()
    {
        Player.GetComponentInChildren<HealthSliderManager>().Heal(20);
        OxygenManager.Instance.player_oxygen.value = OxygenManager.Instance.player_oxygen.maxValue;
        OxygenManager.Instance.equippedFilterTier = 0;
        Timer.stop = false;
        Timer.startTime = Time.realtimeSinceStartup;
        Timer.elpased = 0;
        Timer.GameLength = 20;
        ResourceManager.Instance.resources = new List<RecipeIngredients>();
        UnlockedWeaponManager.clearWeapons();

        // find player and set weapons inactive

    }
    public void restartBase()
    {
        if (Base != null)
        {
            Base.GetComponentInChildren<HealthSliderManager>().Heal(20);

        }
    }


    public void sendPlayerToBoss()
    {
        OxygenManager.Instance.player_oxygen.value = OxygenManager.Instance.player_oxygen.maxValue;
        Timer.stop = true;
    }

    void Update()
    {

        if (gameState != State.Continue) {
            return;
        }
        
        if (baseHealth != null)
        {
            if (baseHealth.value <= 0)
            {
                Timer.stop = true;
                title = "Your base is destroyed";
                YouDied();
                SceneManager.LoadScene("DeathScreen");
            }
        }

        if (playerHealth.value <= 0)
        {
            Timer.stop = true;
            title = "You Died";
            YouDied();
            SceneManager.LoadScene("DeathScreen");
        }
        if (Timer.remainingMin <= 0 && Timer.remainingSec <= 0)
        {

            Timer.stop = true;
            UnityEngine.Debug.Log("Timer finished");
            title = "You Survived!";
            Win();
            SceneManager.LoadScene("DeathScreen");
        }

        string currentScene = SceneManager.GetActiveScene().name;

        if (currentScene == "Boss Chamber")
        {
            boss = GameObject.Find("Boss");
            if (boss == null)
            {
                Timer.stop = true;
                UnityEngine.Debug.Log("Boss Killed");
                title = "You Won!";
                Win();
                SceneManager.LoadScene("DeathScreen");
            }
        }
    }
}