using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LevelSystem : MonoBehaviour
{
    private int XPNow;
    public static int Level;
    private int xpToNext;

    [SerializeField] private GameObject levelPanel;
    [SerializeField] private GameObject lvlWindowPrefab;

    private Slider slider;
    private TextMeshProUGUI xpText;
    private TextMeshProUGUI lvlText;
    private Image starImage;

    private static bool initialized;
    private static Dictionary<int , int> xpToNextLevel = new Dictionary<int , int>();
    private static Dictionary<int, int[]> lvlReward = new Dictionary<int, int[]>();

    private void Awake()
    {
        slider = FindDeepChild(levelPanel.transform, "Slider").GetComponent<Slider>();
        xpText = FindDeepChild(levelPanel.transform, "XP text").GetComponent<TextMeshProUGUI>();
        starImage = FindDeepChild(levelPanel.transform, "Star").GetComponent<Image>();
        lvlText = starImage.transform.GetChild(0).GetComponent<TextMeshProUGUI>();

        if (!initialized)
        { 
            Initialized();
        }
        xpToNextLevel.TryGetValue(Level , out xpToNext);

    }
    private static void Initialized()
    {
        try
        {
            // path to the csv file
            string path = "levelsXP";
            TextAsset textAsset = Resources.Load<TextAsset>(path);
            string[] lines = textAsset.text.Split('\n');

            xpToNextLevel = new Dictionary<int, int>(lines.Length - 1);

            for (int i = 1; i < lines.Length - 1; i++)
            {
                string[] columns = lines[i].Split(',');

                int lvl = -1;
                int xp = -1;
                int curr1 = -1;
                int curr2 = -1;

                int.TryParse(columns[0], out lvl);
                int.TryParse(columns[1], out xp);
                int.TryParse(columns[2], out curr1);
                int.TryParse(columns[3], out curr2);

                if (lvl >= 0 && xp > 0)
                {
                    if (!xpToNextLevel.ContainsKey(lvl))
                    {
                        xpToNextLevel.Add(lvl, xp);
                        lvlReward.Add(lvl, new []{ curr1, curr2 });
                    }
                }

            }
        }
        catch (Exception ex)
        {
            Debug.Log(ex.Message);
        }

        initialized = true;
    }

    private void Start()
    {
        EventManager.Instance.AddListener<XPAddedGameEvent>(OnXPAdded);
        EventManager.Instance.AddListener<LevelChangedGameEvent>(OnLevelChanged);

        UpdateUI();
    }
    private void UpdateUI()
    {
        if (xpToNext == 0)
        {
            Debug.LogError("UpdateUI: xpToNext este 0, ceea ce va cauza NaN. Verifică inițializarea valorilor.");
            return;
        }
        float fill = (float)XPNow / xpToNext;
        slider.value = fill;
        //Debug.Log($"UpdateUI: Valoarea slider-ului setată la {fill}. XPNow: {XPNow}, xpToNext: {xpToNext}");

        xpText.text = XPNow + " / " + xpToNext;
    }

    private void OnXPAdded(XPAddedGameEvent info) 
    {
        XPNow += info.amount;
        //Debug.Log($"OnXPAdded: Experiență adăugată: {info.amount}. XPNow actualizat la {XPNow}");


        UpdateUI();
       
        if(XPNow >= xpToNext)
        {
            Level++;
            //Debug.Log("OnXPAdded: XPNow a depășit xpToNext, nivelul va fi crescut.");
            LevelChangedGameEvent levelChange = new LevelChangedGameEvent(Level);
            EventManager.Instance.QueueEvent(levelChange);
        }
    }

    private void OnLevelChanged(LevelChangedGameEvent info)
    {
        XPNow -= xpToNext;
        xpToNext = xpToNextLevel[info.newLvl];
        if (xpToNext == 0)
        {
            Debug.LogError("OnLevelChanged: xpToNext este 0 după schimbarea nivelului. Verifică valorile în xpToNextLevel.");
        }
        else
        {
            Debug.Log($"OnLevelChanged: xpToNext actualizat la {xpToNext} pentru nivelul {info.newLvl}.");
        }

        lvlText.text = (info.newLvl + 1).ToString();
        UpdateUI();

        // Corectare: folosim metoda standard de instanțiere din Unity
        GameObject window = Instantiate(lvlWindowPrefab, GameManager.current.canvas.transform);

        Transform windowLvlTextTransform = FindDeepChild(window.transform, "levelText"); 

        if (windowLvlTextTransform != null)
        {
            // Получаем компонент TextMeshProUGUI
            TextMeshProUGUI windowLvlText = windowLvlTextTransform.GetComponent<TextMeshProUGUI>();

            if (windowLvlText != null)
            {
                windowLvlText.text = $"You've reached Level {info.newLvl + 1}!"; // Устанавливаем желаемый текст
            }
            else
            {
                Debug.LogError("OnLevelChanged: Не удалось получить компонент TextMeshProUGUI из lvlWindowPrefab.");
            }
        }
        else
        {
            Debug.LogError("OnLevelChanged: lvlText не найден в lvlWindowPrefab.");
        }

        //initialize text and image here
        window.transform.GetChild(2).GetComponent<Button>().onClick.AddListener(() =>
        {
            Destroy(window);
        });

        CurrencyChangeGameEvent currencyInfo =
            new CurrencyChangeGameEvent(lvlReward[info.newLvl][0], CurrencyType.Coins);
        EventManager.Instance.QueueEvent(currencyInfo);

        currencyInfo =
           new CurrencyChangeGameEvent(lvlReward[info.newLvl][1], CurrencyType.Crystals);
        EventManager.Instance.QueueEvent(currencyInfo);
    }

  

      


    //private GameObject Instantiate(GameObject lvlWindowPrefab, object transform)
    //{
    //    throw new NotImplementedException();
    //}

    private Transform FindDeepChild(Transform parent, string childName)
    {
        foreach (Transform child in parent)
        {
            if (child.name == childName)
                return child;
            Transform result = FindDeepChild(child, childName);
            if (result != null)
                return result;
        }
        return null;
    }
}
