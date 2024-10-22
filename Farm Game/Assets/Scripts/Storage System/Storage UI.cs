using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class StorageUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI storageTypeText;
    [SerializeField] private TextMeshProUGUI maxItemsText;
    [SerializeField] private Slider maxItemsSlider;

    [SerializeField] private GameObject itemsView;
    [SerializeField] private GameObject increaseView;

    [SerializeField] private Transform itemsContent;
    [SerializeField] private Transform increaseContent;

    [SerializeField] private GameObject itemPrefab;

    public void SetNameText(string name)
    {
        storageTypeText.text = name;
    }

    public void Initialize(int currentAmount , int maxAmount , Dictionary<CollectibleItem , int> itemAmounts )
    {
        maxItemsText.text = currentAmount + "/" + maxAmount;
        maxItemsSlider.value = (float)currentAmount / maxAmount;

        InitializeItems(itemAmounts);
    }

    private void InitializeItems(Dictionary<CollectibleItem , int> itemAmounts )
    {
        int childCount = itemsContent.childCount;
        for (int i = 0; i < childCount; i++)
        {
            Destroy(itemsContent.GetChild(i).gameObject);
        }

        foreach (var itemPair in itemAmounts)
        {
            GameObject itemHolder = Instantiate(itemPrefab , itemsContent);
            itemHolder.transform.Find("Icon").GetComponent<Image>().sprite = itemPair.Key.Icon;
            itemHolder.transform.Find("Amount").GetComponent<TextMeshProUGUI>().text = itemPair.Value.ToString();
        }
    }

    #region Buttons

    public void CloseButton_Click()
    {
        gameObject.SetActive(false);
    }

    public void IncreaseButton_Click()
    {
        increaseView.SetActive(true);
    }

    public void ConfirmButton_Click()
    {

    }

    public void BackButton_Click()
    {
        increaseView.SetActive(false);
    }

    #endregion
}
