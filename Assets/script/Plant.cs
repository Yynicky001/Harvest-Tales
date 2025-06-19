using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Plant : MonoBehaviour
{
    [SerializeField] GameObject seedModel=null;
    [SerializeField] GameObject youngPlantModel;
    [SerializeField] GameObject maturePlantModel;
    [SerializeField] List<GameObject> plantProducesSpawns;
    [SerializeField] GameObject producePrefab;
    public  int dayOfPlanting;
    [SerializeField] int plantAge = 0;
    [SerializeField] int ageForYoungModel;
    [SerializeField] int ageForMatureModel;
    [SerializeField] int ageForFirstProduceBatch;
    [SerializeField] int daysForNewProduce;
    [SerializeField] int daysRemainingForNewProduce;
    [SerializeField] bool isOneTimeHarvest;
    public bool isWatered;
    private void OnEnable()
    {
        TimeManager.Instance.OnDayPass.AddListener(DayPass);
    }
    private void OnDisable()
    {
        TimeManager.Instance.OnDayPass.RemoveListener(DayPass);
    }
    private void OnDestroy()
    {
        GetComponentInParent<Soil>().isEmpty=true;
        GetComponentInParent<Soil>().plantName = "";
        GetComponentInParent<Soil>().currentPlant = null;
    }
    private void DayPass()
    {
        if (isWatered)
        {
            plantAge++;
        }
        CheckGrowth();
        if (!isOneTimeHarvest)
        {
            CheckProduce();
        }
    }

    private void CheckProduce()
    {
        if (plantAge == ageForFirstProduceBatch)
        {
            GenerateProduceForEmptySpawns();
        }
        if (plantAge > ageForFirstProduceBatch)
        {
            if (daysRemainingForNewProduce == 0)
            {
                GenerateProduceForEmptySpawns();
                daysRemainingForNewProduce = daysForNewProduce;
            }
            else
            {
                daysRemainingForNewProduce--;
            }
        }
    }
    

    private void CheckGrowth()
    {
        seedModel.SetActive(plantAge < ageForYoungModel);
        youngPlantModel.SetActive(plantAge >= ageForYoungModel && plantAge < ageForMatureModel);
        maturePlantModel.SetActive(plantAge >= ageForMatureModel);
        if (plantAge>=ageForMatureModel&&isOneTimeHarvest)
        {
            MakePlantPickable();
        }
    }

    private void MakePlantPickable()
    {
        GetComponent<InteractableObject>().enabled = true;
        GetComponent<SphereCollider>().enabled = true;
    }

    private void GenerateProduceForEmptySpawns()
    {
        foreach (GameObject spawn in plantProducesSpawns) 
        {
            if (spawn.transform.childCount==0)
            {
                //Instantiate the produce from the prefab
                GameObject produce = Instantiate(producePrefab);
                //Set the produce to be a child of the current spawn in the list;
                produce.transform.parent= spawn.transform;
                //Position the produce in the middle of the spawn
                Vector3 producePosition=Vector3.zero;
                producePosition.y = 0f;
                produce.transform.localPosition = producePosition;
            } 
        }
    }
}
