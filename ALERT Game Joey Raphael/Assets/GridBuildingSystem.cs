using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using TMPro;
using Unity.VisualScripting;
//using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.Rendering.Universal;
using UnityEngine.Tilemaps;
using UnityEngine.UI;

/// <summary>
/// Main system that manages:
/// - Placing buildings on a grid
/// - Tracking occupied tiles
/// - Handling wiring mode (connect/disconnect buildings with wires)
/// - Generating income from powered houses
/// </summary>
public class GridBuildingSystem : MonoBehaviour
{
    // Grid and tilemaps
    [SerializeField] public GridLayout gridLayout;
    [SerializeField] public Tilemap MainTilemap;  // Main map where tiles are considered "occupied"
    [SerializeField] public Tilemap TempTilemap;  // Temp/preview map for placement
    [SerializeField] public Tilemap WireTilemap;  // Tilemap used for visualizing wires

    // Building prefabs
    [SerializeField] public GameObject Natural_House_Prefab;
    [SerializeField] public GameObject Natural_Generator_Prefab;
    [SerializeField] public GameObject Office_Prefab;
    [SerializeField] public GameObject Store_Prefab;

    // Wiring UI
    [SerializeField] public Button WiringModeButton;
    [SerializeField] public Sprite WiringOnSprite;
    [SerializeField] public Sprite WiringOffSprite;
    [SerializeField] public Tile WiringTile;

    /// <summary>
    /// 0 = wiring off
    /// 1 = wiring create mode (draw wires)
    /// 2 = wiring delete mode (remove wires)
    /// </summary>
    public int wiringModeOn = 0;

    /// <summary>Types of tiles used for placement validation / visualization.</summary>
    public enum TileType
    {
        Empty,
        White,
        Green,
        Red,
    }

    // Map from logical tile type to actual TileBase assets (currently commented out in Start)
    private static Dictionary<TileType, TileBase> tileBases = new Dictionary<TileType, TileBase>();

    /// <summary>All buildings currently in the world.</summary>
    public List<Building> ListOfBuildings;

    /// <summary>Reference to the player manager for money and question counts.</summary>
    public PlayerManager playerManager;

    /// <summary>
    /// True if the mouse is currently over a UI element that should block world interaction.
    /// Set by BlockClicks.
    /// </summary>
    public bool IsOverUIElement = false;

    [SerializeField] public TileBase wireTile;   // Tile asset for wires
    [SerializeField] public TileBase whiteTile;  // Tile asset used for placeable cells

    /// <summary>Position of the last placed tile (used in wiring/building logic).</summary>
    public Vector3Int lastPlacedTilePosition;

    /// <summary>Currently selected buildings (used in wiring mode: generator + house).</summary>
    [SerializeField] public List<GameObject> selectedGameObjects = new List<GameObject>(2);

    /// <summary>Grid positions of the selected buildings (used for A* pathfinding start/end).</summary>
    public List<Vector3Int> selectedGameObjectsPosition = new List<Vector3Int>(2);

    /// <summary>Current global action for the grid system.</summary>
    public enum CurrentActionType
    {
        Placing,  // placing a building
        None,     // idle
        Wiring,   // wiring (create/delete)
    }

    public CurrentActionType currentActionType = CurrentActionType.None;

    /// <summary>
    /// Reads a block of tiles from a tilemap in a given area.
    /// </summary>
    private static TileBase[] GetTilesBlock(BoundsInt area, Tilemap tilemap)
    {
        TileBase[] array = new TileBase[area.size.x * area.size.y * area.size.z];
        int counter = 0;

        foreach (var v in area.allPositionsWithin)
        {
            Vector3Int pos = new Vector3Int(v.x, v.y, 0);
            array[counter] = tilemap.GetTile(pos);
            counter++;
        }

        return array;
    }

