using UnityEngine;
using UnityEngine.UI;

public class ProgressBarTracker : MonoBehaviour
{
    [SerializeField] private Image _sleepFillImg;
    [SerializeField] private Image _timerFillImg;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        GameManager.OnSleepValueChangedEvent += OnSleepValueChangedCallback;
        GameManager.OnTimerValueChangedEvent += OnTimerValueChangedCallback;
    }

    void OnDestroy()
    {
        GameManager.OnSleepValueChangedEvent -= OnSleepValueChangedCallback;
        GameManager.OnTimerValueChangedEvent -= OnTimerValueChangedCallback;
    }

    private void OnSleepValueChangedCallback(float curSleep, float maxSleep)
    {
        _sleepFillImg.fillAmount = curSleep / maxSleep;
    }

    private void OnTimerValueChangedCallback(float curTimer, float maxTimer)
    {
        _timerFillImg.fillAmount = curTimer / maxTimer;
    }

}
