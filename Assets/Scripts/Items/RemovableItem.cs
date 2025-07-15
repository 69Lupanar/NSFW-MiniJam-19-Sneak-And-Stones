using UnityEngine;

/// <summary>
/// Defines a removable item on the girls' body (cloth, jewelry)
/// </summary>
public abstract class RemovableItem : MonoBehaviour
{
    #region Variables

    /// <summary>
    /// The type of removable item on the monster's body
    /// </summary>
    public RemovableItemType Type => this._type;

    /// <summary>
    /// The default position of the item
    /// </summary>
    public Vector3 StartPos { get; private set; }

    /// <summary>
    /// The type of removable item on the monster's body
    /// </summary>
    [SerializeField] private RemovableItemType _type;

    #endregion

    #region Unity

    void Start()
    {
        StartPos = transform.position;
    }

    private void OnMouseDown()
    {
        ItemRemovalManager.OnItemClickedEvent?.Invoke(this);
    }

    private void OnMouseDrag()
    {
        ItemRemovalManager.OnItemDraggedEvent?.Invoke(this);
    }

    private void OnMouseUp()
    {
        ItemRemovalManager.OnItemReleasedEvent?.Invoke(this);
    }

    #endregion
}