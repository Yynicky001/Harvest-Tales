using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class FishingMinigame : MonoBehaviour
{
    public RectTransform fishTransform;
    public RectTransform catcherTransform;
    public bool isFishOverLapping;
    public Slider successSlider;
    float successIncrement = 15;
    float failDecrement = 12;
    float successThreshold = 100;
    float failThreshold = -100;
    public float successCounter = 0;


    public static FishingMinigame Instance { get; set; }

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
    private void Update()
    {
        if (CheckOverLapping(fishTransform,catcherTransform))
        {
            isFishOverLapping = true;
        }
        else
        {
            isFishOverLapping = false;
        }
        overlappingCalculation();
    }

    private void overlappingCalculation()
    {
        if (isFishOverLapping)
        {
            successCounter += successIncrement * Time.deltaTime;
        }
        else 
        {
            successCounter-=failDecrement * Time.deltaTime;
        }
        successCounter = Mathf.Clamp(successCounter,failThreshold,successThreshold);
        successSlider.value = successCounter;
        if (successCounter>=successThreshold)
        {
            FishingSystem.instance.EndMinigame(true);
            Debug.Log("success");
            successCounter = 0;
            successSlider.value = 0;
            isFishOverLapping=false;
        }
        else if (successCounter<=failThreshold)
        {
            FishingSystem.instance.EndMinigame(false);
            Debug.Log("failed");
            successCounter = 0;
            successSlider.value = 0;
            isFishOverLapping = false;
        }
    }

    private bool CheckOverLapping(RectTransform rect1, RectTransform rect2)
    {
        Rect r1 = new Rect(rect1.position.x, rect1.position.y, rect1.rect.width, rect1.rect.height);
        Rect r2 = new Rect(rect2.position.x, rect2.position.y, rect2.rect.width, rect2.rect.height);
        return r1.Overlaps(r2);
    }
}
