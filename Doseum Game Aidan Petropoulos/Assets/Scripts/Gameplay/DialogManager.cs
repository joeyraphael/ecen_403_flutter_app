using System; // for Action
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; // using UI text

// this handles the lil popup dialog box + typing effect
public class DialogManager : MonoBehaviour
{
    [SerializeField] GameObject dialogBox; // whole UI box
    [SerializeField] Text dialogText; // where the words go
    [SerializeField] int lettersPerSecond; // typing speed

    public event Action OnShowDialog; // fire when dialog opens
    public event Action OnCloseDialog; // fire when dialog closes

    public static DialogManager Instance { get; private set; } // singleton

    private void Awake()
    {
        Instance = this; // store myself as the instance
    }

    Dialog dialog; // current dialog data
    Action onDialogFinished; // callback when dialog done
    int currentLine = 0; // which line i'm on
    bool isTyping; // typing effect in-progress

    public bool IsShowing { get; private set; } // if dialog is currently up

    public IEnumerator ShowDialog(Dialog dialog, Action onFinished = null)
    {
        yield return new WaitForEndOfFrame(); // wait 1 frame bc unity timing weird sometimes

        OnShowDialog?.Invoke(); // tell anyone who cares "hey dialog is starting"

        IsShowing = true; // mark dialog open
        this.dialog = dialog; // save the dialog asset
        onDialogFinished = onFinished; // save callback

        dialogBox.SetActive(true); // show the ui box
        StartCoroutine(TypeDialog(dialog.Lines[0])); // type first line
    }

    public void HandleUpdate()
    {
        // if player presses Z or clicks AND we're not typing rn
        if ((Input.GetKeyDown(KeyCode.Z) || Input.GetMouseButtonDown(0)) && !isTyping)
        {
            ++currentLine; // go to next line

            if (currentLine < dialog.Lines.Count) // still more lines left
            {
                StartCoroutine(TypeDialog(dialog.Lines[currentLine])); // type next line
            }
            else
            {
                // finished dialog
                currentLine = 0; // reset
                IsShowing = false; // hide state
                dialogBox.SetActive(false); // hide UI
                onDialogFinished?.Invoke(); // call callback if exists
                OnCloseDialog?.Invoke(); // broadcast dialog closed
            }
        }
    }

    public IEnumerator TypeDialog(string line)
    {
        isTyping = true; // so player cant skip while letters load
        dialogText.text = ""; // clear text

        // type each letter one-by-one
        foreach (var letter in line.ToCharArray())
        {
            dialogText.text += letter;
            yield return new WaitForSeconds(1f / lettersPerSecond); // typing speed
        }

        isTyping = false; // done typing that line
    }
}
