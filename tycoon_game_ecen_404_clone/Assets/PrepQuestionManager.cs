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



public class PrepQuestionManager : MonoBehaviour
{
    [SerializeField] GameObject Player;
    [SerializeField] TextAsset PrepCardQuestionsJSONFile;
    [SerializeField] Button CloseQuestionButton;
    [SerializeField] Canvas QuestionCanvas;
    public List<string> questionsJson = new List<string>();
    public string question;
    public string answerA;
    public string answerB;
    public string answerC;
    public string answerD;
    public string correctAnswer;
    [SerializeField]
    public Button buttonA;
    
    [SerializeField]
    public Button buttonB;

    [SerializeField]
    public Button buttonC;

    [SerializeField]
    public Button buttonD;

    [SerializeField]

    public GameObject QuestionText;

    public void ReadJsonFile() {
        //Debug.Log("Size of list on start " + questionsJson.Count); //
        char[] charsToTrim = { '[', ']'};
        var testString = PrepCardQuestionsJSONFile.text;
        string result = testString.Remove(0, 1);
        string result2 = result.Remove(result.Length - 1, 1);
        //Debug.Log(result);
        //Debug.Log(result2);
        string[] subs = result2.Split('{', '}');
        foreach (var sub in subs)
        {
            //Debug.Log($"Substring: {sub}");
            if (sub.Contains('"') == true) {
                //Debug.Log($"Substring: {sub}");
                string subModified = sub;
                subModified += "}";
                subModified = subModified.Insert(0, "{");
                questionsJson.Add(subModified);

            }

       }

       //Debug.Log("Size of list now " + questionsJson.Count);
       //Debug.Log(questionsJson[0]);
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    public int GenerateQuestion()
    {
        var randomQuestionNumber = UnityEngine.Random.Range(0, questionsJson.Count);
        Debug.Log("Random Question Number: " + randomQuestionNumber);
        JsonUtility.FromJsonOverwrite(questionsJson[randomQuestionNumber], this);
        //Debug.Log(this.answerA);


        buttonA.onClick.RemoveAllListeners();
        buttonB.onClick.RemoveAllListeners();
        buttonC.onClick.RemoveAllListeners();
        buttonD.onClick.RemoveAllListeners();

        QuestionText.GetComponent<TMP_Text>().text = this.question;
        buttonA.GetComponentInChildren<TMP_Text>().text = this.answerA;
        buttonB.GetComponentInChildren<TMP_Text>().text = this.answerB;
        buttonC.GetComponentInChildren<TMP_Text>().text = this.answerC;
        buttonD.GetComponentInChildren<TMP_Text>().text = this.answerD;

        if (this.answerC == "" && this.answerD == "")
        {
            Color col = Color.white;
            col.a = 0;
            buttonC.GetComponent<Image>().color = col;
            buttonD.GetComponent<Image>().color = col;
            buttonC.interactable = false;
            buttonD.interactable = false;
        }

        buttonA.onClick.AddListener(delegate { CheckIfCorrect(buttonA, this.correctAnswer); });
        buttonB.onClick.AddListener(delegate { CheckIfCorrect(buttonB, this.correctAnswer); });
        buttonC.onClick.AddListener(delegate { CheckIfCorrect(buttonC, this.correctAnswer); });
        buttonD.onClick.AddListener(delegate { CheckIfCorrect(buttonD, this.correctAnswer); });

        return randomQuestionNumber;
    }
    
     public void CheckIfCorrect(Button button, string answer) {
        button.interactable = false;
        bool isAnswerCorrect = false;
        Debug.Log(button.name);
        switch (answer)
        {
            default:
                Debug.Log("idk");
                break;//return false;
            case "answerA":
                if (button.name == "ButtonA")
                {
                    Debug.Log("Correct Answer!");
                    button.GetComponent<Image>().color = Color.green;
                    isAnswerCorrect = true;
                    break;//return true;
                }
                else
                {
                    Debug.Log("Wrong Answer!");
                    button.GetComponent<Image>().color = Color.red;
                    button.interactable = false;
                    GameObject.Find("ButtonA").GetComponent<Image>().color = Color.green;
                    break;//return false;
                }
            case "answerB":
                if (button.name == "ButtonB")
                {
                    Debug.Log("Correct Answer!");
                    button.GetComponent<Image>().color = Color.green;
                    button.interactable = false;
                    isAnswerCorrect = true;
                    break;//return true;
                }
                else
                {
                    Debug.Log("Wrong Answer!");
                    button.GetComponent<Image>().color = Color.red;
                    button.interactable = false;
                    GameObject.Find("ButtonB").GetComponent<Image>().color = Color.green;
                    isAnswerCorrect = true;
                    break;//return false;
                }
            case "answerC":
                if (button.name == "ButtonC")
                {
                    Debug.Log("Correct Answer!");
                    button.GetComponent<Image>().color = Color.green;
                    button.interactable = false;
                    isAnswerCorrect = true;
                    break;//return true;
                }
                else
                {
                    Debug.Log("Wrong Answer!");
                    button.GetComponent<Image>().color = Color.red;
                    button.interactable = false;
                    GameObject.Find("ButtonC").GetComponent<Image>().color = Color.green;
                    break;//return false;
                }
            case "answerD":
                if (button.name == "ButtonD")
                {
                    Debug.Log("Correct Answer!");
                    button.GetComponent<Image>().color = Color.green;
                    button.interactable = false;
                    isAnswerCorrect = true;
                    break;//return true;
                }
                else
                {
                    Debug.Log("Wrong Answer!");
                    button.GetComponent<Image>().color = Color.red;
                    button.interactable = false;
                    GameObject.Find("ButtonD").GetComponent<Image>().color = Color.green;
                    break;//return false;
                }
        }

        if (isAnswerCorrect)
        {
            Player.GetComponent<PlayerManager>().AddMoney(50);
        }
        --Player.GetComponent<PlayerManager>().questionsNotAnsweredToday; 
        GameObject.Find("ButtonA").GetComponent<Button>().interactable = false;
        GameObject.Find("ButtonB").GetComponent<Button>().interactable = false;
        GameObject.Find("ButtonC").GetComponent<Button>().interactable = false;
        GameObject.Find("ButtonD").GetComponent<Button>().interactable = false;

        if (Player.GetComponent<PlayerManager>().questionsNotAnsweredToday > 0)
        {
            QuestionCanvas.transform.Find("QuestionsRemainingText2").GetComponent<TextMeshProUGUI>().text = $"{Player.GetComponent<PlayerManager>().questionsNotAnsweredToday}/5";
            StartCoroutine(StartDelay());
        }
        
    }
    public void ClearQuestion() { // Function to reset UI state of questions (i.e set buttons back to white, make them all interatactable)
        Debug.Log("I RANNNN");
        Color col = Color.white;
        col.a = 1;
        buttonA.GetComponent<Image>().color = Color.white;
        buttonB.GetComponent<Image>().color = Color.white;
        buttonC.GetComponent<Image>().color = Color.white;
        buttonD.GetComponent<Image>().color = Color.white;

        buttonC.GetComponent<Image>().color = col;
        buttonD.GetComponent<Image>().color = col;

        buttonA.interactable = true;
        buttonB.interactable = true;
        buttonC.interactable = true;
        buttonD.interactable = true;
    }

    public void CloseQuestionMenu()
    {
        QuestionCanvas.gameObject.SetActive(false);
        ClearQuestion();
        GenerateQuestion();
    }

    private IEnumerator StartDelay()
    {
        yield return new WaitForSeconds(2);
        ClearQuestion();
        GenerateQuestion();
    }

    void Start()
    {
        ReadJsonFile();
        GenerateQuestion();
        CloseQuestionButton.onClick.AddListener(delegate { CloseQuestionMenu(); });
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
