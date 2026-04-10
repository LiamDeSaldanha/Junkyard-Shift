using System.Collections;
using UnityEngine;

public class Boss_spawning : MonoBehaviour
{
    public GameObject enemyPrefab;
    public float spawnInterval;
    public GameObject[] enemies;
    public GameObject[] enemiesInstance;
    private int numberOfSpawnedEnemies;
    public float sizeOfSpawnArea;
    private bool timerExpired = false;
    private Boss_AI Boss_AI ;
   
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Boss_AI = GetComponent<Boss_AI>();
        enemiesInstance = new GameObject[enemies.Length];
    }

    // Update is called once per frame
    void Update()
    {

        if(Boss_AI.GetState() == Boss_AI.State.Summon && !timerExpired )
        {

            timerExpired = true;
            if (AllEnemiesSlain())
            {
                spawn();
            }

            StartCoroutine(timer());

        }


        

    }

    private void OnTriggerStay(Collider other)
    {

        while (numberOfSpawnedEnemies < enemies.Length && !timerExpired && other.CompareTag("Player"))
        {
            timerExpired = true;
            spawn();
            StartCoroutine(timer());


        }


    }


    private void spawn()
    {
        if (numberOfSpawnedEnemies < enemies.Length)
        {

            enemyPrefab = enemies[Random.Range(0, enemies.Length)];


            Vector3 randomPos = new Vector3(Random.Range(-sizeOfSpawnArea, sizeOfSpawnArea), 0f, Random.Range(-sizeOfSpawnArea, sizeOfSpawnArea));

            enemiesInstance[numberOfSpawnedEnemies] = Instantiate(enemyPrefab, Vector3.zero, Quaternion.identity, transform);
            enemiesInstance[numberOfSpawnedEnemies].transform.localPosition = randomPos;
            //enemies[numberOfSpawnedEnemies].transform.localScale = Vector3.one;

            numberOfSpawnedEnemies++;
        }

    }

    public bool AllEnemiesSlain()
    {
        if (numberOfSpawnedEnemies == enemiesInstance.Length)
        {
            for (int i = 0; i < enemiesInstance.Length; i++)
            {

                if (enemiesInstance[i] != null)
                {

                    return false;
                }
                else
                {
                    numberOfSpawnedEnemies--;
                }
            }
            return true;
        }
        else
        {

            return false;
        }




    }

    IEnumerator timer()
    {
        yield return new WaitForSeconds(spawnInterval);
        timerExpired = false;
    }

   


}
