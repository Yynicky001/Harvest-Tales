using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using UnityEngine;
using UnityEngine.UI;
[CreateAssetMenu(fileName = "commodityData", menuName = "commodityData/commodityInfo", order = 1)]

public class Commodity:ScriptableObject
    
{
    public GameObject Image;
    public string Name;
    public int price;
    public int sellprice;
    [TextArea(5, 10)]
    public string Description;
    [TextArea(5, 10)]
    public string Functionality;
}
