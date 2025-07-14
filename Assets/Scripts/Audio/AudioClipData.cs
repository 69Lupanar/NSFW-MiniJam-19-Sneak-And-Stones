using System;
using UnityEngine;

/// <summary>
/// Represents the data used to setup an AudioSource
/// </summary>
[Serializable]
public struct AudioClipData
{
    #region Variables 

    /// <summary>
    /// The clip to play
    /// </summary>
    public readonly AudioClip Clip => this._clip;

    /// <summary>
    /// The default volume level to play this clip at
    /// </summary>
    public readonly float DefaultVolume => this._defaultVolume;

    /// <summary>
    /// True if the clip must loop
    /// </summary>
    public readonly bool Loop => this._loop;

    /// <summary>
    /// The clip to play
    /// </summary>
    [SerializeField] private AudioClip _clip;

    /// <summary>
    /// The default volume level to play this clip at
    /// </summary>
    [SerializeField] private float _defaultVolume;

    /// <summary>
    /// True if the clip must loop
    /// </summary>
    [SerializeField] private bool _loop;

    #endregion

    #region Constructor

    /// <summary>
    /// Constructor
    /// </summary>
    public AudioClipData(AudioClip clip, float defaultVolume, bool loop)
    {
        this._clip = clip;
        this._defaultVolume = defaultVolume;
        this._loop = loop;
    }

    #endregion
}