    /// <summary>
    /// Fills an area in a tilemap with a given TileType.
    /// </summary>
    private static void SetTilesBlock(BoundsInt area, TileType type, Tilemap tilemap)
    {
        int size = area.size.x * area.size.y * area.size.z;
        TileBase[] tileArray = new TileBase[size];
        FillTiles(tileArray, type);
        tilemap.SetTilesBlock(area, tileArray);
    }

    /// <summary>
    /// Helper to fill an array of tiles with a single TileType.
    /// </summary>
    private static void FillTiles(TileBase[] arr, TileType type)
    {
        for (int i = 0; i < arr.Length; i++)
        {
            arr[i] = tileBases[type];
        }
    }

    /// <summary>Temporary reference to a building being placed.</summary>
    public Building temp;

    /// <summary>The actively selected building (when not in wiring mode).</summary>
    public Building selectedBuilding;

    private Vector3 prevPos;
    private BoundsInt prevArea;

    /// <summary>
    /// Clears the previously previewed area on the TempTilemap.
    /// </summary>
    public void ClearArea()
    {
        TileBase[] toClear = new TileBase[prevArea.size.x * prevArea.size.y * prevArea.size.z];
        FillTiles(toClear, TileType.Empty);
        TempTilemap.SetTilesBlock(prevArea, toClear);
    }

    /// <summary>
    /// Makes the preview building follow the mouse/grid position.
    /// (Visual feedback of where the building will be placed.)
    /// </summary>
    private void FollowBuilding()
    {
        // Update the area position based on building position
        temp.area.position = gridLayout.WorldToCell(temp.gameObject.transform.position);

        // (Tile coloring preview logic is commented out here)
    }

    /// <summary>
    /// Checks if a given tile area is available (all tiles are "White" / free).
    /// </summary>
    public bool CanTakeArea(BoundsInt area)
    {
        TileBase[] baseArray = GetTilesBlock(area, MainTilemap);
        foreach (var b in baseArray)
        {
            if (b != tileBases[TileType.White])
            {
                Debug.Log("Cannot place here!");
                return false;
            }
        }
        return true;
    }

    /// <summary>
    /// Marks an area as taken: removes preview tiles and paints it as "Green" (occupied).
    /// </summary>
    public void TakeArea(BoundsInt area)
    {
        SetTilesBlock(area, TileType.Empty, TempTilemap);
        SetTilesBlock(area, TileType.Green, MainTilemap);
    }

    /// <summary>
    /// Instantiates a temporary building to be placed by the player.
    /// </summary>
    public void InitializeWithBuilding(GameObject building)
    {
        temp = Instantiate(building, UnityEngine.Vector3.zero, UnityEngine.Quaternion.identity)
            .GetComponent<Building>();
        currentActionType = CurrentActionType.Placing;
        // Example: disable button while placing (commented out)
        //GameObject.Find("CoreGameCanvas").transform.GetChild(0).GetComponent<Button>().interactable = false;
    }

    /// <summary>
    /// Spawns a building prefab at a specific grid position and registers it in the building list.
    /// </summary>
    public void GenerateBuilding(GameObject buildingPrefab, Vector3Int position, string buildingName)
    {
        GameObject spawnedBuilding =
            Instantiate(buildingPrefab, gridLayout.CellToWorld(position), Quaternion.identity);

        ListOfBuildings.Add(spawnedBuilding.GetComponent<Building>());
        spawnedBuilding.name = buildingName;
    }

    /// <summary>
    /// Spawns the initial generator and (optionally) natural houses.
    /// </summary>
    public void GenerateNaturalBuildings()
    {
        // Spawn starting generator at origin
        GameObject firstGen = Instantiate(Natural_Generator_Prefab, Vector3.zero, Quaternion.identity);
        firstGen.name = "Generator_0";
        firstGen.GetComponent<Building>().buildingPosition = Vector3.zero;
        firstGen.GetComponent<Building>().maxAllowedConnections = 3;
        firstGen.GetComponent<Building>().buildingType = Building.BuildingType.Generator;
        ListOfBuildings.Add(firstGen.GetComponent<Building>());

        // House generation logic is commented out but kept for reference
    }

