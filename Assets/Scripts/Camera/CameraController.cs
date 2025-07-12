using DG.Tweening;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private Camera _camera;
    [SerializeField] private Transform[] _waypoints;

    Transform _curWaypoint;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        GameManager.OnProgressChangedEvent += OnProgressChangedCallback;
        OnProgressChangedCallback(0);
    }

    private void OnProgressChangedCallback(int progress)
    {
        _curWaypoint = _waypoints[GameManager.Progress % _waypoints.Length];
        _camera.transform.DOMove(_curWaypoint.position, 1f);
        _camera.transform.DORotate(_curWaypoint.rotation.eulerAngles, 1f);
    }
}
