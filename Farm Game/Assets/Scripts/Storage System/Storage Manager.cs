using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StorageManager : MonoBehaviour
{
    public static StorageManager current;

    [SerializeField] private GameObject barnPrefab;
    [SerializeField] private GameObject siloPrefab;

    private StorageBuilding Barn;
    private StorageBuilding Silo;

    private void Awake()
    {
        current = this;
    }

    private void Start()
    {
        GameObject siloObject = BuildingSystem.current.InitializeWithObject(siloPrefab , new Vector3(-13f , 4f));
        Silo = siloObject.GetComponent<StorageBuilding>();
        Silo.Load();
        Silo.Initialize("Silo");

        GameObject barnObject = BuildingSystem.current.InitializeWithObject(barnPrefab , new Vector3(-21f , 6f));
        Barn = barnObject.GetComponent<StorageBuilding>();
        Barn.Load();
        Barn.Initialize("Barn");
    } 
}
