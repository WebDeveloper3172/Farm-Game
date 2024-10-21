using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StorageBuilding : PlaceableObject
{
    private StorageUI storageUI;

    public string Name { get; private set; }

    [SerializeField] private GameObject windowPrefab;

    public void Initialize(string name)
    {
        Name = name;

        GameObject window = Instantiate(windowPrefab , GameManager.current.canvas.transform);
        window.SetActive(false);
        storageUI = window.GetComponent<StorageUI>();
        storageUI.SetNameText(name);

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
