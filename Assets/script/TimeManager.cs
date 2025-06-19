using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class TimeManager : MonoBehaviour
{
    public static TimeManager Instance { get; set; }
    public UnityEvent OnDayPass=new UnityEvent();
    public enum Season
    {
        Spring,
        Summer,
        Fall,
        Winter,
    
    }
    public Season currentSeason = Season.Spring;
    private int daysPerSeason = 2;
    private int daysInCurrentSeason = 1;
   // public UnityEvent 
    public enum DaOfWeek
    {
        Monday,
        Tuesday,
        Wednesday,
        Thursday,
        Friday,
        Saturday,
        Sunday
    }
    public DayOfWeek currentDayOfweek= DayOfWeek.Monday;
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
    public int dayInGame = 1;
    public int yearIGame = 0;
    public TextMeshProUGUI dayUI;
    private void Start()
    {
        dayUI.text = $"{currentDayOfweek},{dayInGame},{currentSeason}";
    }
    public void TriggerNextDay()
    {
        dayInGame+=1;
        daysInCurrentSeason += 1;
        currentDayOfweek = (DayOfWeek)(((int)currentDayOfweek+1)%7);
        
        if (daysInCurrentSeason>daysPerSeason)
        {
            daysInCurrentSeason = 1;
            currentSeason = GetNextSeason();
        }
        UpdateUI();
        OnDayPass.Invoke();
    }

    private Season GetNextSeason()
    {
        int currentSeasonIndex = (int)currentSeason;
        int nextSeasonIndex = (currentSeasonIndex + 1) % 4;
        if (nextSeasonIndex==0)
        {
            yearIGame += 1;
        }
        return (Season)nextSeasonIndex;
    }

    private void UpdateUI()
    {
        dayUI.text = $"{currentDayOfweek}{dayInGame},{currentSeason}";
    }
}
