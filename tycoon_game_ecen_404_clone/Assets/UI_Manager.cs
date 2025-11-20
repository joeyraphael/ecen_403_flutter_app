using System.Collections;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
public class UI_Manager : MonoBehaviour
{

    [SerializeField] GameObject MoneyText;
    [SerializeField] GameObject Player;

    [SerializeField] Button StoreButton;
    [SerializeField] Canvas StoreCanvas;
    [SerializeField] GameObject QuestionsRemainingText;
    [SerializeField] Button QuestionButton;

    [SerializeField] GameObject NotificationText;

    [SerializeField] Sprite Alert_Icon;
    [SerializeField] Sprite Information_Icon;

    [SerializeField] Button ZoomInButton;
    [SerializeField] Button ZoomOutButton;
    [SerializeField] public Camera _camera;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public void EnableStoreCanvas()
    {
        StoreCanvas.gameObject.SetActive(true);
    }
    void UpdateUI()
    {
        TextMeshProUGUI MoneyTextText = MoneyText.GetComponent<TextMeshProUGUI>();
        MoneyTextText.text = "Money: " + Player.GetComponent<PlayerManager>().money.ToString();
        StoreButton.GetComponent<Button>().onClick.AddListener(EnableStoreCanvas);
        QuestionsRemainingText.GetComponent<TextMeshProUGUI>().text = $"{Player.GetComponent<PlayerManager>().questionsNotAnsweredToday}/5"; // update UI to display how many questions the user has left to answer
        if (Player.GetComponent<PlayerManager>().questionsNotAnsweredToday == 0) // if the player has answered all the questions then disable the question UI
        {
            QuestionButton.interactable = false;
        }
        else
        {
            QuestionButton.interactable = true;
        }

    }

    public void SendNotification(string messageText, string messageType, Color messageColor) // Helper function for displaying notification, handles UI
    { // Takes a string for notification message, a string for the type of message, and desired colored of message
        NotificationText.gameObject.SetActive(true); // enable notification text game object
        NotificationText.GetComponent<TextMeshProUGUI>().text = messageText; // get text component of game object
        if (messageType == "alert")
        {
            NotificationText.transform.GetChild(0).gameObject.GetComponent<Image>().sprite = Alert_Icon; // get alert icon to display
            NotificationText.GetComponent<TextMeshProUGUI>().color = messageColor;
        }
        else if (messageType == "information")
        {
            NotificationText.transform.GetChild(0).gameObject.GetComponent<Image>().sprite = Information_Icon; // get information icon to display
            NotificationText.GetComponent<TextMeshProUGUI>().color = messageColor;
        }
        StartCoroutine(StartDelay());
    }

    public void ZoomScreen(int zoomAmount) // helper function to add or subtract from camera orthrographic size, which changes the zoom
    {
        GameObject.Find("Main Camera").GetComponent<IsometricCameraPan>()._currentZoom += zoomAmount;
        Debug.Log(GameObject.Find("Main Camera").GetComponent<IsometricCameraPan>()._currentZoom);
        Debug.Log(zoomAmount);
        //_camera.orthographicSize += zoomAmount;
        Debug.Log("Hello");
    }
    private IEnumerator StartDelay()
    {
        yield return new WaitForSeconds(2);
        NotificationText.gameObject.SetActive(false);
    }
    void Start()
    {
        ZoomInButton.onClick.AddListener(delegate { ZoomScreen(1); }); // add listeners to our buttons
        ZoomOutButton.onClick.AddListener(delegate { ZoomScreen(-1); });
    }

    // Update is called once per frame
    void Update()
    {
        UpdateUI();
    }
}
