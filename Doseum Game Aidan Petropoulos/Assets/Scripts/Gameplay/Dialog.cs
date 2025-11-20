using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Dialog
{
    [SerializeField] List<string> lines;

    public List<string> Lines {
        get { return lines; }
    }

    // Add this constructor
    public Dialog(string text)
    {
        lines = new List<string> { text };
    }

    // Optional: Explicit default constructor (not strictly needed since C# provides it)
    public Dialog()
    {
        lines = new List<string>();
    }
}