using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Building : PlaceableObject
{
    // Variabile pentru a seta durata timerului
    [SerializeField] private int daysToComplete = 0;
    [SerializeField] private int hoursToComplete = 0;
    [SerializeField] private int minutesToComplete = 0;
    [SerializeField] private int secondsToComplete = 0;

    public override void Place()
    {
        base.Place();

        // Creează un nou timer și setează durata folosind zile, ore, minute și secunde
        Timer timer = gameObject.AddComponent<Timer>();
        TimeSpan timeToComplete = new TimeSpan(daysToComplete, hoursToComplete, minutesToComplete, secondsToComplete);

        timer.Initialize("Building", DateTime.Now, timeToComplete);
        timer.StartTimer();
        timer.TimerFinishedEvent.AddListener(delegate
        {
            Destroy(timer);
        });
    }

    private void OnMouseUpAsButton()
    {
        TimerToolTip.ShowTimer_Static(gameObject);
    }
}
