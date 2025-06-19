using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Rendering;
using UnityEngine;


public enum Watersource
{
    Lake,
    River,
    Ocean
}
public class FishingSystem : MonoBehaviour
{
   public static FishingSystem instance { get; set; }
    private void Awake()
    {
        if(instance != null && instance != this)
        {
            Destroy(instance );
        }
        else
        {
            instance = this;
        }
    }
    public List<FishData> lakeFishList;//Cod

    public List<FishData> riverFishList;//Salmon
    public List<FishData> oceanFishList;//Tuna
    public bool isThererABite;
    bool hasPulled;
    public static event Action OnFishingEnd;
    public GameObject minigame;
    FishData fishBitting;
    public FishMovement Fishmovement;

    internal void StartFishing(Watersource watersource)
    {
        StartCoroutine(FishingCoroutine(watersource));
    }

    IEnumerator FishingCoroutine(Watersource watersource)
    {
        yield return new WaitForSeconds(3f);
        FishData fish=CalculateBite(watersource);
        if (fish.fishName=="NoBite")
        {
            Debug.LogWarning("No fish caugh");
            EndFishing();
        }
        else
        {
            Debug.LogWarning(fish.fishName+"is biting");
            StartCoroutine(StartFishStruggle(fish));
        }


    }


    private void EndFishing()
    {
        isThererABite = false;
        hasPulled = false;
        fishBitting = null;
        //Trigger end fishing event
        OnFishingEnd?.Invoke();
        FishingMinigame.Instance.isFishOverLapping = false;
        FishingMinigame.Instance.successSlider.value = 0;
        minigame.gameObject.SetActive(false);
        FishingMinigame.Instance.successCounter = 0;
        //rest the Fishing Rod Model
        var slot = EquipSystem.Instance.selectedNumber;
        EquipSystem.Instance.SelectQuicSlot(slot);
        EquipSystem.Instance.SelectQuicSlot(slot);
    }
    public void SetHasPulled()
    {
        hasPulled=true;
    }

    private FishData CalculateBite(Watersource watersource)
    {
        List<FishData> availableFish=GetAvailableFish(watersource);
        float totalProbaility = 0f;
        foreach(FishData fish in availableFish)//tuna 5% 0-4 salmon 20% 5-19 notible 10% 20-34=35%
        {
            totalProbaility += fish.probability;

        }
        //Generate random number between 0 and total probaility
        int randomValue = UnityEngine.Random.Range(0,Mathf.FloorToInt(totalProbaility)+1);//0-35
        Debug.Log("Random value is" + randomValue);
        float cumulativeProbability = 0f;

        foreach (FishData fish in availableFish)
        {
            cumulativeProbability += fish.probability;
            if (randomValue<=cumulativeProbability) 
            { 
              //This fish is biting
              return fish;
            }
            //This should never happen- Random number out of bounds
           
        }
        return null;
    }
    IEnumerator StartFishStruggle(FishData fish)
    {
        isThererABite = true;
        //wait until player pulls the rod
        while (!hasPulled)
        {
            yield return null;
        }
        Debug.LogWarning("Start MiniGame");
        fishBitting = fish;
        StartMinigame();
    }

    private void StartMinigame()
    {
        minigame.gameObject.SetActive(true);
        Fishmovement.SetDifficulty(fishBitting);
    }

    private List<FishData> GetAvailableFish(Watersource watersource)
    {
        switch(watersource)
        {
            case Watersource.Lake:
                return lakeFishList;
            case Watersource.River:
                return riverFishList;
            case Watersource.Ocean:
                return oceanFishList;
                default:
                return null;
        }
    }
    internal void EndMinigame(bool success)
    {
        minigame.gameObject.SetActive(false);
        if (success)
        {
            Debug.Log("Fish Caught");
            InventorySystem.Instance.AddToInventory(fishBitting.fishName);
            EndFishing();
        }
        else
        {
            Debug.Log("Fish Escape");
            EndFishing();
        }
    }
}
