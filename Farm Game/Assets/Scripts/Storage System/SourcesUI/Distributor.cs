using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Distributor : UIDrag
{
    private Dictionary<CollectibleItem, int> itemsNeeded;
    private CollectibleItem itemToProduce;

    public void Initialize<T>(PlaceableObject src, T item, int amount = -1)
        where T : Producible
    {
        base.Initialize(src);
        itemToProduce = item;
        itemsNeeded = item.ItemsNeeded;

        transform.Find("Icon").GetComponent<Image>().sprite = item.Icon;
        transform.Find("Amount").GetComponent<TextMeshProUGUI>().text = amount.ToString();

        if (amount == -1)
        {
            //amountTr.gameObject.SetActive(false);
        }
        else
        {
            //amountTr.gameObject.SetActive(false);
            //amountTr.GetComponent<TextMeshProUGUI>().text = amount.ToString();
        }
        gameObject.SetActive(true);
    }

    protected override void OnCollide(PlaceableObject collidedSource)
    {
        collidedSource.GetComponent<ISource>().Produce(itemsNeeded , itemToProduce);
    }
}
