using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Producible : CollectibleItem
{
    public List<CollectibleItem> ItemTypes;
    public List<int> ItemAmounts;

    public TimePeriod timeStruct;

    public Dictionary<CollectibleItem, int> ItemsNeeded;
    public TimeSpan productionTime;


    [System.Serializable]
    public struct TimePeriod
    {
        public int Days;
        public int Hours;
        public int Minutes;
        public int Seconds;
    }

    protected void OnValidate()
    {
        ItemsNeeded = new Dictionary<CollectibleItem, int>();

        for (int i = 0; i < ItemTypes.Count && i < ItemAmounts.Count; i++)
        {
            ItemsNeeded.Add(ItemTypes[i] , ItemAmounts[i]);
        }

        productionTime = new TimeSpan(timeStruct.Days , timeStruct.Hours , timeStruct.Minutes , timeStruct.Seconds);
    }
}
