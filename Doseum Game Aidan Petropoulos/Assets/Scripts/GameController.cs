using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


// All the states the game can be in. Helps me track what the player is allowed to do.
public enum GameState { FreeRoam, Clue, Dialog, Menu, Bag, Cutscene, Paused }

public class GameController : MonoBehaviour
{
    // Drag-and-drop refs for the main systems this script controls.
    [SerializeField] PlayerController playerController;
    [SerializeField] ClueSystem clueSystem;
    [SerializeField] Camera worldCamera;

    [SerializeField] Camera clueCamera;
    [SerializeField] private DialogManager dialogManager;

    // Tracks current game state
    GameState state;
    GameState stateBeforePause;

    // Tracks which clues the player has solved so they don’t repeat
    public HashSet<string> solvedClues = new HashSet<string>();

    // Track scene transitions
    public SceneDetails CurrentScene { get; private set; }
    public SceneDetails PrevScene { get; private set; }

    MenuController menuController;

    // Singleton access
    public static GameController Instance { get; private set; }


    private void Awake()
    {
        // If we’re in the Welcome scene, this controller shouldn’t run at all.
        if (SceneManager.GetActiveScene().name == "WelcomeScene")
        {
            Debug.Log("GameController disabled in Welcome Scene");
            enabled = false;
            return;
        }

        // Standard singleton setup, but done safely so duplicates never survive.
        if (Instance == null)
        {
            Instance = this;

            // Check if somehow multiple GameControllers exist (Unity loves duplicating these during load)
            GameController[] controllers = FindObjectsOfType<GameController>();
            if (controllers.Length > 1)
            {
                // Kill all duplicates except the first one.
                for (int i = 1; i < controllers.Length; i++)
                {
                    Destroy(controllers[i].gameObject);
                }
            }

            // If this was parented to something (like a Canvas), detach it so it survives scenes cleanly.
            if (transform.parent != null)
            {
                transform.SetParent(null, true);
            }

            // Persist this across scenes.
            DontDestroyOnLoad(gameObject);

            if (Instance != this)
            {
                // Edge case protection. Probably never hits.
            }
        }
        else if (Instance != this)
        {
            // If we somehow spawn another one, delete it immediately.
            Destroy(gameObject);
            return;
        }

        // Grab menu controller from same object
        menuController = GetComponent<MenuController>();

        // Default state
        state = GameState.FreeRoam;
    }


    // Failsafe singleton getter just in case
    public static GameController GetInstance()
    {
        if (Instance == null)
        {
            Instance = FindObjectOfType<GameController>();
            if (Instance != null)
            {
                DontDestroyOnLoad(Instance.gameObject);
            }
        }
        return Instance;
    }


    private void Start()
    {
        Debug.Log("GameController Start() called");

        // Hook into dialog system so states flip automatically
        DialogManager.Instance.OnShowDialog += () => state = GameState.Dialog;
        DialogManager.Instance.OnCloseDialog += () =>
        {
            if (state == GameState.Dialog)
                state = GameState.FreeRoam;
        };

        // Menu navigation hooks
        menuController.onBack += () => state = GameState.FreeRoam;
        menuController.onMenuSelected += OnMenuSelected;

        // Make sure correct camera is active on startup
        if (worldCamera != null) worldCamera.gameObject.SetActive(true);
        if (clueCamera != null) clueCamera.gameObject.SetActive(false);

        state = GameState.FreeRoam;
    }


    // Used when pausing the entire game (not often used now but good to have)
    public void PauseGame(bool pause)
    {
        if (pause)
        {
            stateBeforePause = state;
            state = GameState.Paused;
        }
        else
        {
            state = stateBeforePause;
        }
    }


    // Starts a clue interaction, switches cameras, sets up the UI.
    public void StartClue(
        string question = "It’s a blackout. Should Timmy open the fridge for the 90th time in a minute?",
        string[] answers = null,
        int correctIndex = 0,
        string clueID = "DefaultClue")
    {
        state = GameState.Clue;

        // Make sure clue UI exists
        clueSystem.gameObject.SetActive(true);

        // Disable overworld camera and turn on clue camera
        worldCamera.gameObject.SetActive(false);
        if (clueCamera != null) clueCamera.gameObject.SetActive(true);

        // If the clue was already solved once, don’t give points again.
        if (solvedClues.Contains(clueID))
        {
            clueSystem.ShowAlreadyFoundMessage();
        }
        else
        {
            // Normal clue flow
            clueSystem.ResetAndStartClueSystem(question, answers, correctIndex, clueID);
        }
    }


    // Ends a clue interaction and returns to free movement.
    public void EndClue()
    {
        state = GameState.FreeRoam;

        if (clueSystem != null) clueSystem.gameObject.SetActive(false);

        // Reactivate walking camera
        if (worldCamera != null)
        {
            worldCamera.gameObject.SetActive(true);
            worldCamera.enabled = true;
        }

        if (clueCamera != null)
            clueCamera.gameObject.SetActive(false);
    }


    private void Update()
    {
        // What player can do depends entirely on the current game state.
        if (state == GameState.FreeRoam)
        {
            playerController.HandleUpdate();
        }
        else if (state == GameState.Clue)
        {
            if (clueSystem != null)
            {
                clueSystem.HandleUpdate();
            }
        }
        else if (state == GameState.Dialog)
        {
            DialogManager.Instance.HandleUpdate();
        }
    }


    // Called whenever I load into a new scene so the player doesn’t spawn in a wall.
    public void SetCurrentScene(SceneDetails currScene)
    {
        PrevScene = CurrentScene;
        CurrentScene = currScene;

        if (currScene != null && playerController != null)
        {
            Vector3 newPos = playerController.transform.position;

            // For SpyAcademy, force the player to a walkable position if Unity spawned them badly.
            if (currScene.name == "SpyAcademy")
            {
                while (!playerController.Character.IsWalkable(newPos))
                {
                    newPos += new Vector3(0.1f, 0.1f, 0);
                }

                playerController.Character.SetPositionAndSnapToTile(newPos);
                Debug.Log("Player snapped to walkable position: " + newPos +
                    " in scene: " + currScene.name);
            }
        }
    }


    // Handles menu selections (Bag, Save, Load)
    void OnMenuSelected(int selectedItem)
    {
        if (selectedItem == 1)
        {
            state = GameState.Bag;
        }
        else if (selectedItem == 2)
        {
            SavingSystem.i.Save("saveSlot1");
            state = GameState.FreeRoam;
        }
        else if (selectedItem == 3)
        {
            SavingSystem.i.Load("saveSlot1");
            state = GameState.FreeRoam;
        }
    }
}
