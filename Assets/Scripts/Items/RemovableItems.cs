using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// List of items to remove for a given level of game progression.
/// Regroups both draggable items and clickable items
/// </summary>
[Serializable]
public class RemovableItems
{
    /// <summary>
    /// The items to remove. They will be removed in the order
    /// they have been assigned in the inspector
    /// </summary>
    public List<RemovableItem> Items => this._items;

    /// <summary>
    /// The items to remove. They will be removed in the order
    /// they have been assigned in the inspector
    /// </summary>
    [SerializeField] private List<RemovableItem> _items;
}
