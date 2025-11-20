using System;
using System.Collections;
using System.Collections.Generic;
//using Mono.Cecil.Cil;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class TimeManager : MonoBehaviour
{
    [SerializeField] GameObject TimeText;
    [SerializeField] GameObject StartTimeButton;
    [SerializeField] GameObject WinScreen;
    [SerializeField] int hour = 0;
    [SerializeField] int day = 0;
    int lengthOfTime;
    int[] timeSeconds;

    GridBuildingSystem gridBuildingSystem;

    void createTimeline(int lengthOfTime)
    {
        timeSeconds = new int[lengthOfTime];
        Array.Clear(timeSeconds, 0, timeSeconds.Length);
        for (int i = 0; i < lengthOfTime; i++)
        {
            timeSeconds[i] = i;
        }
    }

    private IEnumerator StartTimer()
    {
        TextMeshProUGUI text = TimeText.GetComponent<TextMeshProUGUI>();
        //int time = 0;
        while (true)
        {
            yield return new WaitForSeconds(0.2f);
            gridBuildingSystem.GenerateIncome();
            ++hour;
            if (hour == 25)
            {
                day++;
                hour = 0;
                text.text = $"Day: {day} Hour: {hour}";
                GameObject.Find("Player").GetComponent<PlayerManager>().questionsNotAnsweredToday = 5;
                StartTimeButton.GetComponent<Button>().interactable = true;
                GameObject.Find("StoreButton").GetComponent<Button>().interactable = true;
                GameObject.Find("WiringModeButton").GetComponent<Button>().interactable = true;
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
                        building.IncomeText.gameObject.SetActive(false);
                    }
                }

                if (houseCount == connectedCount)
                {
                    Debug.Log("Player has won!");
                    GameObject.Find("Player").GetComponent<PlayerManager>().score = (1 / day) * (100) + GameObject.Find("Player").GetComponent<PlayerManager>().questionsAnsweredCorrectly * 5000 + GameObject.Find("Player").GetComponent<PlayerManager>().money * 10;
                    Debug.Log(GameObject.Find("Player").GetComponent<PlayerManager>().score);
                    WinScreen.SetActive(true);
                    WinScreen.transform.Find("ScoreText").GetComponent<TextMeshProUGUI>().text = "Score: " + GameObject.Find("Player").GetComponent<PlayerManager>().score.ToString();
                }

                break;
            }

            Debug.Log($"Day: {day} Hour: {hour}");
            //Debug.Log($"{text.text}");
            text.text = $"Day: {day} Hour: {hour}";
        }
    }

    public void StartTimerWrapper()
    {
        StartTimeButton.GetComponent<Button>().interactable = false;
        GameObject.Find("StoreButton").GetComponent<Button>().interactable = false;
        GameObject.Find("WiringModeButton").GetComponent<Button>().interactable = false;
        StartCoroutine(StartTimer());
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        gridBuildingSystem = GameObject.Find("Grid").GetComponent<GridBuildingSystem>();
        createTimeline(5);
        /*for (int i = 0; i < timeSeconds.Length; i++)
        {
            Debug.Log(timeSeconds[i]);
        }*/
        StartTimeButton.GetComponent<Button>().onClick.AddListener(StartTimerWrapper);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
