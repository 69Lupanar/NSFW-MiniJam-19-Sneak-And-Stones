using UnityEngine;

/// <summary>
/// Defines a removable item on the girls' body (cloth, jewelry)
/// </summary>
public abstract class RemovableItem : MonoBehaviour
{
    #region Variables

    /// <summary>
    /// The default position of the item
    /// </summary>
    public Vector3 StartPos { get; private set; }

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
