using System;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class StoreManager : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    [SerializeField] Button BuyGeneratorButton;
    [SerializeField] Button BuyPowerPlantButton;
    [SerializeField] Button BuyButton;
    [SerializeField] GameObject ItemNameText;
    [SerializeField] GameObject ItemCostText;
    [SerializeField] GameObject ItemDescriptionText;
    [SerializeField] Canvas StoreCanvas;
    GridBuildingSystem gridBuildingSystem;

    [SerializeField] GameObject purchasedSelectedBuilding;
    [SerializeField] GameObject GreenGenerator;
    [SerializeField] GameObject PowerPlant;
    [SerializeField] Button CloseStoreButton;

    PlayerManager playerManager;

    public void BuyBuilding(GameObject purchasedSelectedBuilding)
    {
        if (purchasedSelectedBuilding == null)
        {
            GameObject.Find("CoreGameCanvas").GetComponent<UI_Manager>().SendNotification("Didn't purchase anything!", "alert", Color.red);
            StoreCanvas.gameObject.SetActive(false);
            return;
        }
        Debug.Log("I ran");
        if (playerManager.money < purchasedSelectedBuilding.GetComponent<Building>().cost)
        {
            Debug.Log("Not enough money!");
            purchasedSelectedBuilding = null;
            GameObject.Find("CoreGameCanvas").GetComponent<UI_Manager>().SendNotification("Not enough money!", "alert", Color.red);
            StoreCanvas.gameObject.SetActive(false);
            return;
        }
        gridBuildingSystem.currentActionType = GridBuildingSystem.CurrentActionType.Placing;
        gridBuildingSystem.temp = Instantiate(purchasedSelectedBuilding.GetComponent<Building>());
        gridBuildingSystem.temp.name = gridBuildingSystem.temp.name + UnityEngine.Random.Range(5, 5000).ToString();
        playerManager.AddMoney(-purchasedSelectedBuilding.GetComponent<Building>().cost);
        gridBuildingSystem.ListOfBuildings.Add(gridBuildingSystem.temp.GetComponent<Building>());
        StoreCanvas.gameObject.SetActive(false);
        gridBuildingSystem.wiringModeOn = 0;
    }
    public void SelectGenerator()
    {
        purchasedSelectedBuilding = GreenGenerator;
        ItemCostText.GetComponent<TextMeshProUGUI>().text = "$" + GreenGenerator.GetComponent<Building>().cost.ToString();
        ItemNameText.GetComponent<TextMeshProUGUI>().text = "Generator";
        ItemDescriptionText.GetComponent<TextMeshProUGUI>().text = "It's just a regular generator.";
    }
    public void SelectPowerPlant()
    {
        purchasedSelectedBuilding = PowerPlant;
        ItemCostText.GetComponent<TextMeshProUGUI>().text = "$" + PowerPlant.GetComponent<Building>().cost.ToString();
        ItemNameText.GetComponent<TextMeshProUGUI>().text = "Power Plant";
        ItemDescriptionText.GetComponent<TextMeshProUGUI>().text = "Supports up to 5 connections.";
    }
    public void CloseStoreMenu()
    {
        StoreCanvas.gameObject.SetActive(false);
    }
     void Start()
    {
        playerManager = GameObject.Find("Player").GetComponent<PlayerManager>();
        gridBuildingSystem = GameObject.Find("Grid").GetComponent<GridBuildingSystem>();
        BuyGeneratorButton.onClick.AddListener(SelectGenerator);
        BuyPowerPlantButton.onClick.AddListener(SelectPowerPlant);
        CloseStoreButton.onClick.AddListener(CloseStoreMenu);
        BuyButton.onClick.AddListener(delegate { BuyBuilding(purchasedSelectedBuilding); });
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
