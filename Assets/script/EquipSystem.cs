using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using Unity.VisualScripting.Antlr3.Runtime.Tree;
using UnityEditor.Build.Content;
using UnityEngine;
using UnityEngine.UI;
public class EquipSystem : MonoBehaviour
{
    public static EquipSystem Instance { get; set; }

    // -- UI -- //
    public GameObject quickSlotsPanel;

    public List<GameObject> quickSlotsList = new List<GameObject>();


    public GameObject numbersHolders;
    public int selectedNumber = -1;
    public GameObject selectedItem;
    public GameObject selectedItemModel;
    public GameObject toolHolder;
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


    private void Start()
    {
        PopulateSlotList();
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            SelectQuicSlot(1);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            SelectQuicSlot(2);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3))
        {

            SelectQuicSlot(3);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            SelectQuicSlot(4);

        }
        else if (Input.GetKeyDown(KeyCode.Alpha5))
        {

            SelectQuicSlot(5);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha6))
        {
            SelectQuicSlot(6);

        }
        else if (Input.GetKeyDown(KeyCode.Alpha7))
        {
            SelectQuicSlot(7);

        }
        else if (Input.GetKeyDown(KeyCode.Alpha8))
        {

            SelectQuicSlot(8);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha9))
        {
            SelectQuicSlot(9);

        }
    }

    public void SelectQuicSlot(int number)
    {
        if (checkIfSlotsFull(number) == true)
        {
            if (selectedNumber != number)
            {
                selectedNumber = number;
                //Unselected previously selected item
                if (selectedItem != null)
                {
                    selectedItem.gameObject.GetComponent<InventoryItem>().isSelected = false;
                }
                selectedItem = GetSelectedItem(number);
                selectedItem.GetComponent<InventoryItem>().isSelected = true;
                SetEquippedModel(selectedItem);
                //Changing color
                foreach (Transform child in numbersHolders.transform)
                {
                    //有bug因为使用MaxText导致了我不能用改变颜色，不知道为什么，所以之后要改的话用Text的文本会比较好
                    child.transform.Find("Text").GetComponent<TextMeshProUGUI>().color = Color.gray;
                }
                TextMeshProUGUI toBeChanged = numbersHolders.transform.Find("number" + number).transform.Find("text").GetComponent<TextMeshProUGUI>();
                toBeChanged.color = Color.gray;
            }
            else//select same slot
            {
                DestroyAllChildren();
                selectedNumber = -1;//null
                if (selectedItem != null)
                {
                    selectedItem.gameObject.GetComponent<InventoryItem>().isSelected = false;
                    selectedItem = null;
                }
                foreach (Transform child in numbersHolders.transform)
                {
                    child.transform.Find("Text").GetComponent<TextMeshProUGUI>().color = Color.gray;
                }
            }
        }
    }

    private void SetEquippedModel(GameObject selectedItem)
    {
        if (selectedItemModel != null)
        {
            Debug.Log("works");
            DestroyImmediate(selectedItemModel.gameObject);
            selectedItemModel = null;
        }
        string selectedItemName = selectedItem.name.Replace("(Clone)", "");
        // selectedItemModel = Instantiate(Resources.Load<GameObject>(selectedItemName + "_Model"), new Vector3(0.6f, 0, 0.4f), Quaternion.Euler(0, -12.5f, -20f));
        selectedItemModel = Instantiate(Resources.Load<GameObject>(CalculateItemModel(selectedItemName)));
        selectedItemModel.transform.SetParent(toolHolder.transform, false);
    }

    private string CalculateItemModel(string selectedItemName)
    {
        switch (selectedItemName)
        {
            case "番茄 种子":
                return "Hand_Model";
            case "南瓜 种子":
                return "Hand_Model";
            case "西瓜 种子":
                return "Hand_Model";
            case "辣椒 种子":
                return "Hand_Model";
            case "水壶":
                return "wateringcan_Model";
            case "钓鱼竿":
                return "FishingRod_Model";
            case "锄头":
                return "hoe";
            case "喷火器":
                return "flamethrowgun_Model";
            case "除熊专家":
                return "flamethrowgun_Modelgun";
            default:
                return null;
        }
    }
    void DestroyAllChildren()
    {
        // 获取当前物体的 Transform 组件
        Transform parentTransform = toolHolder.transform;

        // 遍历子物体，注意要从后往前遍历，因为销毁物体时会改变子物体的数量
        for (int i = parentTransform.childCount - 1; i >= 0; i--)
        {
            // 获取当前子物体的 Transform 组件
            Transform childTransform = parentTransform.GetChild(i);
            // 销毁子物体
            Destroy(childTransform.gameObject);
        }
    }

    GameObject GetSelectedItem(int slotNumber)
    {
        return quickSlotsList[slotNumber - 1].transform.GetChild(0).gameObject;
    }
    private bool checkIfSlotsFull(int slotNumber)
    {
        if (quickSlotsList[slotNumber - 1].transform.childCount > 0)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    private void PopulateSlotList()
    {
        foreach (Transform child in quickSlotsPanel.transform)
        {
            if (child.CompareTag("QuickSlot"))
            {
                quickSlotsList.Add(child.gameObject);
            }
        }
    }

    public void AddToQuickSlots(GameObject itemToEquip)
    {
        // Find next free slot
        GameObject availableSlot = FindNextEmptySlot();
        // Set transform of our object
        itemToEquip.transform.SetParent(availableSlot.transform, false);
        // Getting clean name
        string cleanName = itemToEquip.name.Replace("(Clone)", "");
        // Adding item to list


        InventorySystem.Instance.ReCalculateList();

    }


    private GameObject FindNextEmptySlot()
    {
        foreach (GameObject slot in quickSlotsList)
        {
            if (slot.transform.childCount == 0)
            {
                return slot;
            }
        }
        return new GameObject();
    }
    public bool IsPlayerHoldingSeed()
    {
        if (selectedItemModel != null)
        {
            switch (selectedItemModel.gameObject.name)
            {
                case "Hand_Model(Clone)":
                    return true;
                case "Hand_Model":
                    return true;
                default:
                    return false;

            }
        }
        else
        {
            return false;
        }
    }
    public bool CheckIfFull()
    {

        int counter = 0;

        foreach (GameObject slot in quickSlotsList)
        {
            if (slot.transform.childCount > 0)
            {
                counter += 1;
            }
        }

        if (counter == 9)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    internal bool IsPlayerHoldingWateringCan()
    {
        if (selectedItem != null)
        {
            switch (selectedItem.GetComponent<InventoryItem>().thisName)
            {
                case "水壶":
                    return true;
               
                default:
                    return false;

            }
        }
        else
        {
            return false;
        }
    }
    internal bool IsPlayerHoldinghoe()
    {
        if (selectedItem != null)
        {
            switch (selectedItem.GetComponent<InventoryItem>().thisName)
            {
                case "锄头":
                    return true;

                default:
                    return false;

            }
        }
        else
        {
            return false;
        }
    }

    internal bool IsHoldingWeapon()
    {
        if (selectedItem != null) 
        {
            if (selectedItem.GetComponent<Weapon>() != null)
            {
                return true ;
            }
            else
            {
                return false ;
            }
        }
        else
        {
            return false;
        }
    }

    internal int GetWeaponDamage()
    {
        if (selectedItem != null)
        {
            return selectedItem.GetComponent<Weapon>().weaponDamage;
        }
        else 
        {
            return 0;
        }
    }
}