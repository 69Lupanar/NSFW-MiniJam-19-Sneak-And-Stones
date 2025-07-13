using DG.Tweening;
using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// Manages the player's camera
/// </summary>
public class CameraController : MonoBehaviour
{
    #region Variables

    /// <summary>
    /// The camera
    /// </summary>
    [SerializeField] private Camera _camera;

    /// <summary>
    ///  The list of waypoints to place the camera at
    /// </summary>
    [SerializeField] private Transform[] _waypoints;

    /// <summary>
    ///  The current waypoint
    /// </summary>
    Transform _curWaypoint;

    /// <summary>
    ///  The current waypoint index
    /// </summary>
    int _curWaypointIndex;

    /// <summary>
    /// Manages the player's input
    /// </summary>
    InputAction _previousAngleAction;

    /// <summary>
    /// Manages the player's input
    /// </summary>
    InputAction _nextAngleAction;

    #endregion

    #region Unity

    void Start()
    {
        // GameManager.OnProgressValueChangedEvent += OnProgressValueChangedCallback;
        //OnProgressValueChangedCallback(0);

        _previousAngleAction = InputSystem.actions.FindAction("PreviousAngle");
        _nextAngleAction = InputSystem.actions.FindAction("NextAngle");

        _curWaypointIndex = 0;
        _curWaypoint = _waypoints[_curWaypointIndex];
        MoveCameraToWaypoint();
    }

    //void OnDestroy()
    //{
    //    //GameManager.OnProgressValueChangedEvent -= OnProgressValueChangedCallback;
    //}

    //private void OnProgressValueChangedCallback(int progress)
    //{
    //    _curWaypointIndex = progress % _waypoints.Length;
    //    _curWaypoint = _waypoints[_curWaypointIndex];
    //    MoveCameraToWaypoint();
    //}

    private void Update()
    {
        if (_previousAngleAction.WasPressedThisFrame())
        {
            OnPreviousAngleAction();
        }

        if (_nextAngleAction.WasPressedThisFrame())
        {
            OnNextAngleAction();
        }
    }

    #endregion

    #region Public methods

    /// <summary>
    /// Called by the button 'Previous Angle'
    /// </summary>
    public void OnPreviousAngleAction()
    {
        SetCurWaypoint(-1);
        MoveCameraToWaypoint();
    }

    /// <summary>
    /// Called by the button 'Next Angle'
    /// </summary>
    public void OnNextAngleAction()
    {
        SetCurWaypoint(1);
        MoveCameraToWaypoint();
    }

    #endregion

    #region Private methods

    /// <summary>
    /// Sets the current waypoint
    /// </summary>
    /// <param name="increment">Increases to get to the next waypoint, decreases for the previous one</param>
    private void SetCurWaypoint(int increment)
    {
        _curWaypointIndex += increment;

        if (_curWaypointIndex < 0)
        {
            _curWaypointIndex = _waypoints.Length - 1;
        }

        _curWaypointIndex %= _waypoints.Length;
        _curWaypoint = _waypoints[_curWaypointIndex];
    }

    /// <summary>
    /// Uses DOTween to move the camera
    /// </summary>
    private void MoveCameraToWaypoint()
    {
        _camera.transform.DOMove(_curWaypoint.position, 1f);
        _camera.transform.DORotate(_curWaypoint.rotation.eulerAngles, 1f);
    }

    #endregion
}
