using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableObject : MonoBehaviour
{
    public string ItemName;

    public bool playerInrange;

   [SerializeField] float detectionRange=10f;
    public string GetItemName()
    {
        return ItemName;
    }
    void Update()
    {

        float dstance = Vector3.Distance(PlayerState.Instance.playerbody.transform.position, transform.position);
        if (dstance < detectionRange)
        {
            playerInrange = true;
        }
        else
        {
            playerInrange = false; 
        }
        if (Input.GetKeyDown(KeyCode.Mouse0)&& playerInrange&&SelectionManager.Instance.selectedObject==gameObject)
        {
            //if the inventory is Not full
            if (!InventorySystem.Instance.CheckIfFull()) 
            { 
            Debug.Log("item added to inventory");
            InventorySystem.Instance.AddToInventory(ItemName);
            Destroy(gameObject);
            }
            else
            {
                Debug.Log("Inventory is full!");
            }
    }
    }
    //由于不知道教程在干什么于是我先将这个代码选择注释
   /* private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInrange = true;
        }

    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInrange = false;
        }
    }*/
}
