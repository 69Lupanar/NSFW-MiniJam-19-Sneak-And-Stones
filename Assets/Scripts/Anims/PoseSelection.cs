using System;
using UnityEngine;

/// <summary>
/// Combines the AnimationClips
/// for the pose and its transitions to other poses
/// </summary>
[Serializable]
public class PoseSelection
{
    #region Variables

    /// <summary>
    /// The AnimationClip for the pose
    /// </summary>
    public AnimationClip PoseClip => _poseClip;

    /// <summary>
    /// The AnimationClip for the pose
    /// </summary>
    [SerializeField] private AnimationClip _poseClip;

    /// <summary>
    /// The AnimationClip for the next available poses
    /// </summary>
    [SerializeField] private AnimationClip[] _nextPosesClips;

    /// <summary>
    /// The AnimationClip for the transitions to the next poses
    /// </summary>
    [SerializeField] private AnimationClip[] _transitionsClips;

    #endregion

    #region Public methods

    /// <summary>
    /// Retrieves a random pose and its corresponding transition
    /// </summary>
    /// <returns>Item1: The pose ; Item2 : The transition</returns>
    public (AnimationClip, AnimationClip) GetRandomNextTransitionAndNextPose()
    {
        int randIndex = UnityEngine.Random.Range(0, _nextPosesClips.Length);
        AnimationClip nextPose = _nextPosesClips[randIndex];
        AnimationClip transition = _transitionsClips[randIndex];

        return (nextPose, transition);
    }

    #endregion
}
