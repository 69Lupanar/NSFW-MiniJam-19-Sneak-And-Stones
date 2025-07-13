using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Displays the progression of the monster's stats on the UI
/// </summary>
public class ProgressBarTracker : MonoBehaviour
{
    #region Variables

    /// <summary>
    /// Container of the sleep meter
    /// </summary>
    [SerializeField] private GameObject _sleep;

    /// <summary>
    /// Container of the awake meter
    /// </summary>
    [SerializeField] private GameObject _awake;

    /// <summary>
    /// Sleep meter
    /// </summary>
    [SerializeField] private Image _sleepFillImg;

    /// <summary>
    /// Timer meter
    /// </summary>
    [SerializeField] private Image _timerFillImg;

    /// <summary>
    /// Awake meter
    /// </summary>
    [SerializeField] private Image _awakeFillImg;

    #endregion

    #region Unity

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        GameManager.OnSleepValueChangedEvent += OnSleepValueChangedCallback;
        GameManager.OnTimerValueChangedEvent += OnTimerValueChangedCallback;
        GameManager.OnAwakeValueChangedEvent += OnAwakeValueChangedCallback;
    }

    void OnDestroy()
    {
        GameManager.OnSleepValueChangedEvent -= OnSleepValueChangedCallback;
        GameManager.OnTimerValueChangedEvent -= OnTimerValueChangedCallback;
        GameManager.OnAwakeValueChangedEvent -= OnAwakeValueChangedCallback;
    }

    private void Update()
    {
        // We only display the sleep and awake meters if we're in the editor for testing
        // WARNING : Make sure to disable them in the inspector before release!
        if (Application.isEditor)
        {
            if (GameManager.IsAwake)
            {
                _sleep.SetActive(false);
                _awake.SetActive(true);
            }
            else
            {
                _sleep.SetActive(true);
                _awake.SetActive(false);
            }
        }
    }

    #endregion

    #region Private methods

    /// <summary>
    /// Called when the awake meter's value changed
    /// </summary>
    private void OnAwakeValueChangedCallback(float curAwake, float mawAwake)
    {
        _awakeFillImg.fillAmount = curAwake / mawAwake;
    }

    /// <summary>
    /// Called when the sleep meter's value changed
    /// </summary>
    private void OnSleepValueChangedCallback(float curSleep, float maxSleep)
    {
        _sleepFillImg.fillAmount = curSleep / maxSleep;
    }

    /// <summary>
    /// Called when the timer meter's value changed
    /// </summary>
    private void OnTimerValueChangedCallback(float curTimer, float maxTimer)
    {
        _timerFillImg.fillAmount = curTimer / maxTimer;
    }

    #endregion

}
