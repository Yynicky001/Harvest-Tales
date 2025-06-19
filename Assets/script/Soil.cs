using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Soil : MonoBehaviour
{
    public bool isEmpty = true;
    public bool playerInRanger;
    public string plantName;
    public Plant currentPlant;

    public Material defaultMaterial;
    public Material wateredMaterial;
    internal void MakeSoilWatered()
    {
       //GetComponent<Renderer>().material = wateredMaterial;
    }
    internal void MakeSoilNotWatered()
    {
        //GetComponent<Renderer>().material = defaultMaterial;
    }

    internal void PlantSeed()
    {
        InventoryItem selectedSeed=EquipSystem.Instance.selectedItem.GetComponent<InventoryItem>();
        isEmpty = false;
        Debug.Log("???");
        string onlyPlantName = selectedSeed.thisName.Split(new string[] { " ÖÖ×Ó" }, StringSplitOptions.None)[0];
        plantName = onlyPlantName;
        //Instantiate Plant Prefab;
        GameObject instantiatedPlant = Instantiate(Resources.Load($"{onlyPlantName}Ö²Îï") as GameObject);
        //Set the instantiated plant to be a child of the soil
        instantiatedPlant.transform.parent = gameObject.transform;
        //Make the plant's position in the middle of the soil
        Vector3 plantPosition = Vector3.zero;
        plantPosition.y = 0f;
        instantiatedPlant.transform.localPosition= plantPosition;
        //Set reference to the plant
        currentPlant= instantiatedPlant.GetComponent<Plant>();
        //Set Planting Day
        currentPlant.dayOfPlanting = TimeManager.Instance.dayInGame;
    }

    private void Update()
    {
        float distance = Vector3.Distance(PlayerState.Instance.playerbody.transform.position, transform.position);
        if(distance < 10f)
        {
            playerInRanger = true;
        }
        else
        {
            playerInRanger = false;
        }
    }
}
