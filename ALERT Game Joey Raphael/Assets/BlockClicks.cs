using UnityEngine;
using UnityEngine.EventSystems;

public class BlockClicks : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public GridBuildingSystem gridBuildingSystem;
    public void OnPointerEnter(PointerEventData eventData)
    {
        gridBuildingSystem.IsOverUIElement = true;
        Debug.Log("Hi!");
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        gridBuildingSystem.IsOverUIElement = false;
        Debug.Log("Bye!");
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        gridBuildingSystem = GameObject.Find("Grid").GetComponent<GridBuildingSystem>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
