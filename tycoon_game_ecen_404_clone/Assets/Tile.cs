using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    [SerializeField] private Color _baseColor, _offsetColor;
    [SerializeField] private SpriteRenderer _renderer;
    [SerializeField] private GameObject _highlight;

    public bool isOccupied = false;
    public string placeableType = "null";

    public void Init(bool isOffset)
    {
        _renderer.color = isOffset ? _offsetColor : _baseColor;

    }

    public void OccupyTile(bool isOccupiedParameter)
    {
        isOccupied = isOccupiedParameter;
    }

    void OnMouseEnter()
    {
        _highlight.SetActive(true);
        //Debug.Log(this.name);
    }

    void OnMouseExit()
    {
        _highlight.SetActive(false);
    }
    void OnMouseUp()
    {

        Debug.Log("I was clicked " + this.name);
        var wiringModeActive = GameObject.Find("GameManager").GetComponent<GridManager>().wiringModeActive;
        if (wiringModeActive == false)
        {
            Debug.Log("Wiring mode not active, cannot place wire!");
        }
        else if (placeableType == "obstacle")
        {
            Debug.Log("Tile is already occupied by an obstacle, cannot place wire!");
        }
        else if (placeableType == "wire")
        {
            this.GetComponent<SpriteRenderer>().color = Color.green;
            placeableType = "null";
        }
        else
        {
            this.GetComponent<SpriteRenderer>().color = Color.blue;
            placeableType = "wire";
        }
    }
}