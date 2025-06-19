using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEditor.AssetImporters;
using UnityEngine;
using UnityEngine.UI;

public class SelectionManager : MonoBehaviour
{

    public GameObject interaction_Info_UI;
    TMP_Text interaction_text;
    public Image centerDotImage;
    public static SelectionManager Instance { get; set; }
    public GameObject selectedObject;
    public GameObject selectedSoil;
    private void Start()
    {
        interaction_text = interaction_Info_UI.GetComponent<TMP_Text>();
    }
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }
    }

    void Update()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit))
        {
            var selectionTransform = hit.transform;
            InteractableObject ourInteractable = selectionTransform.GetComponent<InteractableObject>();

            NPC npc =selectionTransform.GetComponent<NPC>();
            NPCtalk nPCtalk=selectionTransform.GetComponent<NPCtalk>();
            judge judge=selectionTransform.GetComponent<judge>();
           
            if (npc&&npc.playerInRange)
            {
                interaction_text.text = "与NPC对话";
                interaction_Info_UI.SetActive(true);
                if (Input.GetMouseButtonDown(0) && npc.isTalkingWithPlayer == false)
                {
                    npc.StartConversation();
                }
                if (DialogSystem.Instance.dialogUIActive)
                {
                    interaction_Info_UI.SetActive(false);
                    centerDotImage.gameObject.SetActive(true);
                }
                
            }
            else
            {
                interaction_text.text = "";
                interaction_Info_UI.SetActive(false);
            }
            if (nPCtalk && nPCtalk.playerInRange) 
            {
                interaction_text.text = "talk";
                interaction_Info_UI.SetActive(true);
                if (Input.GetMouseButtonDown(0) && nPCtalk.isTalkingWithPlayer == false)
                {
                    nPCtalk.startconservation();
                }
                if (talkSystem.Instance.dialogUIActive)
                {
                    interaction_Info_UI.SetActive(true);
                    centerDotImage.gameObject.SetActive(false);
                }
            }
            if (judge && judge.playerInRange)
            {
                interaction_text.text = "judgementtalk";
                interaction_Info_UI.SetActive(true);
                if (Input.GetMouseButtonDown(0) && judge.isTalkingWithPlayer == false)
                {
                    judge.startconservation();
                }
                if (judgmentsystem.Instance.dialogUIActive)
                {
                    interaction_Info_UI.SetActive(true);
                    centerDotImage.gameObject.SetActive(false);
                }
            }

           

            //Harvest 的实验代码，在大互联网加的时候需要我们把这个给删掉
            Soil soil = selectionTransform.GetComponent<Soil>();
            if (soil && soil.playerInRanger)            {
                if (soil.isEmpty&&EquipSystem.Instance.IsPlayerHoldingSeed())
                {
                    string seedName= EquipSystem.Instance.selectedItem.GetComponent<InventoryItem>().thisName;
                    string onlyPlantName = seedName.Split(new string[] { " Seed"},StringSplitOptions.None)[0];
                    interaction_text.text = "种植 "+ onlyPlantName;
                    interaction_Info_UI.SetActive(true);
                    if (Input.GetMouseButton(0))
                    {
                        soil.PlantSeed();
                        Destroy(EquipSystem.Instance.selectedItem);
                        Destroy(EquipSystem.Instance.selectedItemModel);
                    }
                }
                else if (soil.isEmpty)
                {
                    interaction_text.text = "土壤";
                    interaction_Info_UI.SetActive(true);
                }
                else
                {
                    if (EquipSystem.Instance.IsPlayerHoldingWateringCan())
                    {
                        if (soil.currentPlant.isWatered)
                        {
                            interaction_text.text = soil.plantName;
                            interaction_Info_UI.SetActive(true);
                        }
                        else
                        {
                            interaction_text.text = "浇水";
                            interaction_Info_UI.SetActive(true);
                            if (Input.GetMouseButton(0))
                            {
                                soil.currentPlant.isWatered = true;
                                soil.MakeSoilWatered();
                            }
                        }

                    }
                    else
                    {
                        interaction_text.text = soil.plantName;
                        interaction_Info_UI.SetActive(true);
                    }
                    if (EquipSystem.Instance.IsPlayerHoldinghoe())
                    {
                        if (soil.isEmpty == false)
                        {
                            interaction_text.text = "除掉植物";
                            interaction_Info_UI.SetActive(true);
                            
                        }
                    }
                    
                }
                selectedSoil = soil.gameObject;
            }
            else
            {
                if (selectedSoil != null)
                {
                    selectedSoil = null;
                }
            }

            if (ourInteractable && ourInteractable.playerInrange)
            {
                selectedObject = ourInteractable.gameObject;
                interaction_text.text = selectionTransform.GetComponent<InteractableObject>().GetItemName();
                interaction_Info_UI.SetActive(true);
            }
            else
            {
                //interaction_Info_UI.SetActive(false);
            }
            /*if (!soil)
            {
                interaction_text.text = "";
                interaction_Info_UI.SetActive(false);
            }*/

        }
        if (Physics.Raycast(ray, out hit))
        {
            var selectionTransform = hit.transform;
            shopper shopper = selectionTransform.GetComponent<shopper>();
            if (shopper && shopper.playerInRanger)
            {
                if (shopper.transform.CompareTag("shopper"))
                {
                    interaction_text.text = "与商人对话";
                    interaction_Info_UI.SetActive(true);
                }
                if (shopper.transform.CompareTag("sellshopper"))
                {
                    interaction_text.text = "与出售商人对话";
                    interaction_Info_UI.SetActive(true);
                }

                Debug.Log("open");
                if (Input.GetMouseButtonDown(0))
                {
                    shopper.OpenDialogUI();
                }

            }
        }
        if (Physics.Raycast(ray, out hit))
        {
            var selectionTransform = hit.transform;
            Animal animal = selectionTransform.GetComponent<Animal>();
            if (animal && animal.playerInRange)
            {
                interaction_text.text = animal.animalName;
                interaction_Info_UI.SetActive(true);
                if (Input.GetMouseButton(0) && EquipSystem.Instance.IsHoldingWeapon())
                {
                    StartCoroutine(DealDamageTO(animal, 0.3f, EquipSystem.Instance.GetWeaponDamage()));
                }
                else
                {
                    interaction_text.text = "";
                    interaction_Info_UI.SetActive(false);
                }
            }
        }
    }

    IEnumerator DealDamageTO(Animal animal, float delay, int damage)
    {
        yield return new WaitForSeconds(delay);

        animal.TakeDamage(damage);
    }
}