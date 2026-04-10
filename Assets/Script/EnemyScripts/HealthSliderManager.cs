using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class HealthSliderManager : MonoBehaviour
{
    public GameObject parent;
    public Slider healthSlider;
    public int maxHealthValue;
    private int currentHealthValue = 0;
    public static int kills = 0;
    private onKillClaze onKill;
    public List<LootItem> lootItems = new List<LootItem>();
    public EnemyTier enemyTier;
   // private PlayerController playerController;
  //  public GameObject PlayerUI;
   // public GameObject GameOverUI;
   // public GameObject WinUI;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        healthSlider.maxValue = maxHealthValue;
        healthSlider.value = maxHealthValue;
        currentHealthValue = maxHealthValue;
        healthSlider.fillRect.gameObject.SetActive(true);
        if (parent.CompareTag("Claze1")) { 
            
        onKill = GetComponent<onKillClaze>();
        }
    }

    // Update is called once per frame
  
    public void Heal(int amount)
    {
        currentHealthValue += amount;
        healthSlider.value = Mathf.Min(healthSlider.maxValue, currentHealthValue);
    }

    public void resolveCollision(int amount)
    {
        currentHealthValue -= amount;
        healthSlider.value = currentHealthValue;
        if (currentHealthValue <= 0)
        {
            if (parent.CompareTag("Claze1"))
            {
                onKill.OnEnemyDeath();


            }
            if (!parent.CompareTag("Player"))
            {
                Die();
                GameManager.Instance.enemiesKilled++;
            }
            if (parent.CompareTag("Player"))
            {
                SceneManager.LoadScene("DeathScreen");
            }
        }
    }

    private void Die() {
        
        dropItem();
        kills++;
        Destroy(parent, 0.1f);
    }

    public void dropItem()
    {
        foreach(LootItem item in lootItems)
        {
            float dropChance = item.GetDropChance(enemyTier);
            if (Random.Range(0f,100f) <= dropChance)
            {
                InstantiateLoot(item.itemPrefab);
                break;

            }
        }
    }
    public void InstantiateLoot(GameObject loot) {

        if (loot != null)
        {
            GameObject droppedLoot = Instantiate(loot,new Vector3(transform.position.x,0f,transform.position.z),Quaternion.identity); 
        }
    }

}
