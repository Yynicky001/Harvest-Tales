using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Rendering.VirtualTexturing;
using UnityEngine.UI;

public class judge : MonoBehaviour
{
    public bool playerInRange;
    public bool isTalkingWithPlayer;

    TextMeshProUGUI npcDialogText;

    public Button optionButton1;
    TextMeshProUGUI optionButton1Text;

    public Button optionButton2;
    TextMeshProUGUI optionButton2Text;

    public List<judgeInfo> talkInfos;
    public judgeInfo currentActivetalkInfo = null;
    public int activeQuestIndex = 0;
    public bool firstTimeInteraction = true;
    public int currentDialog = 0;
    public string system;
   

    private void Start()
    {
        npcDialogText = judgmentsystem.Instance.dialogText;

        optionButton1 = judgmentsystem.Instance.option1;
        optionButton1Text = judgmentsystem.Instance.option1.transform.Find("Text (TMP)").GetComponent<TextMeshProUGUI>();

        optionButton2 = judgmentsystem.Instance.option2;
        optionButton2Text = judgmentsystem.Instance.option2.transform.Find("Text (TMP)").GetComponent<TextMeshProUGUI>();

    }
    public void startconservation()
    {
        firstTimeInteraction = true;

        if (firstTimeInteraction)
        {
            firstTimeInteraction = false;
            currentActivetalkInfo = talkInfos[activeQuestIndex];
            optionButton2.gameObject.SetActive(false);
            system = currentActivetalkInfo.system;
            startTalkInitialDialog();

        }
       
    }

    private void startTalkInitialDialog()
    {
        judgmentsystem.Instance.OpenDialogUI();

        npcDialogText.text = currentActivetalkInfo.initialDialog[currentDialog];
        optionButton1Text.text = "Next";
        optionButton1.onClick.RemoveAllListeners();
        optionButton1.onClick.AddListener(() => {
            currentDialog++;
            CheckIfDialogDone();
        });

        optionButton2.gameObject.SetActive(false);
    }

    private void CheckIfDialogDone()
    {
        if (currentDialog == currentActivetalkInfo.initialDialog.Count ) // If its the last dialog 
        {
           // npcDialogText.text = currentActivetalkInfo.initialDialog[currentDialog];

           

            SetAcceptAndDeclineOptions();
        }
        else  // If there are more dialogs
        {
            //npcDialogText.text = currentActivetalkInfo.initialDialog[currentDialog];

            optionButton1Text.text = "Next";
            optionButton1.onClick.RemoveAllListeners();
            optionButton1.onClick.AddListener(() => {
                currentDialog++;
                CheckIfDialogDone();
            });
        }
    }

    private void SetAcceptAndDeclineOptions()
    {
        if (system == "1")
        {
            npcDialogText.text = "";
            optionButton2.gameObject.SetActive(true);
            optionButton1Text.text = currentActivetalkInfo.oneOption;
            optionButton1.onClick.RemoveAllListeners();
            optionButton1.onClick.AddListener(() =>
            {
                AcceptedQuest();
            });

            optionButton2.gameObject.SetActive(true);
            optionButton2Text.text = currentActivetalkInfo.twoOption;
            optionButton2.onClick.RemoveAllListeners();
            optionButton2.onClick.AddListener(() =>
            {
                DeclinedQuest();
            });
        }
        else
        {
            npcDialogText.text = "";
            optionButton2.gameObject.SetActive(true);
            optionButton1Text.text = currentActivetalkInfo.oneOption;
            optionButton2.onClick.RemoveAllListeners();
            optionButton2.onClick.AddListener(() =>
            {
                AcceptedQuest();
            });

            optionButton2.gameObject.SetActive(true);
            optionButton2Text.text = currentActivetalkInfo.twoOption;
            optionButton1.onClick.RemoveAllListeners();
            optionButton1.onClick.AddListener(() =>
            {
                DeclinedQuest();
            });
        }
    }

    private void DeclinedQuest()
    {
        npcDialogText.text = currentActivetalkInfo.falseOption;
        CloseDialogUI();
        optionButton2.gameObject.SetActive(false);
        currentDialog = 0;
    }

    private void AcceptedQuest()
    {
        npcDialogText.text = currentActivetalkInfo.rightOption;
        CloseDialogUI();
        activeQuestIndex++;
        optionButton2.gameObject.SetActive(false);
        currentDialog = 0;
    }

    private void CloseDialogUI()
    {
        optionButton1Text.text = "[Close]";
        optionButton1.onClick.RemoveAllListeners();
        optionButton1.onClick.AddListener(() => {
            judgmentsystem.Instance.CloseDialogUI();
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
