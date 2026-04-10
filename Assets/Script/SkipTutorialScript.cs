using UnityEngine;
using UnityEngine.SceneManagement;

public class SkipTutorialScript : MonoBehaviour
{
    public void skip()
    {
        SceneManager.LoadScene("MainGame");
    }
}
