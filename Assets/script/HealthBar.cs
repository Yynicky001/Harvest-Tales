using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    private Slider slider;
    public TMP_Text healthCounter;
    public GameObject playerState;
    private float currentHealth, maxHealth;


    void Start()
    {
        slider=GetComponent<Slider>();
    }

    // Update is called once per frame
    void Update()
    {
        currentHealth = playerState.GetComponent<PlayerState>().currentHealth;
        maxHealth=playerState.GetComponent<PlayerState>().maxHealth;
        float fillValue = currentHealth / maxHealth;
        slider.value = fillValue;
        healthCounter.text = currentHealth + "/" + maxHealth;

    }
}
