using UnityEngine;

/// <summary>
/// Manages the monster's animations
/// </summary>
public class AnimationManager : MonoBehaviour
{
    #region Variables

    /// <summary>
    /// True if the character is currently switching poses
    /// </summary>
    public static bool IsTransitioning { get; private set; }

    /// <summary>
    /// Monster's animator
    /// </summary>
    [SerializeField] private Animator _characterAnim;

    /// <summary>
    /// Monster's default idle animation at the start of the game
    /// </summary>
    [SerializeField] private AnimationClip _defaultPose;

    /// <summary>
    /// Monster's warning animation before waking up
    /// </summary>
    [SerializeField] private AnimationClip _warningClip;

    /// <summary>
    /// Monster's awake animation
    /// </summary>
    [SerializeField] private AnimationClip _awakeClip;

    /// <summary>
    /// Groups each transition for each pose
    /// </summary>
    [SerializeField] private PoseSelection[] _posesSelections;

    /// <summary>
    /// The current idle animation
    /// </summary>
    AnimationClip _curPose;

    /// <summary>
    /// The game manager
    /// </summary>
    GameManager _gameManager;

    /// <summary>
    /// The next pose and transition selected by the manager
    /// </summary>
    (AnimationClip nextPose, AnimationClip transition) _nextPoseAndTransition;

    /// <summary>
    /// true if the animation is currently in the warning phase
    /// </summary>
    bool _isWarning = false;

    #endregion

    #region Unity

    private void Start()
    {
        _gameManager = GetComponent<GameManager>();
        _isWarning = false;
        _curPose = _defaultPose;
        AnimationManager.IsTransitioning = false;
        GameManager.OnSleepWarningEvent += OnSleepWarningCallback;
        GameManager.OnEnterAwakeEvent += OnEnterAwakeCallback;
        //GameManager.OnEnterSleepEvent += OnEnterSleepCallback;
    }

    private void OnDestroy()
    {
        GameManager.OnSleepWarningEvent -= OnSleepWarningCallback;
        GameManager.OnEnterAwakeEvent -= OnEnterAwakeCallback;
        //GameManager.OnEnterSleepEvent -= OnEnterSleepCallback;
    }

    #endregion

    #region Public methods

    /// <summary>
    /// Called when a transition ends
    /// </summary>
    public void OnTransitionClipEndedCallback()
    {
        // The idle pose is automatically played after the transition

        _curPose = _nextPoseAndTransition.nextPose;

        // Since all transitions have different durations, we just 
        // return to the sleep phase once they're done

        _gameManager.ForceSleep();
    }

    /// <summary>
    /// Called when the awake clip ended
    /// </summary>
    public void OnAwakeClipEndedCallback()
    {
        // Play the previously registered pose after the transition

        _characterAnim.Play(_curPose.name);
    }

    #endregion

    #region Private methods

    /// <summary>
    /// Called by the warning before awake
    /// </summary>
    private void OnSleepWarningCallback()
    {
        // TODO : Play warning anim

        if (!_isWarning)
        {
            _isWarning = true;
            _characterAnim.Play(_warningClip.name);
        }
    }

    ///// <summary>
    ///// Called when the monster enters the sleep phase
    ///// </summary>
    //private void OnEnterSleepCallback()
    //{
    //    // TODO : Play idle pose
    //}

    /// <summary>
    /// Called when the monster enters the awake phase
    /// </summary>
    private void OnEnterAwakeCallback()
    {
        _isWarning = false;
        int rand = UnityEngine.Random.Range(0, 2);

        // We switch randomly between playing an animation
        // and just shuffling around in the bed

        if (rand == 0)
        {
            _characterAnim.Play(_awakeClip.name);
        }
        else
        {
            // Play transition

            PoseSelection selection = null;

            foreach (PoseSelection ps in _posesSelections)
            {
                if (ps.PoseClip == _curPose)
                {
                    selection = ps;
                    break;
                }
            }

            if (selection == null)
            {
                print($"Error : the pose \'{_curPose.name}\' isn't referenced in the PoseSelections.");
                return;
            }

            _nextPoseAndTransition = selection.GetRandomNextTransitionAndNextPose();
            _characterAnim.Play(_nextPoseAndTransition.transition.name);
        }
    }

    #endregion
}
