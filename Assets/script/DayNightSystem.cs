using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DayNightSystem : MonoBehaviour
{
    public Light directionalLight;
    public float dayDurationInSeconds = 24.0f;//addjust the duration of a full day
    public int currentHour;
    float currentTimeOfDay = 0.1f;
    public List<SkyboxTimeMapping> timeMappings;
    bool lockNextDayTrigger=false;
    public TextMeshProUGUI timeUI;
    public TextMeshProUGUI dayUI;
    public WeatherSystem weatherSystem;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //Calculatte the current time of day based on the game time
        currentTimeOfDay += Time.deltaTime / dayDurationInSeconds;
        currentTimeOfDay %= 1;//Ensore it stays at between 1and 2;
        currentHour = Mathf.FloorToInt(currentTimeOfDay * 24);
        timeUI.text = $"{currentHour}:00";
        //Update the directional Light's ratation
        
        directionalLight.transform.rotation = Quaternion.Euler(new Vector3((currentTimeOfDay * 360) - 90, 170, 0));
        //Update the skybox material based on the time of day
        if (weatherSystem.isSpecialWeather==false)
        {
            UpdateSkybox();
        }
        if (currentHour == 0 && lockNextDayTrigger == false)
        {
            TimeManager.Instance.TriggerNextDay();
            lockNextDayTrigger = true;
        }
        if (currentHour != 0)
        {
            lockNextDayTrigger = false;

        }

    }

    private void UpdateSkybox()
    {
        //Find the appropriate skybox material for the current hour
        Material currentSkybox = null;
        foreach (SkyboxTimeMapping mapping in timeMappings)
        {
            if (currentHour == mapping.hour)
            {
                currentSkybox=mapping.skyboxMaterial;
                break;
            }
        }
       
        if (currentSkybox!=null)
        {
            RenderSettings.skybox=currentSkybox;

        }
    }
}
[System.Serializable] 
public class SkyboxTimeMapping
{
    public string phaseName;
    public int hour;
    public Material skyboxMaterial;
}
