using System.Collections; // for IEnumerator
using System.Collections.Generic;
using UnityEngine;
using System.Linq; // i use First() here

// this is basically a simple portal that only teleports inside the SAME scene
public class LocationPortal : MonoBehaviour, IPlayerTriggerable
{
    [SerializeField] DestinationIdentifier destinationPortal; // which portal ID i link to
    [SerializeField] Transform spawnPoint; // where i spawn the player

    PlayerController player; // store whos teleporting

    public void OnPlayerTriggered(PlayerController player)
    {
        player.Character.Animator.IsMoving = false; // stop them from walking mid-teleport
        this.player = player; // save reference
        StartCoroutine(Teleport()); // start teleport process
    }

    Fader fader; // screen fade script
    private void Start()
    {
        fader = FindObjectOfType<Fader>(); // grab the fader in scene
    }

    IEnumerator Teleport()
    {
        // fade to black
        yield return fader.FadeIn(0.5f);

        // find the matching portal (same destination ID, but not this one)
        var destPortal = FindObjectsOfType<LocationPortal>()
            .First(x => x != this && x.destinationPortal == this.destinationPortal);

        // move player to the destination portal’s spawn point
        player.Character.SetPositionAndSnapToTile(destPortal.SpawnPoint.position);

        // fade back in
        yield return fader.FadeOut(0.5f);
    }

    // quick getter for spawn point
    public Transform SpawnPoint => spawnPoint;
}
