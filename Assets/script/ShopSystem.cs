using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static UnityEditor.Timeline.Actions.MenuPriority;

public class ShopSystem : MonoBehaviour
{
   
    public List<Button> purchaseButton;
    public List<Commodity> commodity;
    public List<Image> images;
    public List<TMP_Text> moneypurchasedisplay;
    public int moneymount;
    private GameObject itemToAdd;
    private void Start()
    {
        // 初始化金钱显示
        moneymount = int.Parse(Money.Instance.moneymount.text);
        UpdateMoneyDisplay();

        // 确保所有列表长度一致
        ValidateLists();

        // 初始化按钮事件
        for (int i = 0; i < purchaseButton.Count; i++)
        {
            int index = i;
            purchaseButton[i].onClick.AddListener(() => OnButtonClick(index));
        }

        // 实例化商品图标
        for (int j = 0; j < images.Count; j++)
        {
            if (j < commodity.Count && commodity[j].Image != null)
            {
               Transform whatSlotToshow = images[j].transform;
                GameObject prefab = commodity[j].Image;
                itemToAdd = Instantiate(prefab,whatSlotToshow.transform);
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

        // 初始化价格显示
        UpdatePriceDisplays();
    }

    private void ValidateLists()
    {
        int maxCount = Mathf.Max(purchaseButton.Count, images.Count, moneypurchasedisplay.Count);

        // 确保commodity列表长度足够
        while (commodity.Count < maxCount)
        {
            commodity.Add(new Commodity()); // 添加空商品
        }

        // 警告信息
        if (purchaseButton.Count != maxCount)
            Debug.LogWarning("purchaseButton数量与其他列表不一致");
        if (images.Count != maxCount)
            Debug.LogWarning("images数量与其他列表不一致");
        if (moneypurchasedisplay.Count != maxCount)
            Debug.LogWarning("moneydisplay数量与其他列表不一致");
    }

    private void UpdatePriceDisplays()
    {
        for (int u = 0; u < moneypurchasedisplay.Count; u++)
        {
            if (u < commodity.Count && moneypurchasedisplay[u] != null)
            {
                moneypurchasedisplay[u].text = commodity[u].price.ToString();
            }
        }
    }

    private void UpdateMoneyDisplay()
    {
        Money.Instance.moneymount.text = moneymount.ToString();
    }


    public void Purchase(int purchaseSite)
    {
        moneymount= int.Parse(Money.Instance.moneymount.text);
        // 检查索引有效性
        if (purchaseSite < 0 || purchaseSite >= commodity.Count)
        {
            Debug.Log($"无效的购买索引: {purchaseSite}");
            return;
        }

        int purchaseAmount = commodity[purchaseSite].price;

        if (moneymount >= purchaseAmount && InventorySystem.Instance.CheckIfFull() == false)
        {
            // 扣除金钱
            moneymount -= purchaseAmount;
            UpdateMoneyDisplay();

            // 添加到库存
             InventorySystem.Instance.AddToInventory(commodity[purchaseSite].Name);
            
            // 购买成功的其他逻辑...
        }
        else
        {
            // 显示购买失败的提示
            Debug.Log("金钱不足或背包已满");
        }
    }
    private void OnButtonClick(int index)
    {
        Purchase(index);
    }
}
