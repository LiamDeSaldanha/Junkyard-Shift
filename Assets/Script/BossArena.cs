using UnityEngine;
using UnityEngine.SceneManagement;

public class BossArena : MonoBehaviour
{
    private void Awake()
    {
        SceneManager.sceneLoaded += resetPlayerInBossRoom;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (OxygenManager.Instance.equippedFilterTier == 3)
            {
                SceneManager.LoadScene("Boss Chamber");
                GameManager.Instance.sendPlayerToBoss();
                //GameObject.Find("Player").transform.position = new Vector3(12.5f, 10, 82);
            }

        }
        
    }

    static void resetPlayerInBossRoom(Scene scene, LoadSceneMode mode) {
        GameObject objPlayer = GameObject.Find("Player");
        if (scene.name == "Boss Chamber") {
            objPlayer.transform.position = new Vector3(12.5f, 10, 172);
            UnlockedWeaponManager.isBazookacrafted = true;
        } else if (scene.name == "MainGame") {
            objPlayer.transform.position = new Vector3(25, 2, -25);
        }

    }

}
