using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
 
public class ShopManager : MonoBehaviour
   
{
    public static ShopManager current;
    public static Dictionary<CurrencyType, Sprite> currencySprites = new Dictionary<CurrencyType , Sprite>();

    [SerializeField] private List<Sprite> sprites;

    private RectTransform rt;
    private RectTransform prt;
    private bool opened;

    [SerializeField] private GameObject itemPrefab;
    private Dictionary<ObjectType, List<ShopItem>> shopItems = new Dictionary<ObjectType, List<ShopItem>>(5);

    [SerializeField] public TabGroup shopTabs;

    private void Awake()
    {
        current = this;

        rt = GetComponent<RectTransform>();
        prt = transform.parent.GetComponent<RectTransform>();

        EventManager.Instance.AddListener<LevelChangedGameEvent>(OnLevelChanged);
    }

    private void Start()
    {
        currencySprites.Add(CurrencyType.Coins , sprites[0]);
        currencySprites.Add(CurrencyType.Crystals, sprites[1]);

        Load();
        Initialize();

        gameObject.SetActive(false);
    }

    private void Load() 
    {
        ShopItem[] items = Resources.LoadAll<ShopItem>("Shop");

        shopItems.Add(ObjectType.Animals , new List<ShopItem>());
        shopItems.Add(ObjectType.AnimalHomes, new List<ShopItem>());
        shopItems.Add(ObjectType.ProductionBuilding, new List<ShopItem>());
        shopItems.Add(ObjectType.TreesBushes, new List<ShopItem>());
        shopItems.Add(ObjectType.Decorations, new List<ShopItem>());

        foreach (var item in items)
        {
            shopItems[item.Type].Add(item);
        }

    }

    private void Initialize()
    {
        for (int i = 0; i < shopItems.Keys.Count; i++)
        { 
           foreach( var item in shopItems[(ObjectType)i])
           {
                GameObject itemObject = Instantiate(itemPrefab , shopTabs.objectsToSwap[i].transform);
                itemObject.GetComponent<ShopItemHolder>().Initialize(item);
           }
        }
    }

    private void OnLevelChanged(LevelChangedGameEvent info)
    {
        for (int i = 0; i< shopItems.Keys.Count; i++)
        {
            ObjectType key = shopItems.Keys.ToArray()[i];

            for (int j = 0; j < shopItems[key].Count; j++)
            {
                ShopItem item = shopItems[key][j];

                
                if (item.Level == info.newLvl)
                {
                    shopTabs.transform.GetChild(i).GetChild(j).GetComponent<ShopItemHolder>().UnlockItem();
                }
            }
        }

    }

    public void ShopButton_Click()
    {
        float time = 0.2f;
        if (!opened)
        {
            LeanTween.moveX( prt , prt.anchoredPosition.x + rt.sizeDelta.x , time);
            opened = true;
            gameObject.SetActive(true);

        }
        else
        {
            LeanTween.moveX(prt, prt.anchoredPosition.x - rt.sizeDelta.x, time)
                .setOnComplete(delegate ()
                {
                    gameObject.SetActive(false);
                });
            opened = false;

        }
    }

    private bool dragging;

    public void OnBeginDrag()
    {
        dragging = true;
    }
    public void OnEndDrag()
    {
        dragging = false;
    }

    public void OnPointerClick()
    {
        if (!dragging)
        {
            ShopButton_Click();
        }
    }

}





