using System.Collections;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class UI_Manager : MonoBehaviour
{
    // Reference to the Text object that shows the player's money (TextMeshProUGUI component expected)
    [SerializeField] GameObject MoneyText;

    // Reference to the player object so we can read data from PlayerManager (money, questions, etc.)
    [SerializeField] GameObject Player;

    // Button that opens the in-game store
    [SerializeField] Button StoreButton;

    // The store's UI canvas (disabled until opened)
    [SerializeField] Canvas StoreCanvas;

    // Text object that shows how many questions remain today
    [SerializeField] GameObject QuestionsRemainingText;

    // Button used to open/answer a question
    [SerializeField] Button QuestionButton;

    // Notification text object used for alerts / info messages (has a child Image for icon)
    [SerializeField] GameObject NotificationText;

    // Sprite shown when the notification type is an alert
    [SerializeField] Sprite Alert_Icon;

    // Sprite shown when the notification type is informational
    [SerializeField] Sprite Information_Icon;

    // UI buttons to control zooming in and out
    [SerializeField] Button ZoomInButton;
    [SerializeField] Button ZoomOutButton;

    // Reference to the main camera (optional; currently not used directly in ZoomScreen)
    [SerializeField] public Camera _camera;


    /// <summary>
    /// Enables the store canvas so the player can see and interact with the store UI.
    /// Called when the Store button is clicked.
    /// </summary>
    public void EnableStoreCanvas()
    {
        StoreCanvas.gameObject.SetActive(true);
    }

    /// <summary>
    /// Updates various UI elements based on current player state:
    /// - Money display
    /// - Questions remaining
    /// - Question button interactable state
    /// NOTE: This is currently called every frame from Update().
    /// </summary>
    void UpdateUI()
    {
        // Update money text
        TextMeshProUGUI MoneyTextText = MoneyText.GetComponent<TextMeshProUGUI>();
        MoneyTextText.text = "Money: " + Player.GetComponent<PlayerManager>().money.ToString();

        // Make sure the Store button opens the store
        // NOTE: This adds a listener every frame; typically this should be done once in Start().
        StoreButton.GetComponent<Button>().onClick.AddListener(EnableStoreCanvas);

        // Update questions remaining text (e.g., "3/5")
        QuestionsRemainingText
            .GetComponent<TextMeshProUGUI>()
            .text = $"{Player.GetComponent<PlayerManager>().questionsNotAnsweredToday}/5";

        // If no questions remain, disable the question button; otherwise enable it
        if (Player.GetComponent<PlayerManager>().questionsNotAnsweredToday == 0)
        {
            QuestionButton.interactable = false;
        }
        else
        {
            QuestionButton.interactable = true;
        }
    }

    /// <summary>
    /// Displays a notification on screen with text, type (alert / information),
    /// and a color. Automatically hides after a short delay.
    /// </summary>
    /// <param name="messageText">The text to show in the notification.</param>
    /// <param name="messageType">"alert" or "information" (controls icon used).</param>
    /// <param name="messageColor">Color of the notification text.</param>
    public void SendNotification(string messageText, string messageType, Color messageColor)
    {
        // Ensure notification object is visible
        NotificationText.gameObject.SetActive(true);

        // Set notification text
        NotificationText.GetComponent<TextMeshProUGUI>().text = messageText;

        // Choose icon and apply color based on message type
        if (messageType == "alert")
        {
            // Set alert icon
            NotificationText.transform.GetChild(0)
                .gameObject
                .GetComponent<Image>()
                .sprite = Alert_Icon;

            NotificationText.GetComponent<TextMeshProUGUI>().color = messageColor;
        }
        else if (messageType == "information")
        {
            // Set information icon
            NotificationText.transform.GetChild(0)
                .gameObject
                .GetComponent<Image>()
                .sprite = Information_Icon;

            NotificationText.GetComponent<TextMeshProUGUI>().color = messageColor;
        }

        // Start coroutine to hide the notification after a delay
        StartCoroutine(StartDelay());
    }

    /// <summary>
    /// Adjusts the zoom level by changing _currentZoom on the IsometricCameraPan
    /// script attached to the "Main Camera" object.
    /// Positive zoomAmount zooms in, negative zoomAmount zooms out.
    /// </summary>
    /// <param name="zoomAmount">Amount to add to the current zoom value.</param>
    public void ZoomScreen(int zoomAmount)
    {
        // Find the main camera object and modify its zoom through IsometricCameraPan
        GameObject.Find("Main Camera").GetComponent<IsometricCameraPan>()._currentZoom += zoomAmount;

        // Debug logs to verify zoom behavior
        Debug.Log(GameObject.Find("Main Camera").GetComponent<IsometricCameraPan>()._currentZoom);
        Debug.Log(zoomAmount);

        // Alternative method (commented out): directly modifying camera's orthographic size
        //_camera.orthographicSize += zoomAmount;

        Debug.Log("Hello");
    }

    /// <summary>
    /// Coroutine that waits for 2 seconds and then hides the notification text.
    /// </summary>
    private IEnumerator StartDelay()
    {
        yield return new WaitForSeconds(2);
        NotificationText.gameObject.SetActive(false);
    }

    /// <summary>
    /// Unity's Start method.
    /// Sets up button listeners for zoom controls.
    /// </summary>
    void Start()
    {
        // Zoom in when ZoomInButton is clicked
        ZoomInButton.onClick.AddListener(delegate { ZoomScreen(1); });

        // Zoom out when ZoomOutButton is clicked
        ZoomOutButton.onClick.AddListener(delegate { ZoomScreen(-1); });
    }

    /// <summary>
    /// Unity's Update method.
    /// Called once per frame; updates the UI every frame.
    /// </summary>
    void Update()
    {
        UpdateUI();
    }
}
