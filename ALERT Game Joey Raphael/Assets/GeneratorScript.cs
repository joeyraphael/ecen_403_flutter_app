using UnityEngine;

public class GeneratorScript : MonoBehaviour
{
    [SerializeField] public int power = 0;
    [SerializeField] public bool isConnected = false;
    [SerializeField] public bool isOn = true;

    public void SetType() { }

    public void SetConnection(bool state)
    {
        isConnected = state;
    }

    public void SetOn(bool state)
    {
        isOn = state;    
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
