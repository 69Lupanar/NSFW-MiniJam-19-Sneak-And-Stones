using UnityEngine;

/// <summary>
/// Receives the AnimEvent when the event key is reached
/// </summary>
public class AnimEventCallbackReceiver : MonoBehaviour
{
    #region Variables

    /// <summary>
    /// The animation manager
    /// </summary>
    public AnimationManager _animationManager;

    #endregion

    #region Public methods

    /// <summary>
    /// Called when a transition clip has ended
    /// </summary>
    public void OnTransitionClipEndedCallback()
    {
        _animationManager.OnTransitionClipEndedCallback();
    }

    /// <summary>
    /// Called when the awake clip has ended
    /// </summary>
    public void OnAwakeClipEndedCallback()
    {
        _animationManager.OnAwakeClipEndedCallback();
    }

    #endregion
}
