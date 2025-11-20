using System;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Tilemaps;
public class Building : MonoBehaviour, IPointerClickHandler
{

    public bool Placed { get; set; }
    [SerializeField] public BoundsInt area;
    [SerializeField] public int income;
    [SerializeField] public int cost;

    public Vector3 buildingPosition = new Vector3(0, 0, 0);

    public Vector3Int buildingPositionInt;
    [SerializeField] public Dictionary<List<string>, List<Node>> nodeConnectionsDictionary = new Dictionary<List<string>, List<Node>>(5);
    public enum BuildingType
    {
        House,
        Generator,
        None,

    }
    public BuildingType buildingType = BuildingType.None;
    public bool recentlySpawned = false;
    public int maxAllowedConnections;
    Tilemap TempTileMap;
    public GridLayout gridLayout;
    public GridBuildingSystem gridBuildingSystem;
    [SerializeField] public bool isConnectedToPower = false;

    public GameObject IncomeText;
    public GameObject ConnectionText;

    public bool CanBePlaced()
    {
        Vector3Int positionInt = GameObject.Find("Grid").GetComponent<GridBuildingSystem>().gridLayout.LocalToCell(transform.position);
        BoundsInt areaTemp = area;
        areaTemp.position = positionInt;

        if (GameObject.Find("Grid").GetComponent<GridBuildingSystem>().CanTakeArea(areaTemp))
        {
            return true;
        }
        return false;
    }

    public void CheckPowerConnection()
    {
        
    }

    public void Place()
    {
        Vector3Int positionInt = GameObject.Find("Grid").GetComponent<GridBuildingSystem>().gridLayout.LocalToCell(transform.position);
        BoundsInt areaTemp = area;
        areaTemp.position = positionInt;
        Placed = true;
        GameObject.Find("Grid").GetComponent<GridBuildingSystem>().TakeArea(area);
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        gridLayout = GameObject.Find("Grid").GetComponent<GridLayout>();
        gridBuildingSystem = GameObject.Find("Grid").GetComponent<GridBuildingSystem>();
        //income = UnityEngine.Random.Range(5, 8);
        if (this.buildingType == BuildingType.House)
        {
            IncomeText = this.transform.Find("IncomeText").gameObject;
            IncomeText.SetActive(false);
        }
        else if (this.buildingType == BuildingType.Generator)
        {
            ConnectionText = this.transform.Find("ConnectionText").gameObject;
            ConnectionText.SetActive(false);
        }
        //Debug.Log(this.transform.childCount);
        buildingPositionInt = GameObject.Find("Grid").GetComponent<GridBuildingSystem>().gridLayout.LocalToCell(transform.position);
    }


    // Update is called once per frame
    void Update()
    {

    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (gridBuildingSystem.currentActionType == GridBuildingSystem.CurrentActionType.None)
        {
            GameObject clickedObject = eventData.pointerPress;
            Debug.Log(eventData.pointerPress.name);
            Debug.Log(clickedObject.name + " has been selected!");
            //clickedObject.transform.GetChild(0).GetComponent<SpriteRenderer>().color = Color.green;
            GameObject.Find("Grid").GetComponent<GridBuildingSystem>().selectedBuilding = this;
        }
        else if (gridBuildingSystem.currentActionType == GridBuildingSystem.CurrentActionType.Wiring)
        {
            if (gridBuildingSystem.selectedGameObjects.Count == 0)
            {
                Vector2 touchPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                Vector3Int cellPos = gridLayout.LocalToCell(touchPos);
                GameObject clickedObject = eventData.pointerPress;
                if (clickedObject.GetComponent<Building>().buildingType != BuildingType.Generator)
                {
                    Debug.Log("Didn't select a generator, returning!");
                    return;
                }
                clickedObject.transform.Find("Sprite").GetComponent<SpriteRenderer>().color = Color.red;
                gridBuildingSystem.selectedGameObjects.Add(clickedObject);
                gridBuildingSystem.selectedGameObjectsPosition.Add(cellPos);
            }
            else if (gridBuildingSystem.selectedGameObjects.Count == 1)
            {
                Vector2 touchPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                Vector3Int cellPos = gridLayout.LocalToCell(touchPos);
                GameObject clickedObject = eventData.pointerPress;
                if (clickedObject.GetComponent<Building>().buildingType != BuildingType.House)
                {
                    gridBuildingSystem.selectedGameObjects[0].transform.Find("Sprite").GetComponent<SpriteRenderer>().color = Color.white;
                    Debug.Log("Didn't select a house, returning!");
                    return;
                }
                clickedObject.transform.Find("Sprite").GetComponent<SpriteRenderer>().color = Color.blue;
                gridBuildingSystem.selectedGameObjects.Add(clickedObject);
                gridBuildingSystem.selectedGameObjectsPosition.Add(cellPos);
            }
        }
    }

}
