using UnityEngine; // unity stuff
using UnityEngine.SceneManagement; // for loading scenes
using System.Collections; // for IEnumerator

// just an ID so I know which portal links to which
public enum DestinationIdentifier { A, B, C, D }

public class Portal : MonoBehaviour, IPlayerTriggerable
{
    [SerializeField] int sceneToLoad = -1; // which scene this portal brings me to
    [SerializeField] Transform spawnPoint; // where i should spawn in new scene
    [SerializeField] DestinationIdentifier destinationPortal; // this portal’s ID

    PlayerController player; // ref to player that walked into me

    public void OnPlayerTriggered(PlayerController player)
    {
        this.player = player; // store who triggered me
        StartCoroutine(SwitchScene()); // do portal stuff
    }

    IEnumerator SwitchScene()
    {
        DontDestroyOnLoad(gameObject); // keep this portal alive while swapping scenes
        Debug.Log("Portal: Pausing game."); // just for debug

        // load new scene additively so i can move player then unload old scene cleanly
        yield return SceneManager.LoadSceneAsync(sceneToLoad, LoadSceneMode.Additive);

        Portal destPortal = GetDestinationPortal(); // find matching portal on new scene
        UpdatePlayer(destPortal); // move player to that portal’s spawn point

        // remove old scene
        SceneManager.UnloadSceneAsync(gameObject.scene);

        Debug.Log("Portal: Unpausing game."); // another debug line

        Destroy(gameObject); // portal no longer needed
    }

    private Portal GetDestinationPortal()
    {
        // find the portal in the NEW scene with the same destination ID
        foreach (Portal portal in FindObjectsOfType<Portal>())
        {
            if (portal == this) continue; // skip myself
            if (portal.destinationPortal == this.destinationPortal) // match ID
                return portal; // found it
        }
        return null; // shouldnt really happen unless setup wrong
    }

    private void UpdatePlayer(Portal destPortal)
    {
        // move player to exact tile of new portal
        player.Character.SetPositionAndSnapToTile(destPortal.spawnPoint.position);
    }
}
