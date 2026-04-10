using System.Collections;
using UnityEngine;

public class Enemy_spawning : MonoBehaviour
{
    private GameObject enemyPrefab;
    public float spawnInterval;
    public GameObject[] enemies;
    private GameObject tempEnemy;
    private int numberOfSpawnedEnemies;
    public float sizeOfSpawnArea;
    private bool timerExpired = false; 
    public bool hasEnemyLimit = true;
    private BoxCollider boxCollider;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        boxCollider = GetComponent<BoxCollider>();
        boxCollider.size = new Vector3(sizeOfSpawnArea,sizeOfSpawnArea,sizeOfSpawnArea);
        
    }

    // Update is called once per frame
    void Update()
    {

        if ((numberOfSpawnedEnemies < enemies.Length || !hasEnemyLimit)&& !timerExpired )
        {
            timerExpired = true;
            spawn();
            StartCoroutine(timer());


        }




    }
    
    


    private void spawn()
    {
        

            enemyPrefab = enemies[Random.Range(0, enemies.Length)];


            //Vector3 randomPos = new Vector3(Random.Range(-sizeOfSpawnArea, sizeOfSpawnArea), 0f, Random.Range(-sizeOfSpawnArea, sizeOfSpawnArea));

        Vector3 randomOffset = new Vector3(
    Random.Range(-sizeOfSpawnArea, sizeOfSpawnArea),
    0f,
    Random.Range(-sizeOfSpawnArea, sizeOfSpawnArea)
);

        tempEnemy = Instantiate(enemyPrefab, transform.position + randomOffset, Quaternion.identity);
        //enemies[numberOfSpawnedEnemies].transform.localScale = Vector3.one;

        numberOfSpawnedEnemies++;
        

    }

    




    

    IEnumerator timer()
    {
        yield return new WaitForSeconds(spawnInterval);
        timerExpired = false;
    }




}
