using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemsTooltip : MonoBehaviour
{
    private static ItemsTooltip instance;

    [SerializeField] private Camera uiCamera;
    [SerializeField] private List<GameObject> itemHolders;

    private void Awake()
    {
        instance = this;
        transform.parent.gameObject.SetActive(false);
    }

    private void ShowTooltip <T>(GameObject caller , Dictionary<T , int> items)
        where T : Producible
    {
        int i = 0;
        foreach(var itemPair in items)
        {
            itemHolders[i].GetComponent<Distributor>().Initialize(caller.GetComponent<PlaceableObject>() , itemPair.Key , itemPair.Value);

            i++;
            if (i >= itemHolders.Count)
            { break; }
        }
        Vector3 position = caller.transform.position - uiCamera.transform.position;
        position = uiCamera.WorldToScreenPoint(uiCamera.transform.TransformPoint(position));
        transform.position = position;

        transform.parent.gameObject.SetActive(true);
    }

    public void HideToolTip()
    {
        for ( int i = 0; i < itemHolders.Count; i++)
        {
            itemHolders[i].SetActive(false);
        }
        transform.parent.gameObject.SetActive(false);
    }

    public static void ShowTooltip_Static<T>(GameObject caller , Dictionary<T , int> items)
        where T : Producible
    {
        instance.ShowTooltip(caller , items);
    }

    public static void HideTooltip_static()
    {
        instance.HideToolTip();
    }
}
