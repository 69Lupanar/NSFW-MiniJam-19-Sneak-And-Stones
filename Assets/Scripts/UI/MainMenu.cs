using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Handles the main menu actions and transitions
/// </summary>
public class MainMenu : MonoBehaviour
{
    #region Variables

    /// <summary>
    /// Animator for the main menu
    /// </summary>
    [SerializeField] private Animator _menuAnim;

    /// <summary>
    /// AnimationClips to play
    /// </summary>
    [SerializeField] private AnimationClip _startAnim, _toHowToAnim, _toMainAnim;

    /// <summary>
    /// Game scene to load on play
    /// </summary>
    [SerializeField] private string _gameSceneName;

    /// <summary>
    /// Background music clip
    /// </summary>
    [SerializeField] private AudioClip _bgmClip;

    /// <summary>
    /// Manages the transitions between scenes
    /// </summary>
    SceneFader _sceneFader;

    #endregion

    #region Unity

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        AudioManager.Instance.StopAll();
        AudioManager.Instance.Play(_bgmClip);
        _menuAnim.Play(_startAnim.name);
        _sceneFader = FindAnyObjectByType<SceneFader>();
    }

    #endregion

    #region Public methods

    /// <summary>
    /// Called by the Play button
    /// </summary>
    public void OnPlayBtn()
    {
        _sceneFader.FadeOut(1f, _sceneFader.DefaultFadeOutGradient, () => SceneManager.LoadSceneAsync(_gameSceneName, LoadSceneMode.Single));
    }

    /// <summary>
    /// Called by the HowTo button
    /// </summary>
    public void OnHowToBtn()
    {
        _menuAnim.Play(_toHowToAnim.name);
    }

    /// <summary>
    /// Called by the GotIt button
    /// </summary>
    public void OnGotItBtn()
    {
        _menuAnim.Play(_toMainAnim.name);
    }

    ///// <summary>
    ///// Called by the Quit button
    ///// </summary>
    //public void OnQuitBtn()
    //{
    //    Application.Quit();
    //}

    #endregion
}
