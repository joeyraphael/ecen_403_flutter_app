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
public class GridBuildingSystem : MonoBehaviour
{
    // force landscape mode on game
    // performance issue tied to pathfinding
    [SerializeField] public GridLayout gridLayout;
    [SerializeField] public Tilemap MainTilemap;
    [SerializeField] public Tilemap TempTilemap;
    [SerializeField] public Tilemap WireTilemap;
    [SerializeField] public GameObject Natural_House_Prefab;

    [SerializeField] public GameObject Natural_Generator_Prefab;
    [SerializeField] public GameObject Office_Prefab;
    [SerializeField] public GameObject Store_Prefab;

    [SerializeField] public Button WiringModeButton;
    [SerializeField] public Sprite WiringOnSprite;
    [SerializeField] public Sprite WiringOffSprite;
    [SerializeField] public Tile WiringTile;
    public int wiringModeOn = 0;
    public enum TileType
    {
        Empty,
        White,
        Green,
        Red,

    }

    private static Dictionary<TileType, TileBase> tileBases = new Dictionary<TileType, TileBase>();

    public List<Building> ListOfBuildings;

    public PlayerManager playerManager;

    public bool IsOverUIElement = false;

    [SerializeField] public TileBase wireTile;
    [SerializeField] public TileBase whiteTile;

    public Vector3Int lastPlacedTilePosition;
    [SerializeField] public List<GameObject> selectedGameObjects = new List<GameObject>(2);
    public List<Vector3Int> selectedGameObjectsPosition = new List<Vector3Int>(2);

    public enum CurrentActionType
    {
        Placing,
        None,
        Wiring,

    }

    public CurrentActionType currentActionType = CurrentActionType.None;

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

    private static void SetTilesBlock(BoundsInt area, TileType type, Tilemap tilemap)
    {
        int size = area.size.x * area.size.y * area.size.z;
        TileBase[] tileArray = new TileBase[size];
        FillTiles(tileArray, type);
        tilemap.SetTilesBlock(area, tileArray);
    }

    private static void FillTiles(TileBase[] arr, TileType type)
    {
        for (int i = 0; i < arr.Length; i++)
        {
            arr[i] = tileBases[type];
        }
    }

    public Building temp;

    public Building selectedBuilding;
    private Vector3 prevPos;

    private BoundsInt prevArea;

    public void ClearArea()
    {
        TileBase[] toClear = new TileBase[prevArea.size.x * prevArea.size.y * prevArea.size.z];
        FillTiles(toClear, TileType.Empty);
        TempTilemap.SetTilesBlock(prevArea, toClear);
    }

    private void FollowBuilding()
    {
        //ClearArea();
        temp.area.position = gridLayout.WorldToCell(temp.gameObject.transform.position);
        /*BoundsInt buildingArea = temp.area;
        TileBase[] baseArray = GetTilesBlock(buildingArea, MainTilemap);

        int size = baseArray.Length;
        TileBase[] tileArray = new TileBase[size];

        for (int i = 0; i < baseArray.Length; i++)
        {
            if (baseArray[i] == tileBases[TileType.White])
            {
                tileArray[i] = tileBases[TileType.Green];
            }
            else
            {
                FillTiles(tileArray, TileType.Red);
                break;
            }

            TempTilemap.SetTilesBlock(buildingArea, tileArray);
            prevArea = buildingArea;
        } */
    }

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
    public void TakeArea(BoundsInt area)
    {
        SetTilesBlock(area, TileType.Empty, TempTilemap);
        SetTilesBlock(area, TileType.Green, MainTilemap);
    }
    public void InitializeWithBuilding(GameObject building)
    {
        temp = Instantiate(building, UnityEngine.Vector3.zero, UnityEngine.Quaternion.identity).GetComponent<Building>();
        currentActionType = CurrentActionType.Placing;
        //GameObject.Find("CoreGameCanvas").transform.GetChild(0).GetComponent<Button>().interactable = false;
    }

