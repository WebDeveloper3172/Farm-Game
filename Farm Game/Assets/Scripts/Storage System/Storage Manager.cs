using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class StorageManager : MonoBehaviour
{
    public static StorageManager current;

    [SerializeField] private GameObject barnPrefab;
    [SerializeField] private GameObject siloPrefab;

    private string itemsPath = "Storage";
    private Dictionary<AnimalProduct, int> animalProducts;
    private Dictionary<Crop, int> crops;
    private Dictionary<Feed, int> feeds;
    private Dictionary<Fruit, int> fruits;
    private Dictionary<Product, int> products;
    private Dictionary<Tool, int> tools;

    private Dictionary<CollectibleItem, int> barnItems;
    private Dictionary<CollectibleItem, int> siloItems;


    private StorageBuilding Barn;
    private StorageBuilding Silo;

    private void Awake()
    {
        current = this;
        Dictionary<CollectibleItem, int> itemsAmounts = LoadItems();
        Sort(itemsAmounts);

        Field.Initialize(crops);
    }

    private Dictionary<CollectibleItem, int> LoadItems()
    {
        Dictionary<CollectibleItem, int> itemAmounts = new Dictionary<CollectibleItem, int>();
        CollectibleItem[] allItems = Resources.LoadAll<CollectibleItem>(itemsPath);

        for (int i = 0; i< allItems.Length; i++)
        {
            if (allItems[i].Level >= LevelSystem.Level)
            {
                itemAmounts.Add(allItems[i] , 2);
            }
        }

        return itemAmounts;
    }

    private void Sort(Dictionary<CollectibleItem , int> itemsAmounts)
    {
        animalProducts = new Dictionary<AnimalProduct, int>();
        crops = new Dictionary<Crop, int>();
        feeds = new Dictionary<Feed, int>();
        fruits = new Dictionary<Fruit, int>();
        products = new Dictionary<Product, int>();
        tools = new Dictionary<Tool, int>();

        siloItems = new Dictionary<CollectibleItem, int>();
        barnItems = new Dictionary<CollectibleItem, int>();

        foreach (var itemPair in itemsAmounts) 
        {
            if(itemPair.Key is AnimalProduct animalProduct)
            {
                animalProducts.Add(animalProduct , itemPair.Value);
                barnItems.Add(animalProduct , itemPair.Value);
            }
            else if (itemPair.Key is Crop crop)
            {
                crops.Add(crop, itemPair.Value);
                siloItems.Add(crop, itemPair.Value);
            }
            else if (itemPair.Key is Feed feed)
            {
                feeds.Add(feed, itemPair.Value);
                barnItems.Add(feed, itemPair.Value);
            }
            else if (itemPair.Key is Fruit fruit )
            {
                fruits.Add(fruit, itemPair.Value);
                siloItems.Add(fruit, itemPair.Value);
            }
            else if (itemPair.Key is Product product)
            {
                products.Add(product, itemPair.Value);
                barnItems.Add(product, itemPair.Value);
            }
            else if (itemPair.Key is Tool tool)
            {
                tools.Add(tool, itemPair.Value);
                barnItems.Add(tool, itemPair.Value);
            }

        }

    }

    private void Start()
    {
        GameObject siloObject = BuildingSystem.current.InitializeWithObject(siloPrefab , new Vector3(-13f , 4f));
        Silo = siloObject.GetComponent<StorageBuilding>();
        Silo.Load();
        Silo.Initialize(siloItems,"Silo");

        GameObject barnObject = BuildingSystem.current.InitializeWithObject(barnPrefab , new Vector3(-21f , 6f));
        Barn = barnObject.GetComponent<StorageBuilding>();
        Barn.Load();
        Barn.Initialize(barnItems, "Barn");
    } 
}
