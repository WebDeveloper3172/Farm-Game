using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectorTooltip : MonoBehaviour
{
    private static CollectorTooltip intance;

    [SerializeField] private Camera uiCamera;
    [SerializeField] private GameObject collectorHolder;

    private void Awake()
    {
        intance = this;
        transform.parent.gameObject.SetActive(false);
    }

    private void ShowTooltip(GameObject caller)
    {
        collectorHolder.GetComponent<Collector>().Initialize(caller.GetComponent<PlaceableObject>());
        Vector3 position = caller.transform.position - uiCamera.transform.position;
        position = uiCamera.WorldToScreenPoint(uiCamera.transform.TransformPoint(position));
        transform.position = position;

        transform.parent.gameObject.SetActive(true);
    }

    public void HideTooltip()
    {
        transform.parent.gameObject.SetActive(false);
    }

    public static void ShowTooltip_Static(GameObject caller)
    {
        intance.ShowTooltip(caller);
    }
    public static void HideTooltip_Static()
    {
        intance.HideTooltip();
    }
}
