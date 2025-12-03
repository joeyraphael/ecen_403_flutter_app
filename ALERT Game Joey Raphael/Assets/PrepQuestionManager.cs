using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using TMPro;
using NUnit.Framework;
using Unity.VisualScripting;
using System.Collections.Generic;
using UnityEngine.UI;
using System;
using System.Collections;

// Manages loading, showing, and checking multiple-choice “prep” questions
public class PrepQuestionManager : MonoBehaviour
{
    // Reference to the Player object so we can access PlayerManager (money, questions left, etc.)
    [SerializeField] GameObject Player;

    // JSON file that contains all the question data
    [SerializeField] TextAsset PrepCardQuestionsJSONFile;

    // Button to close the question menu
    [SerializeField] Button CloseQuestionButton;

    // Canvas that displays the question UI
    [SerializeField] Canvas QuestionCanvas;

    // List of raw JSON strings, each string representing one question object
    public List<string> questionsJson = new List<string>();

    // Fields that get filled by JsonUtility.FromJsonOverwrite()
    public string question;
    public string answerA;
    public string answerB;
    public string answerC;
    public string answerD;
    public string correctAnswer;

    // Buttons for each answer option
    [SerializeField]
    public Button buttonA;
    
    [SerializeField]
    public Button buttonB;

    [SerializeField]
    public Button buttonC;

    [SerializeField]
    public Button buttonD;

    // Text object used to display the current question
    [SerializeField]
    public GameObject QuestionText;

    /// <summary>
    /// Reads the JSON file and manually splits it into individual question JSON strings,
    /// storing each one in questionsJson.
    /// </summary>
    public void ReadJsonFile() {
        // Example JSON file format is expected to be an array: [ { ... }, { ... }, ... ]

        char[] charsToTrim = { '[', ']'};
        var testString = PrepCardQuestionsJSONFile.text;

        // Remove the first '['
        string result = testString.Remove(0, 1);

        // Remove the trailing ']' from the end
        string result2 = result.Remove(result.Length - 1, 1);

        // Split the JSON into chunks between braces
        string[] subs = result2.Split('{', '}');

        foreach (var sub in subs)
        {
            // If the chunk contains quotes, it likely has data
            if (sub.Contains('"') == true) {
                string subModified = sub;

                // Re-wrap the chunk with '{' and '}' to make it a valid JSON object
                subModified += "}";
                subModified = subModified.Insert(0, "{");

                // Add this question JSON string to the list
                questionsJson.Add(subModified);
            }
        }
    }

    /// <summary>
    /// Picks a random question from questionsJson, applies it to this component via JSON,
    /// and updates the UI (question text and answer button labels).
    /// Also sets up button listeners to check answers.
    /// </summary>
    /// <returns>The index of the randomly chosen question.</returns>
    public int GenerateQuestion()
    {
        // Choose a random index within the list of questions
        var randomQuestionNumber = UnityEngine.Random.Range(0, questionsJson.Count);
        Debug.Log("Random Question Number: " + randomQuestionNumber);

        // Populate this object's fields (question, answers, correctAnswer) from the JSON
        JsonUtility.FromJsonOverwrite(questionsJson[randomQuestionNumber], this);

        // Clear any previous listeners to avoid stacking them
        buttonA.onClick.RemoveAllListeners();
        buttonB.onClick.RemoveAllListeners();
        buttonC.onClick.RemoveAllListeners();
        buttonD.onClick.RemoveAllListeners();

        // Update UI text with the current question and answers
        QuestionText.GetComponent<TMP_Text>().text = this.question;
        buttonA.GetComponentInChildren<TMP_Text>().text = this.answerA;
        buttonB.GetComponentInChildren<TMP_Text>().text = this.answerB;
        buttonC.GetComponentInChildren<TMP_Text>().text = this.answerC;
        buttonD.GetComponentInChildren<TMP_Text>().text = this.answerD;

        // If C and D are empty, hide/disable those buttons (effectively 2-option question)
        if (this.answerC == "" && this.answerD == "")
        {
            Color col = Color.white;
            col.a = 0; // fully transparent

            buttonC.GetComponent<Image>().color = col;
            buttonD.GetComponent<Image>().color = col;

            buttonC.interactable = false;
            buttonD.interactable = false;
        }

        // Add listeners for each answer button that call CheckIfCorrect
        buttonA.onClick.AddListener(delegate { CheckIfCorrect(buttonA, this.correctAnswer); });
        buttonB.onClick.AddListener(delegate { CheckIfCorrect(buttonB, this.correctAnswer); });
        buttonC.onClick.AddListener(delegate { CheckIfCorrect(buttonC, this.correctAnswer); });
        buttonD.onClick.AddListener(delegate { CheckIfCorrect(buttonD, this.correctAnswer); });

        return randomQuestionNumber;
    }
    
