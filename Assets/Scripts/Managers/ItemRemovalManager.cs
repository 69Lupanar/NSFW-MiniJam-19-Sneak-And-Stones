using UnityEngine;

public class ItemRemovalManager : MonoBehaviour
{
    /// <summary>
    /// The items to remove per each level of progression
    /// </summary>
    [SerializeField] private RemovableItems[] _itemsToRemovePerProgressPoint;
}
