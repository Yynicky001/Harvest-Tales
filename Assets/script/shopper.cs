using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class shopper : MonoBehaviour
{
    public bool dialogUIActive;
    public GameObject dialogUI;
    public bool playerInRanger;
    public Button close;

    private void Awake()
    {
        close.onClick.AddListener(CloseDialogUI);
    }
    // Update is called once per frame
    private void Update()
    {
        float distance = Vector3.Distance(PlayerState.Instance.playerbody.transform.position, transform.position);
        if (distance < 10f)
        {
            playerInRanger = true;
        }
        else
        {
            playerInRanger = false;
        }
    }
    public void OpenDialogUI()
    {
        dialogUI.gameObject.SetActive(true);
        dialogUIActive = true;
        Cursor.lockState = CursorLockMode.None;
        Money.Instance.isopen = true;
        Cursor.visible = true;
    }

    public void CloseDialogUI()
    {
        dialogUI.gameObject.SetActive(false);
        dialogUIActive = false;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        Money.Instance.isopen = false;
    }
}
