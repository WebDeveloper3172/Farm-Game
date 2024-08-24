using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CurencySystem : MonoBehaviour
{
    private static Dictionary<CurrencyType, int> CurrencyAmounts = new Dictionary<CurrencyType, int>();

    [SerializeField] private List<GameObject> texts;

    private Dictionary<CurrencyType, TextMeshProUGUI> currencyText = 
        new Dictionary<CurrencyType, TextMeshProUGUI>();

    private void Awake()
    {
        for (int i = 0;  i < texts.Count; i++)
        {
            CurrencyAmounts.Add((CurrencyType)i , 0);
            currencyText.Add((CurrencyType)i , texts[i].transform.GetChild(0).GetComponent<TextMeshProUGUI>());
        }    
    }
    private void Start()
    {
        EventManager.Instance.AddListener<CurrencyChangeGameEvent>(OnCurrencyChange);
        EventManager.Instance.AddListener<NotEnoughCurrencyGameEvent>(OnNotEnough);
    }
    private void OnCurrencyChange(CurrencyChangeGameEvent info)
    {
        CurrencyAmounts[info.currencyType] += info.amount;
        currencyText[info.currencyType].text = CurrencyAmounts[info.currencyType].ToString();
    }

    private void OnNotEnough(NotEnoughCurrencyGameEvent info)
    {
        Debug.Log($"You don't have enough of {info.amount} {info.currencyType}");
    }
}

public enum CurrencyType
{ 
    Coins,
    Crystals
}