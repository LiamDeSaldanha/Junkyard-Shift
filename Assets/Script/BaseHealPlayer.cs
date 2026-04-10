using System.Collections;
using UnityEngine;

public class BaseHealPlayer : MonoBehaviour
{

    public static bool timerRunning;
    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (other != null)
            {
                if (!timerRunning)
                {
                    timerRunning = true;
                    other.gameObject.GetComponentInChildren<HealthSliderManager>().Heal(5);
                    StartCoroutine(timerWait());
                }

            }
        }
    }


    private IEnumerator timerWait()
    {
        yield return new WaitForSeconds(2);
        timerRunning = false;
    }
}
