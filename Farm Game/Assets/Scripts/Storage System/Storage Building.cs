using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class StorageBuilding : PlaceableObject
{
    private StorageUI storageUI;

    private int currentTotal = 0;
    private int storageMax = 100;

    public string Name { get; private set; }

    [SerializeField] private GameObject windowPrefab;

    private Dictionary<CollectibleItem, int> items;

    public void Initialize(Dictionary<CollectibleItem , int> itemAmounts, string name)
    {
        Name = name;

        GameObject window = Instantiate(windowPrefab , GameManager.current.canvas.transform);
        window.SetActive(false);
        storageUI = window.GetComponent<StorageUI>();
        storageUI.SetNameText(name);

        items = itemAmounts;
        currentTotal = itemAmounts.Values.Sum();

        storageUI.Initialize(currentTotal , storageMax , items);

    }

    public virtual void onClick()
    {
        storageUI.gameObject.SetActive(true);
    }

    private void OnMouseUpAsButton()
    {
        onClick();
    }
}
