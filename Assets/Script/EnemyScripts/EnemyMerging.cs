using System.Collections;
using System.Linq;
using System.Net;
using UnityEngine;
using System.Threading;
using System.Collections.Generic;

public class EnemyMerging : MonoBehaviour
{
    public string[] mergeables;
    public static List<GameObject> mergeGroup = new List<GameObject>();
    private GameObject[] objSelected;
    private int countSelectedForMerge = 0;
    public GameObject resultOfMergePrefab;
    public GameObject parent;
    public static bool merging;
    private EnemyAI ai;

    private void Start()
    {
        objSelected = new GameObject[mergeables.Length];
        ai = parent.GetComponentInChildren<EnemyAI>();

    }

    private void OnTriggerStay(Collider other)
    {

        if(ai == null) return;
        if (ai.GetState() != EnemyAI.State.Merge) return;

        lock (mergeGroup)
        {
            //if (merging) return;


            if (!mergeables.Contains(other.tag)) return;

            if (mergeGroup.Contains(other.gameObject) || mergeGroup.Contains(parent)) return;
            if (!(IsEnemyReady(parent) && IsEnemyReady(other.gameObject))) return;
            mergeGroup.Add(other.gameObject);
            mergeGroup.Add(parent);
            //Debug.Log("In merge group: parent: " + parent.name+ " and other: " + other.gameObject.name  );

            if (other.transform.parent != null)
            {
                objSelected[countSelectedForMerge] = other.gameObject;


               // Debug.Log("assigned: " + objSelected[countSelectedForMerge].name);
            }
            else
            {
                // fallback to the collider object itself
                objSelected[countSelectedForMerge] = other.gameObject;
              //  Debug.Log("no parent, using: " + other.gameObject.name);
            }

            countSelectedForMerge++;

            // Check if all required enemies are now selected
            if (countSelectedForMerge == mergeables.Length)
               // Debug.Log("In instance check: parent: " + parent.name + " and other: " + other.gameObject.name);

            {
                int maxIdObj = parent.GetInstanceID();
                foreach (GameObject obj in objSelected)
                {


                    if (obj.GetInstanceID() > maxIdObj)
                    {

                        maxIdObj = obj.GetInstanceID();
                    }
                }
               // if (parent.gameObject.GetInstanceID() == maxIdObj)
               // {
                  //  Debug.Log("In merging " + parent.name + " and " + other.gameObject.name);
               
                
                    merging = true;
                    StartCoroutine(ExecuteMerge());
                

               // }
            }
        }
        
    }

    private IEnumerator ExecuteMerge()
    {
        // Spawn at midpoint between all selected enemies
        
        // Add this to debug

        Coroutine c1 = null;
        Coroutine c2 = null;


        // Destroy all selected enemies
        foreach (GameObject obj in objSelected)
        {

            DissolveEffect dissolve = obj.GetComponentInChildren<DissolveEffect>();
            DissolveEffect dissolveCurrent = transform.parent.gameObject.GetComponentInChildren<DissolveEffect>();
            if (dissolve != null)
            {
              //  Debug.Log("dissolving " + parent.name + " and " );

                if (dissolve != null) c1 = StartCoroutine(dissolve.DissolveOut());
                 if (dissolveCurrent != null) c2 = StartCoroutine(dissolveCurrent.DissolveOut());

            }
            else
            {
        Destroy(parent.transform.gameObject);
                Destroy(obj); // fallback if no dissolve
            }
        }

        if (c1 != null) yield return c1;
        if (c2 != null) yield return c2;
        Vector3 midpoint = Vector3.zero;
        foreach (GameObject obj in objSelected)
        {
            midpoint += obj.transform.position;



        }
        midpoint /= objSelected.Length;

        

        GameObject merge = Instantiate(resultOfMergePrefab, midpoint, Quaternion.identity);
            DissolveEffect dissolveMerge = merge.GetComponentInChildren<DissolveEffect>();
        if (dissolveMerge != null)
        {
            dissolveMerge.StartDissolveIn();
        }
        else {
       //     Debug.Log("No DissolveEffect found on merged prefab");
        }
        merging = false;

    }


    public bool IsEnemyReady(GameObject enemy) { 
        DissolveEffect effect = enemy.GetComponentInChildren<DissolveEffect>();
        for (int i = 0; i < effect._material.Length; i++) {

            if (effect._material[i] != null)
            {
                if (!(effect._material[i].GetFloat("_DissolveAmount") ==0)) {
                    return false;
                }
            }
            else { 
            
            return false;
            }
        }

        return true;

    }
}