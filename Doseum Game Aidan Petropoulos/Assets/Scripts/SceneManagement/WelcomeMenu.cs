using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class WelcomeMenu : MonoBehaviour
{
    [SerializeField] private Button startButton;
    [SerializeField] private Button announcementsButton;
    [SerializeField] private string mainSceneName = "YourMainSceneName"; // change this

    void Start()
    {
        startButton.onClick.AddListener(OnStartClicked);

    }

    void OnStartClicked()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(mainSceneName);
        Debug.Log("Loaded scene: " + mainSceneName);

    }

    void OnAnnouncementsClicked()
    {
        // Example: show a popup panel or load another scene
        Debug.Log("Announcements clicked!");
    }
}
