using System;
using System.Collections;
using System.Collections.Generic;
//using Mono.Cecil.Cil;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Manages in-game time and the "day" cycle.
/// - Advances hours on a timer
/// - Triggers income generation each hour
/// - Resets daily limits (questions, buttons) at day change
/// - Checks win condition and shows win screen
/// </summary>
public class TimeManager : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] GameObject TimeText;        // Displays current day/hour
    [SerializeField] GameObject StartTimeButton; // Button used to start the timer
    [SerializeField] GameObject WinScreen;       // Win screen UI shown when the player wins

    [Header("Time State")]
    [SerializeField] int hour = 0;  // Current hour in the day (0–24)
    [SerializeField] int day = 0;   // Current day counter

    // Unused but kept for potential future use (timeline of seconds)
    int lengthOfTime;
    int[] timeSeconds;

    // Reference to the main grid system to trigger income
    GridBuildingSystem gridBuildingSystem;

    /// <summary>
    /// Creates an array of second values from 0 to lengthOfTime - 1.
    /// Currently unused, but could be used for more detailed time logic.
    /// </summary>
    void createTimeline(int lengthOfTime)
    {
        timeSeconds = new int[lengthOfTime];
        Array.Clear(timeSeconds, 0, timeSeconds.Length);

        for (int i = 0; i < lengthOfTime; i++)
        {
            timeSeconds[i] = i;
        }
    }

    /// <summary>
    /// Coroutine that advances time:
    /// - Every 0.2 seconds, generates income and advances hour
    /// - When hour reaches 25, a new day starts
    /// - Resets daily limits and checks win conditions
    /// </summary>
    private IEnumerator StartTimer()
    {
        TextMeshProUGUI text = TimeText.GetComponent<TextMeshProUGUI>();

        // Infinite loop until we manually break on day rollover
        while (true)
        {
            // Wait for "hour" tick (0.2 seconds in real time)
            yield return new WaitForSeconds(0.2f);

            // Trigger income for all buildings each hour
            gridBuildingSystem.GenerateIncome();

            ++hour;

            // If we've gone past hour 24, start a new day
            if (hour == 25)
            {
                day++;
                hour = 0;

                // Update time display
                text.text = $"Day: {day} Hour: {hour}";

                // Reset player daily question limit
                GameObject.Find("Player")
                    .GetComponent<PlayerManager>()
                    .questionsNotAnsweredToday = 5;

                // Re-enable key buttons for the new day
                StartTimeButton.GetComponent<Button>().interactable = true;
                GameObject.Find("StoreButton").GetComponent<Button>().interactable = true;
                GameObject.Find("WiringModeButton").GetComponent<Button>().interactable = true;

                // Check if all houses are connected to power
                int houseCount = 0;
                int connectedCount = 0;

                foreach (Building building in gridBuildingSystem.ListOfBuildings)
                {
                    if (building.buildingType == Building.BuildingType.House)
                    {
                        houseCount++;

                        if (building.isConnectedToPower == true)
                        {
                            connectedCount++;
                        }

                        // Hide income texts at the start of the new day
                        building.IncomeText.gameObject.SetActive(false);
                    }
                }

                // If all houses are connected, the player wins
                if (houseCount == connectedCount)
                {
                    Debug.Log("Player has won!");

                    PlayerManager player = GameObject.Find("Player").GetComponent<PlayerManager>();

                    // Score formula: based on day, correct answers, and money
                    player.score =
                        (1 / day) * 100 +
                        player.questionsAnsweredCorrectly * 5000 +
                        player.money * 10;

                    Debug.Log(player.score);

                    // Show win screen and score
                    WinScreen.SetActive(true);
                    WinScreen.transform.Find("ScoreText")
                        .GetComponent<TextMeshProUGUI>()
                        .text = "Score: " + player.score.ToString();
                }

                // End timer after finishing this day cycle
                break;
            }

            // Update time text every tick
            Debug.Log($"Day: {day} Hour: {hour}");
            text.text = $"Day: {day} Hour: {hour}";
        }
    }

    /// <summary>
    /// Wrapper for starting the timer:
    /// - Disables Buttons during the "day"
    /// - Starts the timer coroutine
    /// </summary>
    public void StartTimerWrapper()
    {
        StartTimeButton.GetComponent<Button>().interactable = false;
        GameObject.Find("StoreButton").GetComponent<Button>().interactable = false;
        GameObject.Find("WiringModeButton").GetComponent<Button>().interactable = false;

        StartCoroutine(StartTimer());
    }

    /// <summary>
    /// Unity Start method.
    /// Initializes references, creates a small timeline, and hooks up the start button.
    /// </summary>
    void Start()
    {
        gridBuildingSystem = GameObject.Find("Grid").GetComponent<GridBuildingSystem>();

        // Create a simple 5-step timeline (currently unused, but left for debugging/design)
        createTimeline(5);

        // Hook start button to start the timer
        StartTimeButton.GetComponent<Button>().onClick.AddListener(StartTimerWrapper);
    }

    // Currently unused; reserved for future real-time logic if needed
    void Update()
    {
    }
}
