using System;
using System.Collections;
using System.Collections.Generic;
//using Mono.Cecil.Cil;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
public class PlayerManager : MonoBehaviour
{
    [SerializeField] public int money = 500;
    [SerializeField] public int day = 0;
    [SerializeField] public int score = 0;
    [SerializeField] public int questionsAnsweredCorrectly = 0;
    [SerializeField] public int questionsNotAnsweredToday = 5;
    [SerializeField] public List<string> questionsAnswered;

    [SerializeField] public string playerName = "DefaultPlayer";
    public void AddMoney(int moneyAmount)
    {
        money += moneyAmount;
    }


    private IEnumerator StartMoneyGen()
    {
        while (true)
        {
            yield return new WaitForSeconds(5);
            AddMoney(1);
            Debug.Log(money);
        }
    }

    public void StartMoneyGenWrapper()
    {
        StartCoroutine(StartMoneyGen());
    }

    IEnumerator Upload()
    {
        var formData = new List<IMultipartFormSection>();
        formData.Add(new MultipartFormDataSection("field1=foo&field2=bar"));
        formData.Add(new MultipartFormFileSection("my file data", "myfile.txt"));

        using var www = UnityWebRequest.Post("https://pastebin.com/api/api_post.php", formData);
        yield return www.SendWebRequest();

        if (www.result != UnityWebRequest.Result.Success)
        {
            Debug.Log(www.error);
        }
        else
        {
            Debug.Log("Form upload complete!");
        }
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //StartMoneyGenWrapper();
        //StartCoroutine(Upload());
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
