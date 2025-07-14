using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Manages the game over screen
/// </summary>
public class GameOverMenu : MonoBehaviour
{
    #region Variables

    /// <summary>
    /// Game scene to load on play
    /// </summary>
    [SerializeField] private string _mainMenuName;

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
    }

    #endregion

    #region Public methods

    /// <summary>
    /// Called by the TruAgain button
    /// </summary>
    public void OnTryAgainBtn()
    {
        // TODO : SceneManager for fade-in transitions

        _sceneFader.FadeOut(1f, _sceneFader.DefaultFadeOutGradient, () => SceneManager.LoadSceneAsync(_mainMenuName, LoadSceneMode.Single));
    }

    #endregion
}
