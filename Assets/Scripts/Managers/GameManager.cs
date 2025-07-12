using System;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static Action OnSleepWarningEvent;
    public static Action OnVictoryEvent;
    public static Action OnDefeatEvent;
    public static Action<int> OnProgressValueChangedEvent;
    public static Action<float, float> OnSleepValueChangedEvent;
    public static Action<float, float> OnTimerValueChangedEvent;

    public static bool GameWon { get; private set; }

    public static bool GameOver { get; private set; }

    public static bool IsAwake { get; private set; }

    [SerializeField] private int _progressToWin = 1;

    [SerializeField] private Vector2 _minMaxSleepInterval = new(12f, 7f);

    [SerializeField] private float _randomSleepInterval = 2f;

    [SerializeField] private float _remainingSleepBeforeWarning = 1.5f;

    [SerializeField] private float _gameDuration = 120f;

    int _progress = 0;
    float _curRandomSleepLimit = 0f;
    float _curSleep = 0f;
    float _curTimer = 0f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        float rand = UnityEngine.Random.Range(-_randomSleepInterval, _randomSleepInterval);
        _curRandomSleepLimit = Mathf.Lerp(_minMaxSleepInterval.x, _minMaxSleepInterval.y, _curTimer / _gameDuration);
        _curRandomSleepLimit += rand;

        _progress = 0;
        GameWon = false;
        GameOver = false;
        IsAwake = false;
        OnProgressValueChangedEvent += OnProgressValueChangedCallback;
        OnSleepWarningEvent += OnSleepWarningCallback;
    }

    void OnDestroy()
    {
        OnProgressValueChangedEvent -= OnProgressValueChangedCallback;
        OnSleepWarningEvent -= OnSleepWarningCallback;
    }

    // Update is called once per frame
    void Update()
    {
        if (GameWon || GameOver)
        {
            return;
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            RiseProgress();
        }

        RiseSleep();
        RiseTimer();
    }

    private void RiseSleep()
    {
        if (_curSleep > _curRandomSleepLimit)
        {
            float rand = UnityEngine.Random.Range(-_randomSleepInterval, _randomSleepInterval);
            _curRandomSleepLimit = Mathf.Lerp(_minMaxSleepInterval.x, _minMaxSleepInterval.y, _curTimer / _gameDuration);
            _curRandomSleepLimit += rand;
            _curSleep = 0f;
            IsAwake = true;
        }

        _curSleep += Time.deltaTime;
        OnSleepValueChangedEvent?.Invoke(_curSleep, _curRandomSleepLimit);

        if (_curSleep > _curRandomSleepLimit - _remainingSleepBeforeWarning)
        {
            OnSleepWarningEvent?.Invoke();
        }
    }

    private void RiseTimer()
    {
        _curTimer += Time.deltaTime;
        OnTimerValueChangedEvent?.Invoke(_curTimer, _gameDuration);

        if (_curTimer > _gameDuration)
        {
            GameOver = true;
            OnDefeatEvent?.Invoke();
            print("You lose!");
        }
    }

    private void RiseProgress()
    {
        _progress++;
        OnProgressValueChangedEvent?.Invoke(_progress);
    }

    private void OnProgressValueChangedCallback(int progress)
    {
        print(progress);

        if (progress == _progressToWin)
        {
            GameWon = true;
            OnVictoryEvent?.Invoke();
            print("You win!");
        }
    }

    private void OnSleepWarningCallback()
    {
        print("Warning!");
    }

}
