using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TimerToolTip : MonoBehaviour
{

    private static TimerToolTip instance;

    private Timer timer;

    [SerializeField] private Camera uiCamera;
    [SerializeField] private TextMeshProUGUI callerNameText;
    [SerializeField] private TextMeshProUGUI timeLeftText;
    [SerializeField] private Button skipButton;
    [SerializeField] private TextMeshProUGUI skipAmountText;
    [SerializeField] private Slider progressSlider;

    private bool countdown;

    private void Awake()
    {
        instance = this;
        transform.parent.gameObject.SetActive(false);
    }

    private void ShowTimer(GameObject caller)
    {
        timer = caller.GetComponent<Timer>();

        if (timer == null)
        {
            return;
        }
        callerNameText.text = timer.Name;
        skipAmountText.text = timer.skipAmount.ToString();
        skipButton.gameObject.SetActive(true);

        Vector3 position = caller.transform.position - uiCamera.transform.position;
        position = uiCamera.WorldToScreenPoint(uiCamera.transform.TransformPoint(position));
        transform.position = position;

        countdown = true;
        FixedUpdate();

        transform.parent.gameObject.SetActive(true);
    }

    private void FixedUpdate()
    {
        if (countdown)
        {
            progressSlider.value = (float)(1.0 - timer.secondsLeft / timer.timeToFinish.TotalSeconds);
            timeLeftText.text = timer.DisplayTime();
        }
    }

    public void SkipButton()
    {
        EventManager.Instance.AddListenerOnce<EnoughCurrencyGameEvent>(OnEnoughCurrency);
        EventManager.Instance.AddListenerOnce<NotEnoughCurrencyGameEvent>(OnNotEnoughCurrency);

        CurrencyChangeGameEvent info = new CurrencyChangeGameEvent(-timer.skipAmount , CurrencyType.Crystals);
        EventManager.Instance.QueueEvent(info);
    }

    private void OnEnoughCurrency(EnoughCurrencyGameEvent info)
    {
        timer.SkipTimer();
        skipButton.gameObject.SetActive(false);
        EventManager.Instance.RemoveListener<NotEnoughCurrencyGameEvent>(OnNotEnoughCurrency );
    }

    private void OnNotEnoughCurrency(NotEnoughCurrencyGameEvent info)
    {        EventManager.Instance.RemoveListener<EnoughCurrencyGameEvent>(OnEnoughCurrency);
    }

    public void HideTimer()
    {
        transform.parent.gameObject.SetActive(false);
        timer = null;
        countdown = false;
    }

    public static void ShowTimer_Static(GameObject caller)
    {
        instance.ShowTimer(caller);
    }

    public static void HideTimer_Static()
    {
        instance.HideTimer();
    }
}