    public void GenerateBuilding(GameObject buildingPrefab, Vector3Int position, string buildingName)
    {
        GameObject spawnedBuilding = Instantiate(buildingPrefab, gridLayout.CellToWorld(position), Quaternion.identity);
        ListOfBuildings.Add(spawnedBuilding.GetComponent<Building>());
        spawnedBuilding.name = buildingName;

    }
    public void GenerateNaturalBuildings()
    {
        GameObject firstGen = Instantiate(Natural_Generator_Prefab, Vector3.zero, Quaternion.identity);
        firstGen.name = "Generator_0";
        firstGen.GetComponent<Building>().buildingPosition = Vector3.zero;
        firstGen.GetComponent<Building>().maxAllowedConnections = 3;
        firstGen.GetComponent<Building>().buildingType = Building.BuildingType.Generator;
        ListOfBuildings.Add(firstGen.GetComponent<Building>());

        /*for (int i = 0; i < 4; i++)
        {
            Debug.Log("Spawning house!");
            UnityEngine.Vector3 randomCoordinates = new UnityEngine.Vector3(0, 1, 0);
            int randomXcoordinate = 0;
            int randomYcoordinate = 0;
            bool foundUniqueCoordinate = false;
            while (!foundUniqueCoordinate)
            {
                randomXcoordinate = UnityEngine.Random.Range(-10, 10);
                randomYcoordinate = UnityEngine.Random.Range(-12, 10);
                //randomXcoordinate = 0;
                //randomYcoordinate = -4;
                //float randomYcoordinateOffset = 0.5f;
                //Debug.Log($"{randomXcoordinate}, {randomYcoordinate - randomYcoordinateOffset}");
                randomCoordinates = new Vector3(randomXcoordinate, randomYcoordinate, 0);
                foreach (Building generatedBuilding in ListOfBuildings)
                {
                    if (randomCoordinates == generatedBuilding.buildingPosition || TempTilemap.GetTile(new Vector3Int(randomXcoordinate, randomYcoordinate, 0)) == null)
                    {
                        continue;
                    }
                }
                foundUniqueCoordinate = true;
            }
            GameObject spawnedHouse = Instantiate(Natural_House_Prefab, gridLayout.CellToWorld(new Vector3Int(randomXcoordinate, randomYcoordinate, 0)), Quaternion.identity);
            spawnedHouse.name = "House " + i.ToString();
            spawnedHouse.GetComponent<Building>().buildingPosition = new Vector3(randomXcoordinate, randomYcoordinate, 0);
            spawnedHouse.GetComponent<Building>().maxAllowedConnections = 1;
            spawnedHouse.GetComponent<Building>().buildingType = Building.BuildingType.House;
            ListOfBuildings.Add(spawnedHouse.GetComponent<Building>());
        } */


    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    public void ToggleWiringMode()
    {
        var tempColor = WiringModeButton.GetComponent<Image>().color;
        if (wiringModeOn == 0)
        {
            wiringModeOn = 1;
            tempColor.a = 1f;
            WiringModeButton.GetComponent<Image>().sprite = WiringOnSprite;
            WiringModeButton.GetComponent<Image>().color = tempColor;
            currentActionType = CurrentActionType.Wiring;
            WireTilemap.gameObject.SetActive(true);
            GameObject.Find("CoreGameCanvas").GetComponent<UI_Manager>().SendNotification("Select a generator first!", "information", Color.white);
            foreach(Building building in ListOfBuildings)
            {
                if (building.buildingType == Building.BuildingType.Generator)
                {
                    building.ConnectionText.gameObject.SetActive(true);
                    building.ConnectionText.GetComponent<TextMeshPro>().text = $"{building.nodeConnectionsDictionary.Count}/{building.maxAllowedConnections}";
                }    
            }
        }
        else if (wiringModeOn == 1)
        {
            wiringModeOn = 2;
            WiringModeButton.GetComponent<Image>().sprite = WiringOffSprite;
            currentActionType = CurrentActionType.Wiring;
            WireTilemap.gameObject.SetActive(true);
            GameObject.Find("CoreGameCanvas").GetComponent<UI_Manager>().SendNotification("Select a generator first!", "information", Color.white);
            foreach(Building building in ListOfBuildings)
            {
                if (building.buildingType == Building.BuildingType.Generator)
                {
                    building.ConnectionText.gameObject.SetActive(true);
                    building.ConnectionText.GetComponent<TextMeshPro>().text = $"{building.nodeConnectionsDictionary.Count}/{building.maxAllowedConnections}";
                }    
            }
        }
        else if (wiringModeOn == 2)
        {
            wiringModeOn = 0;
            WiringModeButton.GetComponent<Image>().sprite = WiringOnSprite;
            tempColor.a = 0.6f;
            WiringModeButton.GetComponent<Image>().color = tempColor;
            currentActionType = CurrentActionType.None;
            WireTilemap.gameObject.SetActive(false);
            foreach(Building building in ListOfBuildings)
            {
                if (building.buildingType == Building.BuildingType.Generator)
                {
                    building.ConnectionText.gameObject.SetActive(false);
                    //building.ConnectionText.GetComponent<TextMeshPro>().text = $"{building.nodeConnectionsDictionary.Count}/{building.maxAllowedConnections}";
                }    
            }
        }
        Debug.Log(wiringModeOn);
    }

    public void GenerateIncome()
    {
        foreach (Building building in ListOfBuildings)
        {
            if (building.isConnectedToPower == true && building.buildingType == Building.BuildingType.House)
            {
                building.IncomeText.gameObject.SetActive(true);
                playerManager.AddMoney(building.income);
                building.IncomeText.GetComponent<TextMeshPro>().text = $"+${building.income}";
                building.IncomeText.GetComponent<TextMeshPro>().color = Color.yellow;

            } else if (building.isConnectedToPower == false && building.buildingType == Building.BuildingType.House)
            {
               building.IncomeText.gameObject.SetActive(true);
               Component[] components = building.IncomeText.gameObject.GetComponents(typeof(Component));
                foreach(Component component in components) {
                    Debug.Log(component.ToString());
                }
               building.IncomeText.GetComponent<TextMeshPro>().text = $"+$0";
               building.IncomeText.GetComponent<TextMeshPro>().color = Color.red;
            }
        }
    }

    void Start()
    {
        //string tilePath = "";
        //tileBases.Add(TileType.Empty, null);
        //tileBases.Add(TileType.White, Resources.Load<TileBase>(tilePath + "white"));
        //tileBases.Add(TileType.Green, Resources.Load<TileBase>(tilePath + "green"));
        //tileBases.Add(TileType.Red, Resources.Load<TileBase>(tilePath + "red"));
        playerManager = GameObject.Find("Player").GetComponent<PlayerManager>();
        GenerateNaturalBuildings();

        WiringModeButton.GetComponent<Button>().onClick.AddListener(ToggleWiringMode);
        //Debug.Log(ListOfBuildings[0].income);
        GenerateBuilding(Natural_House_Prefab, new Vector3Int(-7, 0, 0), "House A");
        GenerateBuilding(Natural_House_Prefab, new Vector3Int(6, -3, 0), "House B") ;
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

    public void Selection()
    {
        if (!selectedBuilding)
        {
            return;
        }
        else
        {
            if (Input.GetMouseButtonDown(0) && currentActionType == CurrentActionType.None)
            {
                RaycastHit raycastHit;
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                if (Physics.Raycast(ray, out raycastHit, 100f))
                {

                }
                else
                {
                    Debug.Log("Didn't select anything! Resetting selections!");
                    selectedBuilding.transform.Find("Sprite").GetComponent<SpriteRenderer>().color = Color.white;
                    selectedBuilding = null;
                }
            }
           
        }
        
    }

    public void PlaceBuilding()
    {
        if (!temp)
        {
            return;
        }
        if (!temp.Placed && currentActionType == CurrentActionType.Placing)
        {
            Debug.Log("Hello");
            Vector2 touchPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector3Int cellPos = gridLayout.LocalToCell(touchPos);
            temp.transform.localPosition = gridLayout.CellToLocalInterpolated(cellPos);
            if (Input.GetMouseButtonDown(0) && TempTilemap.GetTile(cellPos) == whiteTile)
                {
                    temp.transform.localPosition = gridLayout.CellToLocalInterpolated(cellPos);
                    currentActionType = GridBuildingSystem.CurrentActionType.None;
                    temp = null;
                    return;
                }
            FollowBuilding();
        }
    }
    // Update is called once per frame
    void Update()
    {
        if (currentActionType == CurrentActionType.Placing)
        {
            TempTilemap.gameObject.SetActive(true);
        } else
        {
            TempTilemap.gameObject.SetActive(false);
        }
        Vector2 touchPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector3Int cellPos = gridLayout.LocalToCell(touchPos);
        
        if (wiringModeOn != 0)
        {
            if (Input.GetMouseButtonDown(0) && IsOverUIElement == false)
            {
                //Debug.Log(cellPos + " lkjkdlaf");
                if (wiringModeOn == 1)
                {
    
                    if (selectedGameObjects.Count == 2)
                    {
                        Debug.Log("Two objects have been selected, generating path of wires!");
                        Debug.Log($"Looking for Node {selectedGameObjectsPosition[0].x}, {selectedGameObjectsPosition[0].y}");
                        Debug.Log($"Looking for Node {selectedGameObjectsPosition[1].x}, {selectedGameObjectsPosition[1].y}");
                        Node startingNode = AStarManager.instance.FindNearestNode(new Vector2(selectedGameObjectsPosition[0].x, selectedGameObjectsPosition[0].y));
                        Node endingNode = AStarManager.instance.FindNearestNode(new Vector2(selectedGameObjectsPosition[1].x, selectedGameObjectsPosition[1].y));
                        List<Node> pathOfNodes = AStarManager.instance.GeneratePath(startingNode, endingNode);

                        //Debug.Log(selectedGameObjects[0].GetComponent<Building>().income + " dafadf");
                        int numberOfWires = 0;
                        if (selectedGameObjects[0].GetComponent<Building>().nodeConnectionsDictionary.Count  == selectedGameObjects[0].GetComponent<Building>().maxAllowedConnections)
                        {
                            Debug.Log("Max amount of connections!");
                            GameObject.Find("CoreGameCanvas").GetComponent<UI_Manager>().SendNotification("No more available connections!", "alert", Color.red);
                        }
                        else if (pathOfNodes.Count * 50 <= playerManager.money)
                        {
                            foreach (Node node in pathOfNodes)
                            {
                                Debug.Log(node.name + " HI!");
                                WireTilemap.SetTile(Vector3Int.FloorToInt(node.position), wireTile);
                                numberOfWires++;
                            }
                            Debug.Log($"Number of wires: {numberOfWires}");
                            selectedGameObjects[0].GetComponent<Building>().nodeConnectionsDictionary.Add(new List<string>() {selectedGameObjects[0].name, selectedGameObjects[1].name}, pathOfNodes);
                            selectedGameObjects[1].GetComponent<Building>().nodeConnectionsDictionary.Add(new List<string>() {selectedGameObjects[0].name, selectedGameObjects[1].name}, pathOfNodes);
                            selectedGameObjects[0].GetComponent<Building>().ConnectionText.GetComponent<TextMeshPro>().text = $"{selectedGameObjects[0].GetComponent<Building>().nodeConnectionsDictionary.Count}/{selectedGameObjects[0].GetComponent<Building>().maxAllowedConnections}";
                            playerManager.AddMoney(-50 * numberOfWires);
                            selectedGameObjects[0].GetComponent<Building>().isConnectedToPower = true;
                            selectedGameObjects[1].GetComponent<Building>().isConnectedToPower = true;
                        }
                        else
                        {
                            Debug.Log("Player doesn't have enough money!");
                            GameObject.Find("CoreGameCanvas").GetComponent<UI_Manager>().SendNotification("Not enough money!", "alert", Color.red);
                        }
                        
                        selectedGameObjects[0].transform.Find("Sprite").GetComponent<SpriteRenderer>().color = Color.white;
                        selectedGameObjects[1].transform.Find("Sprite").GetComponent<SpriteRenderer>().color = Color.white;
                        selectedGameObjects.Clear();
                        selectedGameObjectsPosition.Clear();
                    }
                    //Debug.Log(cellPos);
                    //WireTilemap.SetTile(cellPos, wireTile);
                    //playerManager.AddMoney(-50);
                }
                else if (wiringModeOn == 2)
                {
                   if (selectedGameObjects.Count == 2)
                    {
                        Debug.Log("Two objects have been selected, deleting path of wires!");
                        // force player to select generator
                        Node startingNode = AStarManager.instance.FindNearestNode(new Vector2(selectedGameObjectsPosition[0].x, selectedGameObjectsPosition[0].y));
                        Node endingNode = AStarManager.instance.FindNearestNode(new Vector2(selectedGameObjectsPosition[1].x, selectedGameObjectsPosition[1].y));
                        List<Node> pathOfNodes = AStarManager.instance.GeneratePath(startingNode, endingNode);
                        //pathOfNodes.Remove(pathOfNodes[0]);

                        foreach (var node in selectedGameObjects[0].GetComponent<Building>().nodeConnectionsDictionary.ToList())
                        {
                            Debug.Log($"node.Key = {node.Key[0]}, {node.Key[1]}");
                            if (node.Key.Contains(selectedGameObjects[0].name) && node.Key.Contains(selectedGameObjects[1].name))
                            {
                                int numberOfWires = 0;
                                foreach (Node nodeInDictionary in node.Value)
                                {
                                    Debug.Log(nodeInDictionary.name + " BYE!");
                                    WireTilemap.SetTile(Vector3Int.FloorToInt(nodeInDictionary.position), null);
                                    //playerManager.AddMoney(50);
                                    // delete connections, delete list, set connection to false
                                }
                                List<string> entryName = new List<string> {selectedGameObjects[0].name, selectedGameObjects[1].name};
                                selectedGameObjects[0].GetComponent<Building>().nodeConnectionsDictionary.Remove(node.Key);
                                selectedGameObjects[1].GetComponent<Building>().nodeConnectionsDictionary.Remove(node.Key);
                                Debug.Log(selectedGameObjects[0].GetComponent<Building>().nodeConnectionsDictionary.Count);
                                Debug.Log(selectedGameObjects[1].GetComponent<Building>().nodeConnectionsDictionary.Count);
                                //selectedGameObjects[1].GetComponent<Building>().nodeConnectionsDictionary
                                playerManager.AddMoney(10 * numberOfWires);
                                selectedGameObjects[0].GetComponent<Building>().ConnectionText.GetComponent<TextMeshPro>().text = $"{selectedGameObjects[0].GetComponent<Building>().nodeConnectionsDictionary.Count}/{selectedGameObjects[0].GetComponent<Building>().maxAllowedConnections}";
                            } 
                        }

                        selectedGameObjects[0].GetComponent<Building>().isConnectedToPower = false;
                        selectedGameObjects[1].GetComponent<Building>().isConnectedToPower = false;
                        selectedGameObjects[0].transform.Find("Sprite").GetComponent<SpriteRenderer>().color = Color.white;
                        selectedGameObjects[1].transform.Find("Sprite").GetComponent<SpriteRenderer>().color = Color.white;
                        selectedGameObjects.Clear();
                        selectedGameObjectsPosition.Clear();
                    }
                }
            }
            return;
        }
        //Selection();
        PlaceBuilding();



        
        
    }


}
