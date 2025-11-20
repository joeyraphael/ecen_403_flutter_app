//using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayGameScript : MonoBehaviour
{
    string SceneName;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void StartGameScene()
    {
        SceneManager.LoadScene("Test");
    }
    void Start()
    {
        this.GetComponent<Button>().onClick.AddListener(StartGameScene);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
