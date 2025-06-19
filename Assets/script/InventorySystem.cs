using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventorySystem : MonoBehaviour
{
    public GameObject ItemInfoUI;
    public static InventorySystem Instance { get; set; }

    public GameObject inventoryScreenUI;
    public List<GameObject> slotList = new List<GameObject>();
    public List<string> itemList= new List<string>();
    private GameObject itemToAdd;
    private GameObject whatSlotToEquip;

    public bool isOpen;

    //public bool isFull;

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


    void Start()
    {
        isOpen = false;
        PopulateSlotList();
        Cursor.visible = false;
      
    }

    private void PopulateSlotList()
    {
        foreach (Transform child in inventoryScreenUI.transform)
        {
            if(child.CompareTag("slot"))
            {
                slotList.Add(child.gameObject);
            }
        }
    }

    void Update()
    {

        if (Input.GetKeyDown(KeyCode.I) && !isOpen)
        {

            Debug.Log("i is pressed");
            inventoryScreenUI.SetActive(true);
            isOpen = true;
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
        else if (Input.GetKeyDown(KeyCode.I) && isOpen)
        {
            inventoryScreenUI.SetActive(false);
            isOpen = false;
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
    }
    public void AddToInventory(string itemName)
    {

        whatSlotToEquip = FindNextEmptSlot();
        if (whatSlotToEquip == null)
        {
            Debug.LogError("没有可用的空槽位!");
            return;
        }

        // 加载并实例化物品
        GameObject prefab = Resources.Load<GameObject>(itemName);
        
        if (prefab == null)
        {
            Debug.LogError($"无法加载物品预制体: {itemName}");
            return;
        }

        itemToAdd = Instantiate(prefab, whatSlotToEquip.transform); // 直接设置父对象
        itemList.Add(itemName);

        // 设置物品UI参数
        RectTransform itemRect = itemToAdd.GetComponent<RectTransform>();
        RectTransform slotRect = whatSlotToEquip.GetComponent<RectTransform>();

        if (itemRect != null && slotRect != null)
        {
            // 让物品图标填满整个物品槽
            itemRect.anchorMin = Vector2.zero;    // 左下角锚点
            itemRect.anchorMax = Vector2.one;     // 右上角锚点
            itemRect.offsetMin = Vector2.zero;    // 内边距：左、下
            itemRect.offsetMax = Vector2.zero;    // 内边距：右、上

            // 如果物品有Image组件，可以进一步设置
            Image itemImage = itemToAdd.GetComponent<Image>();
            if (itemImage != null)
            {
                itemImage.preserveAspect = true; // 保持图片原始比例
            }
        }

    }

    private GameObject FindNextEmptSlot()
    {
        foreach (GameObject slot in slotList)
        {
            if(slot.transform.childCount == 0)
            {
                return slot;    
            }
          
        }
        return new GameObject();
    }

    public bool CheckIfFull() 
    {
        int counter = 0;
        foreach (GameObject slot in slotList)
        {
            if (slot.transform.childCount>0)
            {
                counter += 1;
            }
            
        }
        if (counter == 32)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public void ReCalculateList()
    {
        itemList.Clear();
        foreach (GameObject slot in slotList)
        {
            if(slot.transform.childCount>0)
            {
                string name=slot.transform.GetChild(0).name;
                string str2 = "(clone)";
                string result = name.Replace(str2, "");
                itemList.Add(result);
            }
        }
    }
    public void RemoveItem(string nameToRemove,int amountToRemove)
    {
        int counter = amountToRemove;
        for (var i = slotList.Count - 1; i >= 0; i--) 
        {
            if (slotList[i].transform.childCount > 0)
            {
                if (slotList[i].transform.GetChild(0).name == nameToRemove + "(Clone)" && counter != 0)
                {
                    DestroyImmediate(slotList[i].transform.GetChild(0).gameObject);
                    counter -= 1;
                }
            }
        }
         ReCalculateList();

    }
    public void Removesellitem(string name)
    {
        for (var i = slotList.Count - 1; i >= 0; i--)
        {
            if (slotList[i].transform.childCount > 0)
            {
                Transform child = slotList[i].transform.GetChild(0);
                InventoryItem item = child.GetComponent<InventoryItem>();
                if (item.thisName == name)
                {
                    DestroyImmediate(slotList[i].transform.GetChild(0).gameObject);
                    break;
                }
            }
        }
    }
    public IEnumerable<string> ReturnObjectNames()
    {
        foreach (GameObject slot in slotList)
        {
            InventoryItem item = slot.GetComponentInChildren<InventoryItem>();
            if (item != null && !string.IsNullOrEmpty(item.thisName))
            {
                yield return item.thisName; // 逐个返回名称
            }
        }
    }

    internal void ReCalculeList()
    {
        throw new NotImplementedException();
    }
}
