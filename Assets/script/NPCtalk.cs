using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.UI;


public class NPCtalk : MonoBehaviour
{

    public bool playerInRange;
    public bool isTalkingWithPlayer;

    TextMeshProUGUI npcDialogText;

    Button optionButton1;
    TextMeshProUGUI optionButton1Text;

    Button optionButton2;
    TextMeshProUGUI optionButton2Text;

    public List<talkInfo> talkInfos;
    public talkInfo currentActivetalkInfo = null;
    public int activeQuestIndex = 0;
    public bool firstTimeInteraction = true;
    public int currentDialog=0;
    private void Start()
    {
        npcDialogText = talkSystem.Instance.dialogText;

        optionButton1 = talkSystem.Instance.option1;
        optionButton1Text = talkSystem.Instance.option1.transform.Find("Text (TMP)").GetComponent<TextMeshProUGUI>();

        optionButton2 = talkSystem.Instance.option2;
        optionButton2Text = talkSystem.Instance.option2.transform.Find("Text (TMP)").GetComponent<TextMeshProUGUI>();

    }
    public void startconservation()
    {
        isTalkingWithPlayer = true;
        if (firstTimeInteraction)
        {
            isTalkingWithPlayer = false;
            currentActivetalkInfo = talkInfos[0];
            optionButton2.gameObject.SetActive(false);
            StartTalkInitialDialog();

        }
    }

    private void StartTalkInitialDialog()
    {
        talkSystem.Instance.OpenDialogUI();
        npcDialogText.text = currentActivetalkInfo.NPCDialogue[currentDialog];
        optionButton1Text.text=currentActivetalkInfo.playerDialogue[currentDialog];
        optionButton1.onClick.RemoveAllListeners();
        optionButton1.onClick.AddListener(() => {
            currentDialog++;
            CheckIfDialogDone();
        });
    }

    private void CheckIfDialogDone()
    {
        if (currentDialog == currentActivetalkInfo.NPCDialogue.Count - 1) // If its the last dialog 
        {
            npcDialogText.text = currentActivetalkInfo.NPCDialogue[currentDialog];
            optionButton1.onClick.RemoveAllListeners();
            optionButton1.onClick.AddListener(() =>
            {
                closeUI();
                currentDialog = 0;
            });
         }
        else  // If there are more dialogs
        {
            Debug.Log("currentDialog");
           npcDialogText.text = currentActivetalkInfo.NPCDialogue[currentDialog];
           optionButton1Text.text = currentActivetalkInfo.playerDialogue[currentDialog];
            optionButton1.onClick.RemoveAllListeners();
            optionButton1.onClick.AddListener(() => {
                currentDialog++;
                CheckIfDialogDone();
            });
        }
    }

    private void closeUI()
    {
        optionButton1Text.text = "[Close]";
        optionButton1.onClick.RemoveAllListeners();
        optionButton1.onClick.AddListener(() => {
            talkSystem.Instance.CloseDialogUI();
            isTalkingWithPlayer = false;
        });
        optionButton2.gameObject.SetActive(false);
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
        }
    }
}
