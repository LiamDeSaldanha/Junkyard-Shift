using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class projectileManager : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        isRunning = false;
    }

    private List<GameObject> projectiles = new List<GameObject>();
    private List<float> entryTime = new List<float>();

    private float maxLifeTime = 30.0f;
    private bool isRunning = false;

    // Update is called once per frame
    void FixedUpdate() { 
        if (!isRunning) {
            Debug.Log("start cleaning projectiles");
            isRunning = true;
            StartCoroutine(removeOldProjectiles());
        }
    }

    IEnumerator removeOldProjectiles() {
        foreach (Transform proj in transform) {
            if (!projectiles.Contains(proj.gameObject)) {
                Debug.Log("found new projectile");
                projectiles.Add(proj.gameObject);
                entryTime.Add(Time.time);
            }
        }

        int size = projectiles.Count;

        if (size > 0) {
            Debug.Log("looking for old projectiles");
            for (int i = size - 1; i >= 0; i--) {
                if (Time.time - entryTime[i] > maxLifeTime) {
                    Debug.Log("Destroying projectiles");
                    Destroy(projectiles[i]);
                    projectiles.RemoveAt(i);
                    entryTime.RemoveAt(i);
                }
            }
        }

        float elapsedTime = 0;
        while (elapsedTime < maxLifeTime/2) {
            elapsedTime += Time.deltaTime;
            yield return null; // Wait for next frame
        }

        Debug.Log("stop cleaning projectiles");
        isRunning = false;
    }
}