    /// <summary>
    /// Checks if the clicked button corresponds to the correct answer.
    /// Updates button colors (green for correct, red for incorrect),
    /// gives the player money if correct, decrements questions left,
    /// and triggers next question if any remain.
    /// </summary>
    /// <param name="button">The button that was clicked.</param>
    /// <param name="answer">The correct answer key (e.g. "answerA").</param>
    public void CheckIfCorrect(Button button, string answer) {
        // Disable the clicked button to prevent double-clicking
        button.interactable = false;
        bool isAnswerCorrect = false;

        Debug.Log(button.name);

        // Compare the correctAnswer string (answerA/B/C/D) with which button was pressed
        switch (answer)
        {
            default:
                Debug.Log("idk");
                break;

            case "answerA":
                if (button.name == "ButtonA")
                {
                    // Correct
                    Debug.Log("Correct Answer!");
                    button.GetComponent<Image>().color = Color.green;
                    isAnswerCorrect = true;
                    break;
                }
                else
                {
                    // Incorrect
                    Debug.Log("Wrong Answer!");
                    button.GetComponent<Image>().color = Color.red;
                    button.interactable = false;

                    // Highlight correct button
                    GameObject.Find("ButtonA").GetComponent<Image>().color = Color.green;
                    break;
                }

            case "answerB":
                if (button.name == "ButtonB")
                {
                    Debug.Log("Correct Answer!");
                    button.GetComponent<Image>().color = Color.green;
                    button.interactable = false;
                    isAnswerCorrect = true;
                    break;
                }
                else
                {
                    Debug.Log("Wrong Answer!");
                    button.GetComponent<Image>().color = Color.red;
                    button.interactable = false;
                    GameObject.Find("ButtonB").GetComponent<Image>().color = Color.green;
                    isAnswerCorrect = true; // (Note: currently set true here too)
                    break;
                }

            case "answerC":
                if (button.name == "ButtonC")
                {
                    Debug.Log("Correct Answer!");
                    button.GetComponent<Image>().color = Color.green;
                    button.interactable = false;
                    isAnswerCorrect = true;
                    break;
                }
                else
                {
                    Debug.Log("Wrong Answer!");
                    button.GetComponent<Image>().color = Color.red;
                    button.interactable = false;
                    GameObject.Find("ButtonC").GetComponent<Image>().color = Color.green;
                    break;
                }

            case "answerD":
                if (button.name == "ButtonD")
                {
                    Debug.Log("Correct Answer!");
                    button.GetComponent<Image>().color = Color.green;
                    button.interactable = false;
                    isAnswerCorrect = true;
                    break;
                }
                else
                {
                    Debug.Log("Wrong Answer!");
                    button.GetComponent<Image>().color = Color.red;
                    button.interactable = false;
                    GameObject.Find("ButtonD").GetComponent<Image>().color = Color.green;
                    break;
                }
        }

        // Reward the player with money if they got the answer correct
        if (isAnswerCorrect)
        {
            Player.GetComponent<PlayerManager>().AddMoney(50);
        }

        // Decrement the number of questions remaining today
        --Player.GetComponent<PlayerManager>().questionsNotAnsweredToday; 

        // Disable all buttons after a choice is made
        GameObject.Find("ButtonA").GetComponent<Button>().interactable = false;
        GameObject.Find("ButtonB").GetComponent<Button>().interactable = false;
        GameObject.Find("ButtonC").GetComponent<Button>().interactable = false;
        GameObject.Find("ButtonD").GetComponent<Button>().interactable = false;

        // If there are still questions left, update UI and prepare next question after a delay
        if (Player.GetComponent<PlayerManager>().questionsNotAnsweredToday > 0)
        {
            QuestionCanvas.transform.Find("QuestionsRemainingText2")
                .GetComponent<TextMeshProUGUI>()
                .text = $"{Player.GetComponent<PlayerManager>().questionsNotAnsweredToday}/5";

            StartCoroutine(StartDelay());
        }
    }

    /// <summary>
    /// Resets the visual state of all answer buttons:
    /// - Restores white color and full opacity
    /// - Makes all buttons interactable
    /// This prepares the UI for the next question.
    /// </summary>
    public void ClearQuestion() {
        Debug.Log("I RANNNN");

        Color col = Color.white;
        col.a = 1;

        // Reset colors
        buttonA.GetComponent<Image>().color = Color.white;
        buttonB.GetComponent<Image>().color = Color.white;
        buttonC.GetComponent<Image>().color = Color.white;
        buttonD.GetComponent<Image>().color = Color.white;

        buttonC.GetComponent<Image>().color = col;
        buttonD.GetComponent<Image>().color = col;

        // Make all buttons interactable again
        buttonA.interactable = true;
        buttonB.interactable = true;
        buttonC.interactable = true;
        buttonD.interactable = true;
    }

    /// <summary>
    /// Hides the question canvas and resets/loads the next question.
    /// Called when the “Close” button is pressed.
    /// </summary>
    public void CloseQuestionMenu()
    {
        QuestionCanvas.gameObject.SetActive(false);
        ClearQuestion();
        GenerateQuestion();
    }

    /// <summary>
    /// Waits for 2 seconds, then clears the current question state and
    /// immediately generates a new question.
    /// Used to give players time to see if they were correct.
    /// </summary>
    private IEnumerator StartDelay()
    {
        yield return new WaitForSeconds(2);
        ClearQuestion();
        GenerateQuestion();
    }

    /// <summary>
    /// Unity Start method.
    /// Initializes questions from JSON, generates the first question,
    /// and hooks up the close button.
    /// </summary>
    void Start()
    {
        ReadJsonFile();
        GenerateQuestion();
        CloseQuestionButton.onClick.AddListener(delegate { CloseQuestionMenu(); });
    }

    // Update is called once per frame (currently not used)
    void Update()
    {
        
    }
}