    /// <summary>
    /// Toggles between wiring modes (off, create, delete),
    /// and updates UI + generator connection text accordingly.
    /// </summary>
    public void ToggleWiringMode()
    {
        var tempColor = WiringModeButton.GetComponent<Image>().color;

        // Mode 0 -> 1 (enter wiring-create mode)
        if (wiringModeOn == 0)
        {
            wiringModeOn = 1;
            tempColor.a = 1f;
            WiringModeButton.GetComponent<Image>().sprite = WiringOnSprite;
            WiringModeButton.GetComponent<Image>().color = tempColor;
            currentActionType = CurrentActionType.Wiring;
            WireTilemap.gameObject.SetActive(true);

            GameObject.Find("CoreGameCanvas")
                .GetComponent<UI_Manager>()
                .SendNotification("Select a generator first!", "information", Color.white);

            foreach (Building building in ListOfBuildings)
            {
                if (building.buildingType == Building.BuildingType.Generator)
                {
                    building.ConnectionText.gameObject.SetActive(true);
                    building.ConnectionText.GetComponent<TextMeshPro>().text =
                        $"{building.nodeConnectionsDictionary.Count}/{building.maxAllowedConnections}";
                }
            }
        }
        // Mode 1 -> 2 (switch to wiring-delete mode)
        else if (wiringModeOn == 1)
        {
            wiringModeOn = 2;
            WiringModeButton.GetComponent<Image>().sprite = WiringOffSprite;
            currentActionType = CurrentActionType.Wiring;
            WireTilemap.gameObject.SetActive(true);

            GameObject.Find("CoreGameCanvas")
                .GetComponent<UI_Manager>()
                .SendNotification("Select a generator first!", "information", Color.white);

            foreach (Building building in ListOfBuildings)
            {
                if (building.buildingType == Building.BuildingType.Generator)
                {
                    building.ConnectionText.gameObject.SetActive(true);
                    building.ConnectionText.GetComponent<TextMeshPro>().text =
                        $"{building.nodeConnectionsDictionary.Count}/{building.maxAllowedConnections}";
                }
            }
        }
        // Mode 2 -> 0 (turn wiring off completely)
        else if (wiringModeOn == 2)
        {
            wiringModeOn = 0;
            WiringModeButton.GetComponent<Image>().sprite = WiringOnSprite;
            tempColor.a = 0.6f;
            WiringModeButton.GetComponent<Image>().color = tempColor;
            currentActionType = CurrentActionType.None;
            WireTilemap.gameObject.SetActive(false);

            foreach (Building building in ListOfBuildings)
            {
                if (building.buildingType == Building.BuildingType.Generator)
                {
                    building.ConnectionText.gameObject.SetActive(false);
                }
            }
        }

        Debug.Log(wiringModeOn);
    }

    /// <summary>
    /// Iterates through all buildings and generates income for powered houses.
    /// Shows temporary floating income text with color feedback.
    /// </summary>
    public void GenerateIncome()
    {
        foreach (Building building in ListOfBuildings)
        {
            // Powered house: generate income in yellow
            if (building.isConnectedToPower == true &&
                building.buildingType == Building.BuildingType.House)
            {
                building.IncomeText.gameObject.SetActive(true);
                playerManager.AddMoney(building.income);
                building.IncomeText.GetComponent<TextMeshPro>().text = $"+${building.income}";
                building.IncomeText.GetComponent<TextMeshPro>().color = Color.yellow;
            }
            // Unpowered house: show +$0 in red
            else if (building.isConnectedToPower == false &&
                     building.buildingType == Building.BuildingType.House)
            {
                building.IncomeText.gameObject.SetActive(true);

                // Debugging: list all components on IncomeText
                Component[] components = building.IncomeText.gameObject.GetComponents(typeof(Component));
                foreach (Component component in components)
                {
                    Debug.Log(component.ToString());
                }

                building.IncomeText.GetComponent<TextMeshPro>().text = "+$0";
                building.IncomeText.GetComponent<TextMeshPro>().color = Color.red;
            }
        }
    }

