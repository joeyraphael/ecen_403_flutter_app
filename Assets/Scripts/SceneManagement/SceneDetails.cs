using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneDetails : MonoBehaviour
{
    [SerializeField] List<SceneDetails> connectedScenes;

    public bool IsLoaded { get; private set; }

    List<SavableEntity> savableEntities;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Debug.Log($"Entered {gameObject.name}");
            try
            {
                //  Load this scene and all recursively connected scenes
                HashSet<SceneDetails> visited = new HashSet<SceneDetails>();
                LoadConnectedScenesRecursively(visited);

                var gc = GameController.GetInstance();
                if (gc != null)
                {
                    gc.SetCurrentScene(this);

                    // Adjust player position if spawning in SpyAcademy
                    if (gameObject.name == "SpyAcademy") // Adjust to your scene name
                    {
                        var player = collision.GetComponent<PlayerController>();
                        var character = player.Character;
                        Vector3 spawnPos = collision.transform.position;
                        int attempts = 0;
                        while (!character.IsWalkable(spawnPos) && attempts < 10)
                        {
                            spawnPos += new Vector3(0.1f, 0.1f, 0);
                            attempts++;
                        }
                        character.SetPositionAndSnapToTile(spawnPos);
                        Debug.Log($"Player spawned at: {spawnPos} in {gameObject.name}");
                    }
                }
                else
                {
                    Debug.LogError("GameController.Instance is null in SceneDetails.OnTriggerEnter2D!");
                }

                //  Unload any scenes not part of the connected cluster
                var allConnected = visited;
                var loadedScenes = FindObjectsOfType<SceneDetails>().Where(s => s.IsLoaded).ToList();

                foreach (var scene in loadedScenes)
                {
                    if (!allConnected.Contains(scene))
                    {
                        Debug.Log($"Unloading unconnected scene: {scene.name}");
                        scene.UnloadScene();
                    }
                }
            }
            catch (System.Exception e)
            {
                Debug.LogError($"Exception in SceneDetails.OnTriggerEnter2D: {e.Message}. Continuing game...");
            }
        }
    }

    // Recursive loading function
    private void LoadConnectedScenesRecursively(HashSet<SceneDetails> visited)
    {
        if (visited.Contains(this))
            return;

        visited.Add(this);
        LoadScene();

        foreach (var connected in connectedScenes)
        {
            if (connected != null)
                connected.LoadConnectedScenesRecursively(visited);
        }
    }

    public void LoadScene()
    {
        if (!IsLoaded)
        {
            Debug.Log($"Loading scene: {gameObject.name}");
            var operation = SceneManager.LoadSceneAsync(gameObject.name, LoadSceneMode.Additive);
            IsLoaded = true;

            operation.completed += (AsyncOperation op) =>
            {
                UnpauseGame();
                savableEntities = GetSavableEntitiesInScene();

                if (SavingSystem.i != null)
                {
                    SavingSystem.i.RestoreEntityStates(savableEntities);
                }
                else
                {
                    Debug.LogWarning("SavingSystem.i was null when trying to restore entity states.");
                }
            };
        }
    }

    void UnpauseGame()
    {
        Time.timeScale = 1f; // Unpause the game when transitioning to a new scene.
    }

    public void UnloadScene()
    {
        if (IsLoaded)
        {
            Debug.Log($"Unloading scene: {gameObject.name}");
            if (SavingSystem.i != null)
            {
                SavingSystem.i.CaptureEntityStates(savableEntities);
            }
            else
            {
                Debug.LogWarning("SavingSystem.i is null in UnloadScene, skipping entity state capture.");
            }

            SceneManager.UnloadSceneAsync(gameObject.name);
            Debug.Log($"Scene {gameObject.name} successfully unloaded");
            IsLoaded = false;
        }
    }

    List<SavableEntity> GetSavableEntitiesInScene()
    {
        var currScene = SceneManager.GetSceneByName(gameObject.name);
        var savableEntities = FindObjectsOfType<SavableEntity>().Where(x => x.gameObject.scene == currScene).ToList();
        return savableEntities;
    }
}
