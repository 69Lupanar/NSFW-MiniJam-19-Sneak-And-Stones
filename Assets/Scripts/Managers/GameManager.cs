using System;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Manages the game state & player progression
/// </summary>
public class GameManager : MonoBehaviour
{
    #region Events

    /// <summary>
    /// Warning before awake
    /// </summary>
    public static Action OnSleepWarningEvent;

    /// <summary>
    /// Called when the monster enters the sleep phase
    /// </summary>
    public static Action OnEnterSleepEvent;

    /// <summary>
    /// Called when the monster enters the awake phase
    /// </summary>
    public static Action OnEnterAwakeEvent;

    /// <summary>
    /// When the player wins
    /// </summary>
    public static Action OnVictoryEvent;

    /// <summary>
    /// When the player loses
    /// </summary>
    public static Action OnDefeatEvent;

    /// <summary>
    /// When the players removes all items on a specific body part
    /// </summary>
    public static Action<int> OnProgressValueChangedEvent;

    /// <summary>
    /// Progression of the sleep meter
    /// </summary>
    public static Action<float, float> OnSleepValueChangedEvent;

    /// <summary>
    /// Progression of the timer meter
    /// </summary>
    public static Action<float, float> OnTimerValueChangedEvent;

    /// <summary>
    /// Progression of the awake meter
    /// </summary>
    public static Action<float, float> OnAwakeValueChangedEvent;

    #endregion

    #region Variables

    /// <summary>
    /// true if the player wins
    /// </summary>
    public static bool GameWon { get; private set; }

    /// <summary>
    /// true if the player loses
    /// </summary>
    public static bool GameOver { get; private set; }

    /// <summary>
    /// true if the monster is awake
    /// </summary>
    public static bool IsAwake { get; private set; }

    /// <summary>
    /// Scene to load on defeat
    /// </summary>
    [SerializeField] private string _gameOverSceneName;

    /// <summary>
    /// Scene to load on victory
    /// </summary>
    [SerializeField] private string _victorySceneName;

    /// <summary>
    /// The number of body parts to expose to win
    /// </summary>
    [SerializeField] private int _progressToWin = 6;

    /// <summary>
    /// The min and max amount of time the monster is asleep.
    /// The longer the game gets, the faster the monster will wake up.
    /// </summary>
    [SerializeField] private Vector2 _minMaxSleepInterval = new(12f, 7f);

    /// <summary>
    /// Offset to keep the sleep interval unpredictable
    /// </summary>
    [SerializeField] private float _randomSleepDurationOffset = 2f;

    /// <summary>
    /// The remaining amount of time before the game warns the player
    /// the monster is about to wake up
    /// </summary>
    [SerializeField] private float _remainingSleepBeforeWarning = 1.5f;

    /// <summary>
    /// Duration of a session
    /// </summary>
    [SerializeField] private float _gameDuration = 120f;

    /// <summary>
    /// The amount of time the monster stays awake
    /// </summary>
    [SerializeField] private float _awakeDuration = 3f;

    // Variables modified during a game session

    int _progress = 0;
    float _curRandomSleepLimit = 0f;
    float _curSleep = 0f;
    float _curTimer = 0f;
    float _curAwake = 0f;

    /// <summary>
    /// Allows to increase the sleep meter's speed
    /// if the player removes an item or touches the monster
    /// </summary>
    float _sleepSpeedFactor = 1f;

    /// <summary>
    /// Manages the transitions between scenes
    /// </summary>
    SceneFader _sceneFader;

    #endregion

    #region Unity

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _sceneFader = FindAnyObjectByType<SceneFader>();

        float rand = UnityEngine.Random.Range(-_randomSleepDurationOffset, _randomSleepDurationOffset);
        _curRandomSleepLimit = Mathf.Lerp(_minMaxSleepInterval.x, _minMaxSleepInterval.y, _curTimer / _gameDuration);
        _curRandomSleepLimit += rand;

        _sleepSpeedFactor = 1f;
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

        //if (Input.GetKeyDown(KeyCode.Space))
        //{
        //    RiseProgress();
        //}

        if (IsAwake)
        {
            RiseAwake();
        }
        else
        {
            RiseSleep();
        }

