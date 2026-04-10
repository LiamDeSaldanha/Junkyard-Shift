using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class OxygenManager : MonoBehaviour
{

    public Transform t_parent;
    public float max_oxygen = 100f;
    public Slider player_oxygen ;
    public List<OxygenZone> zones = new List<OxygenZone>();
    public List<FilterRecipe> recipes = new List<FilterRecipe>();
   
    public int equippedFilterTier = 0;
    private bool isDead = false;
    private OxygenZone currentZone;
    private bool timerRunning = false;
    public HealthSliderManager healthSliderManager;
    public static OxygenManager Instance { get; private set; }
    



    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject); 
    }


    private void Start()
    {
        player_oxygen.maxValue = max_oxygen;
        player_oxygen.value = max_oxygen;

    }
    private void Update()
    {
        if (timerRunning) return;
        UpdateZone();
        if (currentZone == null) return;
        Debug.Log("startcorotine");
        
       timerRunning = true;
        StartCoroutine(OxygenLoop());
        
        HandleOxygenDrain();
    }
    private IEnumerator OxygenLoop()
    {
        Debug.Log("in loop: "+currentZone.tickOfZone);
        yield return new WaitForSeconds(currentZone.tickOfZone);
        timerRunning = false;
    }
        private void HandleOxygenDrain()
    {
        if (currentZone == null) return;

        if (equippedFilterTier >= currentZone.requiredFilterTier)
        {

            // Filter is sufficient — refill slowly
            player_oxygen.value = Mathf.Min(max_oxygen,player_oxygen.value +currentZone.refillRatePerSecond);
        }
        else
        {
            // Filter insufficient — drain
            

           

            player_oxygen.value = Mathf.Max(0f, player_oxygen.value - currentZone.drainRatePerSecond);

            if (player_oxygen.value <= 0f)
            {
                healthSliderManager.resolveCollision(5);
            }
                //Die();
        }

    }
    bool CanCraft(FilterRecipe recipe)
    {
        bool flag = true;

        foreach (RecipeIngredients ing in recipe.ingredients)
        {
            if( ResourceManager.Instance.GetResource(ing.item) < ing.amount)
            {
                return false;
            }
        }

        return flag;
    }
    private void UpdateZone()
    {
        OxygenZone newZone = zones
             .Where(z => t_parent.transform.position.z >= z.zThreshold)
             .OrderByDescending(z => z.zThreshold)
             .FirstOrDefault();

        if (newZone != currentZone)
        {
            currentZone = newZone;
            Debug.Log($"Entered zone: {currentZone?.zoneName ?? "None"}");
        }
    }
}
