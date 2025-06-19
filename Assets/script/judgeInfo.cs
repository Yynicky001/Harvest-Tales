using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "judgData", menuName = "judgData/judgInfo", order = 3)]

public class judgeInfo : ScriptableObject
{
    public List<string> initialDialog;
    [TextArea(5, 10)]
    public string rightOption;
    [TextArea(5, 10)]
    public string falseOption;
    [TextArea(5, 10)]
    public string oneOption;
    [TextArea(5, 10)]
    public string twoOption;
    [TextArea(5, 10)]
    public string system;
}
