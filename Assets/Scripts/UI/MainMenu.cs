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

    #endregion

    #region Unity

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _menuAnim.Play(_startAnim.name);
    }

    #endregion

    #region Public methods

    /// <summary>
    /// Called by the Play button
    /// </summary>
    public void OnPlayBtn()
    {
        // TODO : SceneManager for fade-in transitions

        SceneManager.LoadSceneAsync(_gameSceneName, LoadSceneMode.Single);
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
