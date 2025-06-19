using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime;
using UnityEngine;
using UnityEngine.UI;


public class Map : MonoBehaviour
{
    public Button optionButton1;
    public Button optionButton2;
    private bool isOpen;
    public GameObject MapScreenUI;
    public Transform player, destination1,destination2;
    public GameObject playerg;
    public static Map Instance { get; set; }
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

    }
    void Update()
    {

        if (Input.GetKeyDown(KeyCode.K) && !isOpen)
        {

            Debug.Log("i is pressed");
            MapScreenUI.SetActive(true);
            isOpen = true;
            Cursor.lockState = CursorLockMode.None;
        }
        else if (Input.GetKeyDown(KeyCode.K) && isOpen)
        {
            MapScreenUI.SetActive(false);
            isOpen = false;
            Cursor.lockState = CursorLockMode.Locked;
        }
        if (Input.GetKeyDown(KeyCode.Mouse0)&&isOpen==true)
        {

            
            optionButton1.onClick.AddListener(() =>
            {

                transformMap1();
                optionButton1.onClick.RemoveAllListeners();
            });
          
            optionButton2.onClick.AddListener(() =>
            {

                transformMap2();
                optionButton2.onClick.RemoveAllListeners();
            });
        }
    }

    private void transformMap2()
    {
        playerg.SetActive(false);
        player.position = destination2.position;
        playerg.SetActive(true);
    }

    private void transformMap1()
    {

        playerg.SetActive(false);
        player.position = destination1.position;
        playerg.SetActive(true);
    }
}