    /// <summary>
    /// Unity Start method.
    /// Initializes player reference, natural buildings, wiring button, and some pre-placed structures.
    /// </summary>
    void Start()
    {
        // Example of how tileBases could be initialized (currently commented out)
        /*
        string tilePath = "";
        tileBases.Add(TileType.Empty, null);
        tileBases.Add(TileType.White, Resources.Load<TileBase>(tilePath + "white"));
        tileBases.Add(TileType.Green, Resources.Load<TileBase>(tilePath + "green"));
        tileBases.Add(TileType.Red, Resources.Load<TileBase>(tilePath + "red"));
        */

        playerManager = GameObject.Find("Player").GetComponent<PlayerManager>();
        GenerateNaturalBuildings();

        WiringModeButton.GetComponent<Button>().onClick.AddListener(ToggleWiringMode);

        // Spawn some initial buildings at fixed positions
        GenerateBuilding(Natural_House_Prefab, new Vector3Int(-7, 0, 0), "House A");
        GenerateBuilding(Natural_House_Prefab, new Vector3Int(6, -3, 0), "House B");
        GenerateBuilding(Natural_House_Prefab, new Vector3Int(0, -9, 0), "House C");
        GenerateBuilding(Natural_House_Prefab, new Vector3Int(5, 9, 0), "House D");

        GenerateBuilding(Store_Prefab, new Vector3Int(-8, 8, 0), "Store A");
        GenerateBuilding(Store_Prefab, new Vector3Int(-6, 10, 0), "Store B");
        GenerateBuilding(Store_Prefab, new Vector3Int(-6, 6, 0), "Store C");

        GenerateBuilding(Office_Prefab, new Vector3Int(4, 17, 0), "Office A");
        GenerateBuilding(Office_Prefab, new Vector3Int(10, 7, 0), "Office B");
        GenerateBuilding(Office_Prefab, new Vector3Int(12, 7, 0), "Office C");
        GenerateBuilding(Office_Prefab, new Vector3Int(12, 9, 0), "Office D");
        GenerateBuilding(Office_Prefab, new Vector3Int(10, 10, 0), "Office E");
    }

