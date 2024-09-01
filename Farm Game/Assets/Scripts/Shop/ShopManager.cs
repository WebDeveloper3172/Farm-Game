using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ShopManager : MonoBehaviour

{
    public static ShopManager current;
    public static Dictionary<CurrencyType, Sprite> currencySprites = new Dictionary<CurrencyType, Sprite>();

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
        prt = transform.parent != null ? transform.parent.GetComponent<RectTransform>() : null;

        if (prt == null)
        {
            Debug.LogError("Awake: `prt` este null. Asigură-te că obiectul are un părinte cu `RectTransform`.");
        }

        EventManager.Instance.AddListener<LevelChangedGameEvent>(OnLevelChanged);
    }

    private void Start()
    {
        currencySprites.Add(CurrencyType.Coins, sprites[0]);
        currencySprites.Add(CurrencyType.Crystals, sprites[1]);

        Load();
        Initialize();

        gameObject.SetActive(false);
    }

    private void Load()
    {
        ShopItem[] items = Resources.LoadAll<ShopItem>("Shop");

        shopItems.Add(ObjectType.Animals, new List<ShopItem>());
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
            foreach (var item in shopItems[(ObjectType)i])
            {
                GameObject itemObject = Instantiate(itemPrefab, shopTabs.objectsToSwap[i].transform);
                itemObject.GetComponent<ShopItemHolder>().Initialize(item);
            }
        }
    }

    private void OnLevelChanged(LevelChangedGameEvent info)
    {
        if (shopTabs == null)
        {
            Debug.LogError("OnLevelChanged: shopTabs este null. Asigură-te că este corect atribuit.");
            return;
        }

        // Iterăm prin cheile din dicționarul shopItems
        for (int i = 0; i < shopItems.Keys.Count; i++)
        {
            ObjectType key = shopItems.Keys.ToArray()[i];

            // Iterăm prin lista de ShopItem-uri asociate fiecărui key
            for (int j = 0; j < shopItems[key].Count; j++)
            {
                ShopItem item = shopItems[key][j];

                // Verificăm dacă nivelul item-ului corespunde cu nivelul curent
                if (item.Level == info.newLvl)
                {
                    if (i >= shopTabs.objectsToSwap.Count)
                    {
                        Debug.LogError($"OnLevelChanged: Indexul i ({i}) depășește numărul de obiecte din shopTabs.objectsToSwap.");
                        continue;
                    }

                    Transform tabTransform = shopTabs.objectsToSwap[i].transform;
                    if (tabTransform.childCount <= j)
                    {
                        Debug.LogError($"OnLevelChanged: Indexul j ({j}) depășește numărul de copii ai transformului.");
                        continue;
                    }

                    // Accesăm obiectul copil specific și componenta ShopItemHolder
                    Transform itemTransform = tabTransform.GetChild(j);
                    ShopItemHolder itemHolder = itemTransform.GetComponent<ShopItemHolder>();
                    if (itemHolder == null)
                    {
                        Debug.LogError("OnLevelChanged: Componenta ShopItemHolder nu a fost găsită pe obiectul de tip ShopItem.");
                        continue;
                    }

                    // Aici are loc logica de deblocare a item-ului
                    itemHolder.UnlockItem();
                    Debug.Log($"OnLevelChanged: Item-ul {itemHolder.name} a fost deblocat pentru nivelul {info.newLvl}.");
                }
            }
        }
    }



    public void ShopButton_Click()
    {
        float time = 0.2f;

        // Debug pentru a verifica dacă variabila `prt` este null
        if (prt == null)
        {
            Debug.LogError("ShopButton_Click: `prt` este null.");
            return;
        }

        // Debug pentru a verifica dacă variabila `rt` este null
        if (rt == null)
        {
            Debug.LogError("ShopButton_Click: `rt` este null.");
            return;
        }

        Debug.Log("ShopButton_Click: opened = " + opened);

        if (!opened)
        {
            Debug.Log("ShopButton_Click: Deschiderea magazinului.");
            LeanTween.moveX(prt, prt.anchoredPosition.x + rt.sizeDelta.x, time);
            opened = true;
            gameObject.SetActive(true);
        }
        else
        {
            Debug.Log("ShopButton_Click: Închiderea magazinului.");
            LeanTween.moveX(prt, prt.anchoredPosition.x - rt.sizeDelta.x, time)
                .setOnComplete(delegate ()
                {
                    Debug.Log("ShopButton_Click: Dezactivarea gameObject-ului după închidere.");
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