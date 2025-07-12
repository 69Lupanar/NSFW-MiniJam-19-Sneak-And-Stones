using System;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static Action<int> OnProgressChangedEvent;

    public static int Progress { get; private set; }

    [SerializeField] private int ProgressToWin = 1;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Progress = 0;
        OnProgressChangedEvent += OnProgressChangedCallback;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && Progress < ProgressToWin)
        {
            RiseProgress();
        }
    }

    private void RiseProgress()
    {
        Progress++;
        OnProgressChangedEvent?.Invoke(Progress);
    }

    private void OnProgressChangedCallback(int progress)
    {
        print(progress);

        if (progress == ProgressToWin)
        {
            print("You win!");
        }
    }
}
