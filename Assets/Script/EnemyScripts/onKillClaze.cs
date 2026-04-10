using UnityEngine;

public class onKillClaze : MonoBehaviour
{
    public GameObject enemyPrefab;

    public void OnEnemyDeath()
    {
        GameObject prefab = enemyPrefab;
        Instantiate(prefab, transform.position, transform.rotation);
    }
}