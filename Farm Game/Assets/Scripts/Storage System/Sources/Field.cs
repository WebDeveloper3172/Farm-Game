using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Field : PlaceableObject , ISource
{
    private static Dictionary<Crop, int> allCrops;
    private static int amount = 2;

    private SpriteRenderer sr;
    private Sprite emptyFieldSprite;

    public State currentState { get; set; }
    private Crop currentCrop;
    private Timer timer;

    private void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
        emptyFieldSprite = sr.sprite;
    }

    public static void Initialize(Dictionary<Crop , int> crops)
    {
        allCrops = crops;
    }
    protected override void OnClick()
    {
        switch (currentState)
        {
            case State.Empty:
                ItemsTooltip.ShowTooltip_Static(gameObject , allCrops);
                break;
            case State.InProgress:
                TimerToolTip.ShowTimer_Static(gameObject);
                break;
            case State.Ready:
                CollectorTooltip.ShowTooltip_Static(gameObject);
                break;
        }
    }

    public void Produce(Dictionary<CollectibleItem, int> itemsNeeded, CollectibleItem itemToProduce)
    {
        if (currentState != State.Empty)
        { 
            return;
        }
        if (itemToProduce is Crop crop)
        {
            currentCrop = crop;
        }
        else
        {
            return;
        }

        foreach (var itemPair in itemsNeeded)
        { 
            //if(!StorageManager.current.IsEnoughOf(itemPair.Key , itemPair.Value))
            //{
            //    Debug.Log("Not enough items");
            //    return;
            //}
        }

        currentState = State.InProgress;
        sr.sprite = currentCrop.growingCrop;
        timer = gameObject.AddComponent<Timer>();
        timer.Initialize(currentCrop.name , DateTime.Now , currentCrop.productionTime);
        timer.TimerFinishedEvent.AddListener( delegate
        {
            currentState = State.Ready;
            sr.sprite = currentCrop.readyCrop;

            Destroy(timer);
            timer = null;
        });
        timer.StartTimer();
    }

    public void Collect()
    {
        currentState = State.Empty;
        sr.sprite = emptyFieldSprite;
        currentCrop = null;

    }
}
