using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "TalkData", menuName = "TalkData/talkInfo", order = 2)]
public class talkInfo : ScriptableObject
{
    [TextArea(5, 10)]
    public List<string> NPCDialogue;
    [TextArea(5, 10)]
    public List<string> playerDialogue;
    [TextArea(5, 10)]
    public List<string> NPCDialogue2;
    [TextArea(5, 10)]
    public List<string> playerDialogue2;
}
