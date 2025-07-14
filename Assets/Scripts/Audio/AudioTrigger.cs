using UnityEngine;

/// <summary>
/// Allows calls to the AudioManager from an UnityEvent
/// without losing references when the scene changes
/// </summary>
public class AudioTrigger : MonoBehaviour
{
    #region Public methods

    /// <summary>
    /// Plays the requested clip (UnityEvent version)
    /// </summary>
    public void Play(AudioClip clip)
    {
        AudioManager.Instance.Play(clip);
    }

    #endregion
}
