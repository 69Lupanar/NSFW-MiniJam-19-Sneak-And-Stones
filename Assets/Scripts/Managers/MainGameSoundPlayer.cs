using UnityEngine;

/// <summary>
/// Plays the requested sfxs during gameplay
/// </summary>
public class MainGameSoundPlayer : MonoBehaviour
{
    #region Variables

    /// <summary>
    /// Background music clip
    /// </summary>
    [SerializeField] private AudioClip _bgmClip;

    /// <summary>
    /// Played when a piece of jewerly is removed
    /// </summary>
    [SerializeField] private AudioClip _jewelryRemovedClip;

    /// <summary>
    /// Played when a piece of cloth is removed
    /// </summary>
    [SerializeField] private AudioClip _clothRemovedClip;

    /// <summary>
    /// Played when the player loses
    /// </summary>
    [SerializeField] private AudioClip _defeatClip;

    /// <summary>
    /// Played when the player wins
    /// </summary>
    [SerializeField] private AudioClip _victoryClip;

    /// <summary>
    /// Played when the monster shuffles around
    /// </summary>
    [SerializeField] private AudioClip _awakeClip;

    /// <summary>
    /// List of clips to be played randomly as warning 
    /// before the monster wakes up
    /// </summary>
    [SerializeField] private AudioClip[] _warningClips;
    private bool _isWarning;

    #endregion

    #region Unity

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        ItemRemovalManager.OnItemRemovedEvent += OnItemRemovedCallback;
        GameManager.OnVictoryEvent += OnVictoryCallback;
        GameManager.OnDefeatEvent += OnDefeatCallback;
        GameManager.OnEnterSleepEvent += OnEnterSleepCallback;
        GameManager.OnEnterAwakeEvent += OnEnterAwakeCallback;
        GameManager.OnSleepWarningEvent += OnSleepWarningCallback;

        AudioManager.Instance.StopAll();
        AudioManager.Instance.Play(_bgmClip);
    }

    private void OnDestroy()
    {
        ItemRemovalManager.OnItemRemovedEvent -= OnItemRemovedCallback;
        GameManager.OnVictoryEvent -= OnVictoryCallback;
        GameManager.OnDefeatEvent -= OnDefeatCallback;
        GameManager.OnEnterSleepEvent -= OnEnterSleepCallback;
        GameManager.OnEnterAwakeEvent -= OnEnterAwakeCallback;
        GameManager.OnSleepWarningEvent -= OnSleepWarningCallback;
    }

    #endregion

    #region Private methods

    /// <summary>
    /// Called when an item is removed
    /// </summary>
    private void OnItemRemovedCallback(RemovableItem item)
    {
        switch (item.Type)
        {
            case RemovableItemType.Cloth:
                AudioManager.Instance.Play(_clothRemovedClip);
                break;
            case RemovableItemType.Jewelry:
                AudioManager.Instance.Play(_jewelryRemovedClip);
                break;
            default:
                Debug.LogError($"Error in MainGameSoundPlayer: Cloth type \"{item.Type}\" has no associated sound effect.");
                break;
        }
    }

    /// <summary>
    /// Called when the player wins the game
    /// </summary>
    private void OnVictoryCallback()
    {
        AudioManager.Instance.Play(_victoryClip);
    }

    /// <summary>
    /// Called when the player loses the game
    /// </summary>
    private void OnDefeatCallback()
    {
        AudioManager.Instance.Play(_defeatClip);
    }

    /// <summary>
    /// Called when the monster enters the sleep phase
    /// </summary>
    private void OnEnterSleepCallback()
    {
        _isWarning = false;
    }

    /// <summary>
    /// Called when the monster enters the awake phase
    /// </summary>
    private void OnEnterAwakeCallback()
    {
        AudioManager.Instance.Play(_awakeClip);
    }

    /// <summary>
    /// Called when the monster is about to wake up
    /// </summary>
    private void OnSleepWarningCallback()
    {
        if (!_isWarning && _warningClips != null && _warningClips.Length > 0)
        {
            _isWarning = true;
            AudioManager.Instance.Play(_warningClips[UnityEngine.Random.Range(0, _warningClips.Length)]);
        }
    }

    #endregion
}
