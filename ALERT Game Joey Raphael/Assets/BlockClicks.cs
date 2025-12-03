using UnityEngine;
using UnityEngine.EventSystems;

/// <summary>
/// Detects when the pointer enters or exits a UI element and updates the
/// GridBuildingSystem so it knows whether clicking should affect the game world.
/// Prevents unwanted tile placement when interacting with UI.
/// </summary>
public class BlockClicks : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    /// <summary>
    /// Reference to the GridBuildingSystem so we can toggle whether the
    /// pointer is currently over a UI element.
    /// </summary>
    public GridBuildingSystem gridBuildingSystem;

    /// <summary>
    /// Called when the mouse or touch pointer enters this UI object's area.
    /// Sets the flag so world clicks are blocked.
    /// </summary>
    public void OnPointerEnter(PointerEventData eventData)
    {
        gridBuildingSystem.IsOverUIElement = true;
        Debug.Log("Hi!");
    }

    /// <summary>
    /// Called when the pointer exits this UI object.
    /// Re-enables interaction with the world.
    /// </summary>
    public void OnPointerExit(PointerEventData eventData)
    {
        gridBuildingSystem.IsOverUIElement = false;
        Debug.Log("Bye!");
    }

    /// <summary>
    /// Unity Start method. Automatically locates and caches the GridBuildingSystem
    /// reference on the object named "Grid".
    /// </summary>
    void Start()
    {
        gridBuildingSystem = GameObject.Find("Grid").GetComponent<GridBuildingSystem>();
    }

    // Unused update loop (kept for potential future use)
    void Update()
    {
    }
}
