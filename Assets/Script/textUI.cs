using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class textUI : MonoBehaviour
{
    public UIDocument uiText;
    public float elapsedTime;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        showTextUI(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void showTextUI(bool state) {
        if (state)
        {
            uiText.rootVisualElement.style.display = DisplayStyle.Flex;
        }
        else {
            uiText.rootVisualElement.style.display = DisplayStyle.None;
        }
    }

    public void showTextUI(string text, float elapsedTime)
    {
        showTextUI(true);
        uiText.rootVisualElement.Q<Label>("TextLabel").text = text;
        StartCoroutine(closeAfterTime(elapsedTime));
    }

    public void showTextUI(string text)
    {
        showTextUI(true);
        uiText.rootVisualElement.Q<Label>("TextLabel").text = text;
    }

    IEnumerator closeAfterTime(float time) {
        float elapsedTime = 0;
        while (elapsedTime < time)
        {
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        showTextUI(false);
    }
}
