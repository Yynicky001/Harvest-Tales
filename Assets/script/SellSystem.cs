using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class SellSystem : MonoBehaviour
{
    public List<Button> sellButton;
    public List<Commodity> commodity;
    public List<Image> images;
    public List<TMP_Text> moneyselldisplay;
    public int moneymount;
    private GameObject itemToAdd;
    void Start()
    {
        ValidateLists();
        for (int i = 0; i < sellButton.Count; i++)
        {
            int index = i;
            sellButton[i].onClick.AddListener(() => OnButtonClick(index));
        }

        // 实例化商品图标
        for (int j = 0; j < images.Count; j++)
        {
            if (j < commodity.Count && commodity[j].Image != null)
            {
                Transform whatSlotToshow = images[j].transform;
                GameObject prefab = commodity[j].Image;
                itemToAdd = Instantiate(prefab, whatSlotToshow.transform);
                RectTransform itemRect = itemToAdd.GetComponent<RectTransform>();
                RectTransform slotRect = whatSlotToshow.transform.GetComponent<RectTransform>();
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
        }
        UpdatePriceDisplays();

    }

    private void UpdatePriceDisplays()
    {
        for (int u = 0; u < moneyselldisplay.Count; u++)
        {
            if (u < commodity.Count && moneyselldisplay[u] != null)
            {
                moneyselldisplay[u].text = commodity[u].price.ToString();
            }
        }
    }
    private void ValidateLists()
    {
        int maxCount = Mathf.Max(sellButton.Count, images.Count, moneyselldisplay.Count);

        // 确保commodity列表长度足够
        while (commodity.Count < maxCount)
        {
            commodity.Add(new Commodity()); // 添加空商品
        }

        // 警告信息
        if (sellButton.Count != maxCount)
            Debug.LogWarning("purchaseButton数量与其他列表不一致");
        if (images.Count != maxCount)
            Debug.LogWarning("images数量与其他列表不一致");
        if (moneyselldisplay.Count != maxCount)
            Debug.LogWarning("moneydisplay数量与其他列表不一致");
    }
    private void OnButtonClick(int index)
    {
        Debug.Log("start");
        sell(index);
    }

    private void sell(int index)
    {
        foreach (string name in InventorySystem.Instance.ReturnObjectNames())
        {
            if (name == commodity[index].Name)
            {
                string money = Money.Instance.moneymount.text;
               
                if (int.TryParse(money,out int moneyAmount))
                {
                    int newMoney = moneyAmount + commodity[index].sellprice; // 计算新的金额
                    Money.Instance.moneymount.text = newMoney.ToString();

                }
                InventorySystem.Instance.Removesellitem(name);
                break;
            }
        }
    }
}
