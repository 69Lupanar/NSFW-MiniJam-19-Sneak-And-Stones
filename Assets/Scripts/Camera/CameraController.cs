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
        GameManager.OnProgressValueChangedEvent += OnProgressValueChangedCallback;
        OnProgressValueChangedCallback(0);
    }

    void OnDestroy()
    {
        GameManager.OnProgressValueChangedEvent -= OnProgressValueChangedCallback;
    }

    private void OnProgressValueChangedCallback(int progress)
    {
        _curWaypoint = _waypoints[progress % _waypoints.Length];
        _camera.transform.DOMove(_curWaypoint.position, 1f);
        _camera.transform.DORotate(_curWaypoint.rotation.eulerAngles, 1f);
    }
}
