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
