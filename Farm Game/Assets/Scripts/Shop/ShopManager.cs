using System.Collections;
using System.Collections.Generic;
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
    }

    private void Start()
    {
        currencySprites.Add(CurrencyType.Coins , sprites[0]);
        currencySprites.Add(CurrencyType.Crystals, sprites[1]);

        gameObject.SetActive(false);
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