    /// <summary>
    /// Handles deselecting a building if the player clicks on empty space
    /// while in "None" action mode.
    /// </summary>
    public void Selection()
    {
        if (!selectedBuilding)
        {
            return;
        }

        if (Input.GetMouseButtonDown(0) && currentActionType == CurrentActionType.None)
        {
            RaycastHit raycastHit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out raycastHit, 100f))
            {
                // Something was hit; selection stays as is.
            }
            else
            {
                // Clicked empty space: clear selection
                Debug.Log("Didn't select anything! Resetting selections!");
                selectedBuilding.transform.Find("Sprite")
                    .GetComponent<SpriteRenderer>().color = Color.white;
                selectedBuilding = null;
            }
        }
    }

    /// <summary>
    /// Handles placing a building currently being previewed (temp).
    /// </summary>
    public void PlaceBuilding()
    {
        // No building in placement mode
        if (!temp)
        {
            return;
        }

        if (!temp.Placed && currentActionType == CurrentActionType.Placing)
        {
            Vector2 touchPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector3Int cellPos = gridLayout.LocalToCell(touchPos);

            // Snap preview building to grid
            temp.transform.localPosition = gridLayout.CellToLocalInterpolated(cellPos);

            // On left-click, confirm placement if tile is allowed (white)
            if (Input.GetMouseButtonDown(0) && TempTilemap.GetTile(cellPos) == whiteTile)
            {
                temp.transform.localPosition = gridLayout.CellToLocalInterpolated(cellPos);
                currentActionType = CurrentActionType.None;
                temp = null;
                return;
            }

            FollowBuilding();
        }
    }

    /// <summary>
    /// Unity Update loop.
    /// Manages:
    /// - Temp tilemap visibility during placement
    /// - Wiring mode interactions (create/delete wires)
    /// - Building placement
    /// </summary>
    void Update()
    {
        // Show/hide temp tilemap based on current action
        if (currentActionType == CurrentActionType.Placing)
        {
            TempTilemap.gameObject.SetActive(true);
        }
        else
        {
            TempTilemap.gameObject.SetActive(false);
        }

        // Always track current mouse position in grid space
        Vector2 touchPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector3Int cellPos = gridLayout.LocalToCell(touchPos);

        // -----------------------
        // Wiring Mode Handling
        // -----------------------
        if (wiringModeOn != 0)
        {
            // Only allow grid clicks if not over UI
            if (Input.GetMouseButtonDown(0) && IsOverUIElement == false)
            {
                // Wiring mode: create wires
                if (wiringModeOn == 1)
                {
                    if (selectedGameObjects.Count == 2)
                    {
                        Debug.Log("Two objects have been selected, generating path of wires!");
                        Debug.Log($"Looking for Node {selectedGameObjectsPosition[0].x}, {selectedGameObjectsPosition[0].y}");
                        Debug.Log($"Looking for Node {selectedGameObjectsPosition[1].x}, {selectedGameObjectsPosition[1].y}");

                        // Get start/end nodes based on building positions
                        Node startingNode = AStarManager.instance.FindNearestNode(
                            new Vector2(selectedGameObjectsPosition[0].x, selectedGameObjectsPosition[0].y));
                        Node endingNode = AStarManager.instance.FindNearestNode(
                            new Vector2(selectedGameObjectsPosition[1].x, selectedGameObjectsPosition[1].y));

                        List<Node> pathOfNodes = AStarManager.instance.GeneratePath(startingNode, endingNode);

                        int numberOfWires = 0;

                        // Check generator connection limit
                        if (selectedGameObjects[0].GetComponent<Building>().nodeConnectionsDictionary.Count
                            == selectedGameObjects[0].GetComponent<Building>().maxAllowedConnections)
                        {
                            Debug.Log("Max amount of connections!");
                            GameObject.Find("CoreGameCanvas")
                                .GetComponent<UI_Manager>()
                                .SendNotification("No more available connections!", "alert", Color.red);
                        }
                        // Enough money to place the wires
                        else if (pathOfNodes.Count * 50 <= playerManager.money)
                        {
                            // Draw each node in the path onto the WireTilemap
                            foreach (Node node in pathOfNodes)
                            {
                                Debug.Log(node.name + " HI!");
                                WireTilemap.SetTile(Vector3Int.FloorToInt(node.position), wireTile);
                                numberOfWires++;
                            }

                            Debug.Log($"Number of wires: {numberOfWires}");

                            // Store connection info in both buildings' dictionaries
                            selectedGameObjects[0].GetComponent<Building>()
                                .nodeConnectionsDictionary
                                .Add(new List<string>() { selectedGameObjects[0].name, selectedGameObjects[1].name },
                                     pathOfNodes);

                            selectedGameObjects[1].GetComponent<Building>()
                                .nodeConnectionsDictionary
                                .Add(new List<string>() { selectedGameObjects[0].name, selectedGameObjects[1].name },
                                     pathOfNodes);

                            // Update connection text (e.g., 1/3)
                            selectedGameObjects[0].GetComponent<Building>()
                                .ConnectionText.GetComponent<TextMeshPro>().text =
                                $"{selectedGameObjects[0].GetComponent<Building>().nodeConnectionsDictionary.Count}/" +
                                $"{selectedGameObjects[0].GetComponent<Building>().maxAllowedConnections}";

                            // Charge player for wire cost
                            playerManager.AddMoney(-50 * numberOfWires);

                            // Mark both buildings as powered
                            selectedGameObjects[0].GetComponent<Building>().isConnectedToPower = true;
                            selectedGameObjects[1].GetComponent<Building>().isConnectedToPower = true;
                        }
                        else
                        {
                            Debug.Log("Player doesn't have enough money!");
                            GameObject.Find("CoreGameCanvas")
                                .GetComponent<UI_Manager>()
                                .SendNotification("Not enough money!", "alert", Color.red);
                        }

                        // Clear selection + visual highlight
                        selectedGameObjects[0].transform.Find("Sprite")
                            .GetComponent<SpriteRenderer>().color = Color.white;
                        selectedGameObjects[1].transform.Find("Sprite")
                            .GetComponent<SpriteRenderer>().color = Color.white;
                        selectedGameObjects.Clear();
                        selectedGameObjectsPosition.Clear();
                    }
                }
                // Wiring mode: delete wires
                else if (wiringModeOn == 2)
                {
                    if (selectedGameObjects.Count == 2)
                    {
                        Debug.Log("Two objects have been selected, deleting path of wires!");

                        // Force player to select generator first, then house
                        Node startingNode = AStarManager.instance.FindNearestNode(
                            new Vector2(selectedGameObjectsPosition[0].x, selectedGameObjectsPosition[0].y));
                        Node endingNode = AStarManager.instance.FindNearestNode(
                            new Vector2(selectedGameObjectsPosition[1].x, selectedGameObjectsPosition[1].y));

                        List<Node> pathOfNodes = AStarManager.instance.GeneratePath(startingNode, endingNode);

                        // Iterate over connections stored on the generator
                        foreach (var node in selectedGameObjects[0]
                                     .GetComponent<Building>()
                                     .nodeConnectionsDictionary.ToList())
                        {
                            Debug.Log($"node.Key = {node.Key[0]}, {node.Key[1]}");

                            // Match entry that connects these two buildings
                            if (node.Key.Contains(selectedGameObjects[0].name) &&
                                node.Key.Contains(selectedGameObjects[1].name))
                            {
                                int numberOfWires = 0;

                                // Clear wire tiles from the path
                                foreach (Node nodeInDictionary in node.Value)
                                {
                                    Debug.Log(nodeInDictionary.name + " BYE!");
                                    WireTilemap.SetTile(Vector3Int.FloorToInt(nodeInDictionary.position), null);
                                    // numberOfWires++; // (currently unused, can be incremented here if needed)
                                }

                                // Remove entries from both buildings' dictionaries
                                selectedGameObjects[0].GetComponent<Building>()
                                    .nodeConnectionsDictionary.Remove(node.Key);
                                selectedGameObjects[1].GetComponent<Building>()
                                    .nodeConnectionsDictionary.Remove(node.Key);

                                Debug.Log(selectedGameObjects[0].GetComponent<Building>().nodeConnectionsDictionary.Count);
                                Debug.Log(selectedGameObjects[1].GetComponent<Building>().nodeConnectionsDictionary.Count);

                                // Refund some money based on removed wires (currently numberOfWires is 0)
                                playerManager.AddMoney(10 * numberOfWires);

                                // Update generator connection text
                                selectedGameObjects[0].GetComponent<Building>()
                                    .ConnectionText.GetComponent<TextMeshPro>().text =
                                    $"{selectedGameObjects[0].GetComponent<Building>().nodeConnectionsDictionary.Count}/" +
                                    $"{selectedGameObjects[0].GetComponent<Building>().maxAllowedConnections}";
                            }
                        }

                        // Mark both buildings as unpowered
                        selectedGameObjects[0].GetComponent<Building>().isConnectedToPower = false;
                        selectedGameObjects[1].GetComponent<Building>().isConnectedToPower = false;

                        // Clear selection highlight + lists
                        selectedGameObjects[0].transform.Find("Sprite")
                            .GetComponent<SpriteRenderer>().color = Color.white;
                        selectedGameObjects[1].transform.Find("Sprite")
                            .GetComponent<SpriteRenderer>().color = Color.white;
                        selectedGameObjects.Clear();
                        selectedGameObjectsPosition.Clear();
                    }
                }
            }

            // Early exit because wiring mode consumes click handling
            return;
        }

        // Building placement when not wiring (Selection logic is optional)
        //Selection();
        PlaceBuilding();
    }
}
