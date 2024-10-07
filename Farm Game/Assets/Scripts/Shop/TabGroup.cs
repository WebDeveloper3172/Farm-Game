using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TabGroup : MonoBehaviour
{
    public Sprite tabIdle;
    public Sprite tabActive;

    public List<TabButton> tabButtons = new List<TabButton>();
    public List<GameObject> objectsToSwap = new List<GameObject>();
    public List<TextMeshProUGUI> textsToSwap = new List<TextMeshProUGUI>();

    public ScrollRect scrollRect;

    [NonSerialized] public TabButton selectedTab;

    private void Start()
    {
        OnTabSelected(tabButtons[0]);
    }

    public void Subscribe(TabButton button)
    {
        tabButtons.Add(button);
    }

    private void ResetTabs()
    {
        foreach (var button in tabButtons)
        {
            if (selectedTab != null && button == selectedTab)
            {
                continue;
            }
           
            button.background.sprite = tabIdle;
        }
    }

    public void OnTabSelected(TabButton button)
    {
        selectedTab = button;
        ResetTabs();
        button.background.sprite = tabActive;

        int index = button.transform.GetSiblingIndex();

        for (int i=0; i < objectsToSwap.Count; i++)
        {
            if (i == index)
            {
                objectsToSwap[i].SetActive(true);
                textsToSwap[i].gameObject.SetActive(true);
                scrollRect.content = objectsToSwap[i].GetComponent<RectTransform>();
            }
            else
            {
                objectsToSwap[i].SetActive(false);
                textsToSwap[i].gameObject.SetActive(false);
            }
        }
    }
}

