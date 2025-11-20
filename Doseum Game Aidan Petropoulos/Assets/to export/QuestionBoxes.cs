using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class QuestionBoxes : MonoBehaviour
{
    [SerializeField] private Text dialogText;
    [SerializeField] private Text[] actionTexts;  // Size 3 in Inspector
    [SerializeField] private GameObject actionSelector;
    [SerializeField] Color highlightedColor = Color.yellow;

    public Coroutine StartTyping(string dialog)
    {
        Debug.Log($"StartTyping: {dialog}");
        StopAllCoroutines();
        if (dialogText != null)
        {
            return StartCoroutine(TypeDialog(dialog));
        }
        Debug.LogError("dialogText is null!");
        return null;
    }

    private IEnumerator TypeDialog(string dialog)
    {
        dialogText.text = "";
        foreach (char letter in dialog)
        {
            dialogText.text += letter;
            yield return new WaitForSeconds(0.05f);
        }
    }

    public void EnableActionSelector(bool enabled)
    {
        if (actionSelector != null) actionSelector.SetActive(enabled);
        else Debug.LogWarning("actionSelector is null!");
    }

    public void UpdateActionSelection(int selectedAction)
    {
        if (actionTexts != null && actionTexts.Length >= 3)
        {
            for (int i = 0; i < 3; i++)
            {
                if (actionTexts[i] != null)
                {
                    actionTexts[i].color = (i == selectedAction) ? highlightedColor : Color.black;
                    Debug.Log($"Set answer {i} color to {(i == selectedAction ? "highlightedColor (yellow)" : "black")} for text: {actionTexts[i].text}, GameObject: {actionTexts[i].gameObject.name}");
                }
            }
        }
        else
        {
            Debug.LogWarning("actionTexts invalid!");
        }
    }

    public void SetDialog(string dialog)
    {
        dialogText.text = dialog;
    }

    public void SetAnswerTexts(string[] answers)
    {
        Debug.Log($"SetAnswerTexts: {string.Join(", ", answers)}");
        if (actionTexts != null && actionTexts.Length >= 3 && answers != null && answers.Length >= 3)
        {
            for (int i = 0; i < 3; i++)
            {
                if (actionTexts[i] != null) actionTexts[i].text = answers[i];
            }
        }
        else
        {
            Debug.LogWarning("Cannot set answers: actionTexts or answers invalid!");
        }
    }

    // New method to expose actionTexts for click detection
    public Text[] GetAnswerTexts()
    {
        return actionTexts;
    }
}