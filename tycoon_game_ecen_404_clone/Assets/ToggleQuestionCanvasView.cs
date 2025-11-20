using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using TMPro;
using NUnit.Framework;
using Unity.VisualScripting;
using System.Collections.Generic;
using UnityEngine.UI;
using System;
public class ToggleQuestionCanvasView : MonoBehaviour
{
    [SerializeField]public GameObject QuestionCanvas;

    void ToggleQuestionCanvasViewFunction() {
        QuestionCanvas.SetActive(true);
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
     this.GetComponent<Button>().onClick.AddListener(delegate {ToggleQuestionCanvasViewFunction();});   
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
