using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class TutorialButtonScript : MonoBehaviour
{
    [SerializeField] GameObject TutorialScreen;
    void ShowTutorialScreen()
    {
        TutorialScreen.SetActive(true);
    }
    void Start()
    {
        this.GetComponent<Button>().onClick.AddListener(ShowTutorialScreen);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
