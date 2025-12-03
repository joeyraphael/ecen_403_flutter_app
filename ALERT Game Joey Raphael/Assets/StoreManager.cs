using System;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Manages the in-game store UI and purchasing of new buildings.
/// Handles item selection, cost display, and spawning purchased buildings
/// into the GridBuildingSystem for placement.
/// </summary>
public class StoreManager : MonoBehaviour
{
    // Buttons for store interactions
    [SerializeField] Button BuyGeneratorButton;
    [SerializeField] Button BuyPowerPlantButton;
    [SerializeField] Button BuyButton;

    // UI text elements showing details of the currently selected item
    [SerializeField] GameObject ItemNameText;
    [SerializeField] GameObject ItemCostText;
    [SerializeField] GameObject ItemDescriptionText;

    // Store canvas to show/hide the store UI
    [SerializeField] Canvas StoreCanvas;

    // Reference to the main grid building system
    GridBuildingSystem gridBuildingSystem;

    // The building prefab currently selected in the store
    [SerializeField] GameObject purchasedSelectedBuilding;

    // Prefabs available for purchase
    [SerializeField] GameObject GreenGenerator;
    [SerializeField] GameObject PowerPlant;

    // Button to close the store UI
    [SerializeField] Button CloseStoreButton;

    // Reference to player manager (for money checks)
    PlayerManager playerManager;

    /// <summary>
    /// Attempts to purchase the given building and prepare it for placement.
    /// Deducts cost, creates a temp building in the GridBuildingSystem, and closes the store.
    /// </summary>
    /// <param name="purchasedSelectedBuilding">The building prefab the player chose to buy.</param>
    public void BuyBuilding(GameObject purchasedSelectedBuilding)
    {
        // No item selected
        if (purchasedSelectedBuilding == null)
        {
            GameObject.Find("CoreGameCanvas")
                .GetComponent<UI_Manager>()
                .SendNotification("Didn't purchase anything!", "alert", Color.red);

            StoreCanvas.gameObject.SetActive(false);
            return;
        }

        Debug.Log("I ran");

        // Check if the player has enough money
        if (playerManager.money < purchasedSelectedBuilding.GetComponent<Building>().cost)
        {
            Debug.Log("Not enough money!");
            purchasedSelectedBuilding = null;

            GameObject.Find("CoreGameCanvas")
                .GetComponent<UI_Manager>()
                .SendNotification("Not enough money!", "alert", Color.red);

            StoreCanvas.gameObject.SetActive(false);
            return;
        }

        // Enter placing mode and create a temporary building instance
        gridBuildingSystem.currentActionType = GridBuildingSystem.CurrentActionType.Placing;
        gridBuildingSystem.temp = Instantiate(purchasedSelectedBuilding.GetComponent<Building>());

        // Give the new building a unique-ish name
        gridBuildingSystem.temp.name =
            gridBuildingSystem.temp.name + UnityEngine.Random.Range(5, 5000).ToString();

        // Deduct money from the player
        playerManager.AddMoney(-purchasedSelectedBuilding.GetComponent<Building>().cost);

        // Track this building in the main list
        gridBuildingSystem.ListOfBuildings.Add(gridBuildingSystem.temp.GetComponent<Building>());

        // Close the store and reset wiring mode
        StoreCanvas.gameObject.SetActive(false);
        gridBuildingSystem.wiringModeOn = 0;
    }

    /// <summary>
    /// Selects the Generator as the item to purchase and updates the UI details.
    /// </summary>
    public void SelectGenerator()
    {
        purchasedSelectedBuilding = GreenGenerator;
        ItemCostText.GetComponent<TextMeshProUGUI>().text =
            "$" + GreenGenerator.GetComponent<Building>().cost.ToString();
        ItemNameText.GetComponent<TextMeshProUGUI>().text = "Generator";
        ItemDescriptionText.GetComponent<TextMeshProUGUI>().text = "It's just a regular generator.";
    }

    /// <summary>
    /// Selects the Power Plant as the item to purchase and updates the UI details.
    /// </summary>
    public void SelectPowerPlant()
    {
        purchasedSelectedBuilding = PowerPlant;
        ItemCostText.GetComponent<TextMeshProUGUI>().text =
            "$" + PowerPlant.GetComponent<Building>().cost.ToString();
        ItemNameText.GetComponent<TextMeshProUGUI>().text = "Power Plant";
        ItemDescriptionText.GetComponent<TextMeshProUGUI>().text = "Supports up to 5 connections.";
    }

    /// <summary>
    /// Closes the store UI without purchasing anything.
    /// </summary>
    public void CloseStoreMenu()
    {
        StoreCanvas.gameObject.SetActive(false);
    }

    /// <summary>
    /// Unity Start method.
    /// Wires up button listeners and grabs references to core managers.
    /// </summary>
    void Start()
    {
        playerManager = GameObject.Find("Player").GetComponent<PlayerManager>();
        gridBuildingSystem = GameObject.Find("Grid").GetComponent<GridBuildingSystem>();

        // Hook up selection buttons
        BuyGeneratorButton.onClick.AddListener(SelectGenerator);
        BuyPowerPlantButton.onClick.AddListener(SelectPowerPlant);

        // Hook up close button
        CloseStoreButton.onClick.AddListener(CloseStoreMenu);

        // Buy button triggers purchase of whatever is currently selected
        BuyButton.onClick.AddListener(delegate { BuyBuilding(purchasedSelectedBuilding); });
    }

    // Currently unused, but available for future store logic
    void Update()
    {
    }
}