        RiseTimer();
    }

    #endregion

    #region Public methods

    /// <summary>
    /// Increments the progress meter
    /// </summary>
    public void RiseProgress()
    {
        _progress++;
        OnProgressValueChangedEvent?.Invoke(_progress);
    }

    /// <summary>
    /// Set the sleep progression speed
    /// </summary>
    public void SetSleepSpeedFactor(float value)
    {
        this._sleepSpeedFactor = value;
    }

    /// <summary>
    /// Resets the sleep progression speed to 1
    /// </summary>
    public void ResetSleepSpeedFactor()
    {
        this._sleepSpeedFactor = 1f;
    }

    /// <summary>
    /// Manages the player's defeat
    /// </summary>
    public void LoseGame()
    {
        if (!GameOver)
        {
            GameOver = true;
            OnDefeatEvent?.Invoke();
            print("You lose!");
            _sceneFader.FadeOut(1f, _sceneFader.DefeatFadeOutGradient, () => SceneManager.LoadSceneAsync(_gameOverSceneName, LoadSceneMode.Single));
        }
    }

    /// <summary>
    /// Returns to the sleep phase
    /// </summary>
    public void ForceSleep()
    {
        IsAwake = false;
        OnEnterSleepEvent?.Invoke();
    }

    #endregion

    #region Private methods

    /// <summary>
    /// Manages the player's victory
    /// </summary>
    private void WinGame()
    {
        if (!GameWon)
        {
            GameWon = true;
            OnVictoryEvent?.Invoke();
            print("You win!");
            _sceneFader.FadeOut(1f, _sceneFader.VictoryFadeOutGradient, () => SceneManager.LoadSceneAsync(_victorySceneName, LoadSceneMode.Single));
        }
    }

    /// <summary>
    /// Fills the awake meter
    /// </summary>
    private void RiseAwake()
    {
        // After being awake for a while, the monster goes back to sleep

        if (_curAwake > _awakeDuration)
        {
            IsAwake = false;
            _curAwake = 0f;
            OnEnterSleepEvent?.Invoke();
        }

        _curAwake += Time.deltaTime;
        OnAwakeValueChangedEvent?.Invoke(_curAwake, _awakeDuration);
    }

    /// <summary>
    /// Fills the sleep meter
    /// </summary>
    private void RiseSleep()
    {
        // At the end of her sleep, the monster wakes up

        if (_curSleep > _curRandomSleepLimit)
        {
            float rand = UnityEngine.Random.Range(-_randomSleepDurationOffset, _randomSleepDurationOffset);
            _curRandomSleepLimit = Mathf.Lerp(_minMaxSleepInterval.x, _minMaxSleepInterval.y, _curTimer / _gameDuration);
            _curRandomSleepLimit += rand;
            _curSleep = 0f;
            IsAwake = true;
            OnEnterAwakeEvent?.Invoke();
        }

        _curSleep += Time.deltaTime * _sleepSpeedFactor;
        OnSleepValueChangedEvent?.Invoke(_curSleep, _curRandomSleepLimit);

        if (_curSleep > _curRandomSleepLimit - _remainingSleepBeforeWarning)
        {
            OnSleepWarningEvent?.Invoke();
        }
    }

    /// <summary>
    /// Fills the timer meter
    /// </summary>
    private void RiseTimer()
    {
        _curTimer += Time.deltaTime;
        OnTimerValueChangedEvent?.Invoke(_curTimer, _gameDuration);

        // If the timer reaches 0, we lose

        if (_curTimer > _gameDuration)
        {
            LoseGame();
        }
    }

    /// <summary>
    /// Tracks the player progress
    /// and notifies if the player wins
    /// </summary>
    /// <param name="progress"></param>
    private void OnProgressValueChangedCallback(int progress)
    {
        print(progress);

        // If all body parts are exposed, we win

        if (progress == _progressToWin)
        {
            WinGame();
        }
    }

    /// <summary>
    /// Triggers a warning before the monster wakes up
    /// </summary>
    private void OnSleepWarningCallback()
    {
        print("Warning!");
    }

    #endregion
}
