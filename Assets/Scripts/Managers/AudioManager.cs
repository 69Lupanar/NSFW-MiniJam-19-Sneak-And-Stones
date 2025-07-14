using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Manages the sound effects and musics
/// </summary>
public class AudioManager : MonoBehaviour
{
    #region Variables

    /// <summary>
    /// The default volume for all newly create AudioClipDatas
    /// </summary>
    private const float DEFAULT_AUDIOCLIP_DATA_VOLUME = 0.5f;

    /// <summary>
    /// Singleton instance
    /// </summary>
    public static AudioManager Instance { get; private set; }

    /// <summary>
    /// The list of all clips to play in the game
    /// </summary>
    [SerializeField] private List<AudioClipData> _clips;

    /// <summary>
    /// The pool containing the inactive AudioSources
    /// </summary>
    List<AudioSource> _inactiveAudioSources;

    /// <summary>
    /// The pool containing the active AudioSources
    /// </summary>
    List<AudioSource> _activeAudioSources;

    Stack<int> _muteSourcesIndices = new();

    #endregion

    #region Unity

    private void Awake()
    {
        if (Instance != null)
        {
            DestroyImmediate(this);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(Instance.gameObject);
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _inactiveAudioSources = new List<AudioSource>(1);
        _activeAudioSources = new List<AudioSource>(1);
    }

    private void Update()
    {
        // If the game isn't focused, the source will stop playing
        // and will be removed accidentally

        if (!Application.isFocused)
        {
            return;
        }

        // Tracks all active AudioSources and disables the ones that have finished playing

        foreach (AudioSource source in _activeAudioSources)
        {
            if (!source.isPlaying)
            {
                source.Stop();
                source.clip = null;
                _muteSourcesIndices.Push(_activeAudioSources.IndexOf(source));
            }
        }

        while (_muteSourcesIndices.TryPop(out int index))
        {
            AudioSource source = _activeAudioSources[index];
            ReleaseAudioSource(source);
        }

        _muteSourcesIndices.Clear();
    }

    #endregion

    #region Public methods

    /// <summary>
    /// Plays an AudioClip, even if not registered in the AudioManager
    /// (UnityEvent version)
    /// </summary>
    public void Play(AudioClip clip)
    {
        Play(clip, null, null);
    }

    /// <summary>
    /// Plays an AudioClip, even if not registered in the AudioManager
    /// </summary>
    public void Play(AudioClip clip, float? volume = null, bool? loop = null)
    {
        bool contains = false;
        foreach (AudioClipData data in _clips)
        {
            if (data.Clip == clip)
            {
                contains = true;
                volume ??= data.DefaultVolume;
                loop ??= data.Loop;
                break;
            }
        }

        AudioClipData newData = new(clip,
                                    volume != null ? volume.Value : DEFAULT_AUDIOCLIP_DATA_VOLUME,
                                    loop != null && loop.Value);

        // If the manager doesn't contain the clip,
        // we add it for future usage

        if (!contains)
        {
            _clips.Add(newData);
        }

        AudioSource source = GetAudioSourceFromPool();
        source.clip = clip;
        source.volume = newData.DefaultVolume;
        source.loop = newData.Loop;
        source.Play();
    }

    /// <summary>
    /// Stops and disables an AudioSource
    /// </summary>
    public void Stop(AudioClip clip)
    {
        for (int i = _activeAudioSources.Count - 1; i >= 0; --i)
        {
            AudioSource source = _activeAudioSources[i];
            if (source.clip == clip)
            {
                source.Stop();
                source.clip = null;
                ReleaseAudioSource(source);
                break;
            }
        }
    }

    /// <summary>
    /// Stops all active AudioSources
    /// </summary>
    public void StopAll()
    {
        for (int i = _activeAudioSources.Count - 1; i >= 0; --i)
        {
            AudioSource source = _activeAudioSources[i];
            source.Stop();
            source.clip = null;
            ReleaseAudioSource(source);
        }
    }

    #endregion

    #region Private methods

    /// <summary>
    /// Gets a free AudioSource from the inactive pool
    /// or creates it if none are available
    /// </summary>
    private AudioSource GetAudioSourceFromPool()
    {
        AudioSource source;

        if (_inactiveAudioSources.Count > 0)
        {
            source = _inactiveAudioSources[0];
            _inactiveAudioSources.RemoveAt(0);
            source.enabled = true;
        }
        else
        {
            source = gameObject.AddComponent<AudioSource>();
            source.playOnAwake = false;
        }

        _activeAudioSources.Add(source);
        return source;
    }

    /// <summary>
    /// Disables an AudioSource and returns it to the inactive pool
    /// </summary>
    private void ReleaseAudioSource(AudioSource source)
    {
        _activeAudioSources.Remove(source);
        _inactiveAudioSources.Add(source);
        source.enabled = false;
    }

    #endregion
}
