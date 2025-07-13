using System;
using UnityEngine;

public class ItemRemovalManager : MonoBehaviour
{
    #region Events

    /// <summary>
    /// Called when the player clicks on a removable item
    /// </summary>
    public static Action<RemovableItem> OnItemClickedEvent;

    /// <summary>
    /// Called when the player drags a removable item
    /// </summary>
    public static Action<RemovableItem> OnItemDraggedEvent;

    /// <summary>
    /// Called when the player releases a removable item
    /// </summary>
    public static Action<RemovableItem> OnItemReleasedEvent;

    #endregion

    #region Variables

    /// <summary>
    /// The sleep speed factor to apply when the player drags an item
    /// </summary>
    [SerializeField] private float _sleepSpeedFactorOnDrag = 2f;

    /// <summary>
    /// The distance before a dragged object is considered removed
    /// </summary>
    [SerializeField] private float _itemDragMaxDistance = 2f;

    /// <summary>
    /// The main Camera
    /// </summary>
    [SerializeField] private Camera _mainCamera;

    /// <summary>
    /// The items to remove per each level of progression
    /// </summary>
    [SerializeField] private ItemPile[] _itemPiles;

    int _curPileIndex = 0;
    RemovableItem _curRemovedItem;

    /// <summary>
    /// Offset to keep the item correctly placed when dragged
    /// </summary>
    Vector3 _mousePosOffset;

    /// <summary>
    /// Manages the player's progression
    /// </summary>
    GameManager _gameManager;

    #endregion

    #region Unity

    private void Start()
    {
        _gameManager = GetComponent<GameManager>();
        _curPileIndex = 0;

        ItemRemovalManager.OnItemClickedEvent += OnItemClickedCallback;
        ItemRemovalManager.OnItemDraggedEvent += OnItemDraggedCallback;
        ItemRemovalManager.OnItemReleasedEvent += OnItemReleasedCallback;
    }

    private void OnDestroy()
    {

        ItemRemovalManager.OnItemClickedEvent -= OnItemClickedCallback;
        ItemRemovalManager.OnItemDraggedEvent -= OnItemDraggedCallback;
        ItemRemovalManager.OnItemReleasedEvent -= OnItemReleasedCallback;
    }

    #endregion

    #region Private methods

    /// <summary>
    /// Called when the player clicks on a removable item
    /// </summary>
    private void OnItemClickedCallback(RemovableItem item)
    {
        if (GameManager.IsAwake)
        {
            _gameManager.LoseGame();
            return;
        }

        for (int i = 0; i < _itemPiles.Length; ++i)
        {
            ItemPile pile = _itemPiles[i];

            if (pile.Items.Contains(item))
            {
                switch (item)
                {
                    case RemovableItemOnClick:
                        if (ItemIsOnTopOfPile(pile, item))
                        {
                            RemoveItemFromPile(pile, item);

                            // If the pile is then empty, that body part is completely exposed

                            if (pile.Items.Count == 0)
                            {
                                _gameManager.RiseProgress();
                            }
                        }
                        return;

                    case RemovableItemOnDrag:

                        if (ItemIsOnTopOfPile(pile, item))
                        {
                            // Accelerates the game while the player holds and item
                            _gameManager.SetSleepSpeedFactor(_sleepSpeedFactorOnDrag);

                            _mousePosOffset = item.transform.position - GetMouseWorldPos(item);
                            _curPileIndex = i;
                            _curRemovedItem = item;
                        }
                        return;

                    default:
                        return;
                }
            }
        }

    }

    /// <summary>
    /// Gets the mouse position in world space
    /// </summary>
    private Vector3 GetMouseWorldPos(RemovableItem item)
    {
        // Pixel coordinates of mouse (x,y)

        Vector3 mousePoint = Input.mousePosition;


        // z coordinate of game object on screen

        mousePoint.z = _mainCamera.WorldToScreenPoint(item.transform.position).z;


        // Convert it to world points

        return Camera.main.ScreenToWorldPoint(mousePoint);
    }

    /// <summary>
    /// Called when the player drags a removable item
    /// </summary>
    private void OnItemDraggedCallback(RemovableItem item)
    {
        if (GameManager.IsAwake)
        {
            _gameManager.LoseGame();
            return;
        }

        if (_curRemovedItem != null)
        {
            item.transform.position = GetMouseWorldPos(item) + _mousePosOffset;

            if (Vector3.Distance(item.transform.position, item.StartPos) > _itemDragMaxDistance)
            {
                ItemPile pile = _itemPiles[_curPileIndex];
                RemoveItemFromPile(pile, item);

                if (pile.Items.Count == 0)
                {
                    _gameManager.RiseProgress();
                    OnItemReleasedCallback(item);
                }
            }
        }
    }

    /// <summary>
    /// Called when the player releases a removable item
    /// </summary>
    private void OnItemReleasedCallback(RemovableItem item)
    {
        if (_curRemovedItem != null)
        {
            _curRemovedItem.transform.position = _curRemovedItem.StartPos;
        }

        _curRemovedItem = null;

        // Resets sleep speed
        _gameManager.ResetSleepSpeedFactor();

    }

    /// <summary>
    /// Indicates if the item is the first item on top
    /// </summary>
    /// <param name="pile">The item pile</param>
    /// <param name="item">The item to remove</param>
    private bool ItemIsOnTopOfPile(ItemPile pile, RemovableItem item)
    {
        if (pile.Items.Count == 0)
        {
            return false;
        }

        return pile.Items[0] == item;
    }


    /// <summary>
    /// Removes the item from the pile if it's the first item on top
    /// </summary>
    /// <param name="pile">The item pile</param>
    /// <param name="item">The item to remove</param>
    private void RemoveItemFromPile(ItemPile pile, RemovableItem item)
    {
        pile.Items.RemoveAt(0);
        Destroy(item.gameObject);
    }

    #endregion
}
