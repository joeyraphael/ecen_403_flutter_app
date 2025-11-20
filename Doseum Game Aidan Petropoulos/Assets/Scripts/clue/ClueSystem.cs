using System.Collections; // unity stuff
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems; // for UI raycasts
using UnityEngine.UI; // for Text UI

// states the clue box can be in
public enum ClueState { Start, PlayerAction, PlayerMove, Busy }

public class ClueSystem : MonoBehaviour
{
    [SerializeField] private QuestionBoxes dialogBox; // the UI box i made
    [SerializeField] private GameController gameController; // so i can end clues

    private ClueState state; // what the clue system is doing rn
    int currAction; // which answer player is on
    private bool lastAnswerWasWrong; // so player can retry
    private string currentQuestion; // saved q
    private string[] currentAnswers; // saved answers
    private int correctAnswerIndex; // which one is right
    private string currentClueID; // the id for this clue
    private bool isFirstTry; // for scoring

    private bool inputLocked = false; // stops double taps on mobile

    // clue only finishes once so no repeat farming points lol
    private bool clueCompleted = false;

    private void Awake()
    {
        gameObject.SetActive(false); // hide clue box at start

        if (dialogBox == null)
        {
            // i probably forgot to drag the object into inspector lol
        }
    }

    public void HandleUpdate()
    {
        // picking answers
        if (state == ClueState.PlayerAction)
        {
            HandleActionSelection(); // handles keyboard ↑↓

            // touch input for answers
            if (Input.touchCount > 0)
            {
                Touch t = Input.GetTouch(0);
                if (t.phase == TouchPhase.Began)
                {
                    Vector2 touchPos = t.position;

                    int answerIndex = CheckForUIAnswerClick(touchPos); // see which answer they hit
                    if (answerIndex >= 0)
                    {
                        currAction = answerIndex;
                        if (dialogBox != null)
                            dialogBox.UpdateActionSelection(currAction);

                        if (inputLocked) return; // avoid double clicking
                        inputLocked = true;

                        if (currAction == correctAnswerIndex) CorrectAnswer();
                        else WrongAnswer();
                    }
                }
            }
            // mouse input (for editor/webgl)
            else if (Input.GetMouseButtonDown(0))
            {
                Vector2 mousePos = Input.mousePosition;

                int answerIndex = CheckForUIAnswerClick(mousePos);
                if (answerIndex >= 0)
                {
                    currAction = answerIndex;

                    if (dialogBox != null)
                        dialogBox.UpdateActionSelection(currAction);

                    if (inputLocked) return;
                    inputLocked = true;

                    if (currAction == correctAnswerIndex) CorrectAnswer();
                    else WrongAnswer();
                }
            }
        }
        // after wrong answer, let the player press anything to retry
        else if (state == ClueState.PlayerMove && lastAnswerWasWrong)
        {
            if (Input.GetKeyDown(KeyCode.R)) // keyboard retry
                PlayerAction();

            if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began) // phone retry
                PlayerAction();

            else if (Input.GetMouseButtonDown(0)) // mouse retry
                PlayerAction();
        }
    }

    private int CheckForUIAnswerClick(Vector2 screenPosition)
    {
        if (dialogBox == null) return -1;

        // grab the UI text objects from my dialog box
        Text[] answerTexts = dialogBox.GetAnswerTexts();
        if (answerTexts == null || answerTexts.Length < 3) return -1;

        // create a fake pointer event to raycast UI
        PointerEventData eventData = new PointerEventData(EventSystem.current)
        {
            position = screenPosition
        };

        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventData, results); // check what UI objects finger hit

        // loop thru hits and see if any of them match the answer text objects
        foreach (var r in results)
        {
            GameObject hit = r.gameObject;

            for (int i = 0; i < answerTexts.Length; i++)
            {
                if (answerTexts[i] != null && hit == answerTexts[i].gameObject)
                    return i; // return which answer was clicked
            }
        }

        return -1; // nothing clicked
    }

    public void ResetAndStartClueSystem(string question, string[] answers, int correctIndex, string clueID)
    {
        // reset everything for a new clue
        state = ClueState.Start;
        currAction = 0;
        lastAnswerWasWrong = false;
        isFirstTry = true;
        inputLocked = false;
        clueCompleted = false;
        StopAllCoroutines(); // clean slate

        // make sure clue has an ID
        if (string.IsNullOrEmpty(clueID))
        {
            clueID = "Clue_" + gameObject.GetInstanceID(); // fallback id
        }
        else if (clueID == "DefaultClue")
        {
            // if i forgot to give it a real ID
        }

        currentClueID = clueID;
        correctAnswerIndex = correctIndex;
        currentQuestion = question;
        currentAnswers = answers;

        if (dialogBox != null)
        {
            dialogBox.StartTyping(question); // type out question
            dialogBox.SetAnswerTexts(answers); // show answers
            dialogBox.EnableActionSelector(true); // highlight options
        }

        currAction = 0;
        state = ClueState.PlayerAction; // let player answer now
        isFirstTry = true;

        if (dialogBox != null)
            dialogBox.UpdateActionSelection(currAction);
    }

    private IEnumerator ClueSetup()
    {
        if (dialogBox != null)
        {
            dialogBox.StartTyping("A clue has appeared!");
            yield return new WaitForSeconds(1f);
            PlayerAction();
        }
    }

    private void PlayerAction()
    {
        state = ClueState.PlayerAction;
        inputLocked = false; // let inputs register again

        if (dialogBox != null)
        {
            // safety check
            if (string.IsNullOrEmpty(currentQuestion) || currentAnswers == null)
                return;

            dialogBox.EnableActionSelector(false); // hide cursor for a sec
            dialogBox.SetDialog("");
            dialogBox.StartTyping(currentQuestion); // show question again
            dialogBox.SetAnswerTexts(currentAnswers);
            dialogBox.EnableActionSelector(true);

            currAction = 0;
            dialogBox.UpdateActionSelection(currAction);
        }
    }

    void HandleActionSelection()
    {
        // keyboard controls for scrolling answers
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            if (currAction < 2) currAction++;
        }
        else if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            if (currAction > 0) currAction--;
        }

        if (dialogBox != null)
            dialogBox.UpdateActionSelection(currAction);
    }

    private void CorrectAnswer()
    {
        state = ClueState.PlayerMove;
        StartCoroutine(DisplayCorrectAnswer());
    }

    private IEnumerator DisplayCorrectAnswer()
    {
        // only score the clue once
        if (clueCompleted)
            yield break;
        clueCompleted = true;

        yield return new WaitForSeconds(0.5f);

        if (dialogBox != null)
        {
            dialogBox.StartTyping("Correct!"); // show correct message
            dialogBox.EnableActionSelector(false);
        }

        if (ScoreManager.Instance != null)
            ScoreManager.Instance.AddClueFound(isFirstTry); // score logic

        yield return new WaitForSeconds(1f);

        inputLocked = false;

        if (gameController != null)
        {
            gameController.solvedClues.Add(currentClueID); // mark clue as done
            gameController.EndClue(); // exit clue mode
        }
    }

    private void WrongAnswer()
    {
        state = ClueState.PlayerMove;

        if (dialogBox != null)
        {
            dialogBox.StartTyping("Wrong! Press the screen to try again."); // retry msg
            dialogBox.EnableActionSelector(false);
        }

        lastAnswerWasWrong = true;
        isFirstTry = false; // no bonus now

        inputLocked = false; // allow retry
    }

    public void ShowAlreadyFoundMessage()
    {
        StartCoroutine(DisplayAlreadyFoundMessage());
    }

    private IEnumerator DisplayAlreadyFoundMessage()
    {
        if (dialogBox != null)
            yield return dialogBox.StartTyping("You already solved this clue.");

        yield return new WaitForSeconds(1f);

        if (gameController != null)
            gameController.EndClue();
    }
}
