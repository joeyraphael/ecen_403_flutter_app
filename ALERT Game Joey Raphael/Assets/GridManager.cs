using System;
using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.UI;

public class GridManager : MonoBehaviour
{
    [SerializeField] private int _width, _height = 5;
    [SerializeField] private Tile _tilePrefab;

    [SerializeField] private Transform _cam;

    private Dictionary<Vector2, Tile> _tiles;

    [SerializeField] public int _totalRocks;

    public bool wiringModeActive = false;

    [SerializeField] public Button WiringModeButton;

    public List<string> placeableTypeList = new List<string> { "obstacle", "wire", "voltage_source", "null" };

    [SerializeField] public List<Vector2> voltageSourcesPositionList;
    public List<Vector2> housesPositionList;

    [SerializeField] public Button StartTimeButton;

    void Start()
    {
        //GenerateGrid();
        //GenerateRocks(3);
        //GenerateVoltageSources(1);
        //GenerateHouses(2);
        //WiringModeButton.GetComponent<Button>().onClick.AddListener(ChangeWiringMode);
        //StartTimeButton.GetComponent<Button>().onClick.AddListener(CheckVoltageSourceConnections);
    }
    
    bool CheckCardinalPaths(Vector2 startingPosition, Vector2 propagationDirection) {
        int numberOfValidPaths = 0;
        if (GetTileAtPosition(startingPosition + Vector2.up).placeableType == "wire") {
            numberOfValidPaths++;
        }
        if (GetTileAtPosition(startingPosition + Vector2.down).placeableType == "wire") {
            numberOfValidPaths++;
        }
        if (GetTileAtPosition(startingPosition + Vector2.left).placeableType == "wire") {
            numberOfValidPaths++;
        }
        if (GetTileAtPosition(startingPosition + Vector2.right).placeableType == "wire") {
            numberOfValidPaths++;
        }
        return false;
    }

    void PropagateSignal(Vector2 startingPosition) {
        bool reachedSignalEndPath = false;
        while (!reachedSignalEndPath) {

        }
    }

    void CheckVoltageSourceConnections()
    {
        foreach (Vector2 position in voltageSourcesPositionList)
        {
            Debug.Log("Hello!!!");
            Tile chosenTile = GetTileAtPosition(position);
            Debug.Log(position + Vector2.up);
            if (GetTileAtPosition(position + Vector2.up).placeableType == "wire") {
                Debug.Log("Hello!");
            }
            if (GetTileAtPosition(position + Vector2.down).placeableType == "wire") {
                
            }
            if (GetTileAtPosition(position + Vector2.right).placeableType == "wire") {
                
            }
            if (GetTileAtPosition(position + Vector2.left).placeableType == "wire") {
                
            }
        }
    }

    void ChangeWiringMode()
    {
        wiringModeActive = !wiringModeActive;
        Debug.Log("I was clicked!!!");
        Image WiringModeImage = WiringModeButton.GetComponent<Image>();
        if (wiringModeActive == false)
        {
            WiringModeImage.color = Color.red;
        }
        else
        {
            WiringModeImage.color = Color.green;
        }
    }

    void GenerateGrid()
    {
        _tiles = new Dictionary<Vector2, Tile>();
        for (int x = 0; x < _width; x++)
        {
            for (int y = 0; y < _height; y++)
            {
                var spawnedTile = Instantiate(_tilePrefab, new Vector3(x, y), Quaternion.identity);
                spawnedTile.name = $"Tile {x} {y}";
                spawnedTile.placeableType = "null";
                var isOffset = (x % 2 == 0 && y % 2 != 0) || (x % 2 != 0 && y % 2 == 0);
                spawnedTile.Init(isOffset);

                _tiles[new Vector2(x, y)] = spawnedTile;
            }
        }

        _cam.transform.position = new Vector3((float)_width / 2 - 0.5f, (float)_height / 2 - 0.5f, -10);
    }

    public Tile GetTileAtPosition(Vector2 pos)
    {
        if (_tiles.TryGetValue(pos, out var tile)) return tile;
        return null;
    }

    void GenerateRocks(int _totalRocks)
    {
        for (int x = 0; x < _totalRocks; x++)
        {
            Vector2 randomVector2 = new Vector2(UnityEngine.Random.Range(0, _width), UnityEngine.Random.Range(0, _height));
            Tile chosenTile = GetTileAtPosition(randomVector2);
            if (chosenTile.isOccupied == false)
            {
                chosenTile.GetComponent<SpriteRenderer>().color = Color.red;
                chosenTile.isOccupied = true;
                chosenTile.placeableType = "obstacle";
            }
        }
    }

    void GenerateVoltageSources(int _totalVoltageSources)
    {
        for (int x = 0; x < _totalVoltageSources; x++)
        {
            Vector2 randomVector2 = new Vector2(UnityEngine.Random.Range(0, _width), UnityEngine.Random.Range(0, _height));
            Tile chosenTile = GetTileAtPosition(randomVector2);
            if (chosenTile.isOccupied == false)
            {
                chosenTile.GetComponent<SpriteRenderer>().color = Color.cyan;
                chosenTile.isOccupied = true;
                chosenTile.placeableType = "obstacle";
                voltageSourcesPositionList.Add(randomVector2);
            }
        }
    }

    void GenerateHouses(int _totalHouses)
    { 
        for (int x = 0; x < _totalHouses; x++)
        {
            Vector2 randomVector2 = new Vector2(UnityEngine.Random.Range(0, _width), UnityEngine.Random.Range(0, _height));
            Tile chosenTile = GetTileAtPosition(randomVector2);
            if (chosenTile.isOccupied == false)
            {
                chosenTile.GetComponent<SpriteRenderer>().color = Color.magenta;
                chosenTile.isOccupied = true;
                chosenTile.placeableType = "obstacle";
                housesPositionList.Add(randomVector2);
            }
        }
    }

}