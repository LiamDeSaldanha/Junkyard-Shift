using UnityEngine;
using UnityEngine.UIElements;

public class Timer : MonoBehaviour
{
    public UIDocument uiTimer;
    public static float startTime = 1;
    public static int remainingTime = 0;
    public static int GameLength = 20; // minutes
    public static int remainingMin = 1;
    public static int remainingSec = 1;
    public static float elpased = 0;
    public static bool stop;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        startTime = Time.realtimeSinceStartup;
    }

    // Update is called once per frame
    void Update()
    {
        if (!stop)
        {
            elpased += Time.deltaTime;
            float timeSinceStart = elpased;

            remainingTime = GameLength * 60 - Mathf.FloorToInt(timeSinceStart);
            remainingMin = remainingTime / 60;
            remainingSec = remainingTime % 60;

            uiTimer.rootVisualElement.Q<Label>("Timer").text = remainingMin + ":" + remainingSec;
        }
        
    }
}
