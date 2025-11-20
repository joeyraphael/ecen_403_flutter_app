using System.Collections; 
using System.Collections.Generic; 
using UnityEngine; 

// this handles NPCs (walking around + talking)
public class NPCController : MonoBehaviour, Interactable
{
    [SerializeField] Dialog dialog; // what dialog this npc says
    [SerializeField] List<Vector2> movementPattern; // little walk path
    [SerializeField] float timeBetweenPattern; // how long npc sits before next step

    NPCState state; 
    float idleTimer = 0f; // counts how long npc been not moving
    int currentPattern = 0; 

    Character character; // npc's character script

    private void Awake()
    {
        character = GetComponent<Character>(); // grab char script
    }

    public void Interact(Transform initiator)
    {
        // only talk if npc not already busy doing something
        if (state == NPCState.Idle)
        {
            state = NPCState.Dialog; 
            character.LookTowards(initiator.position); 

            // show dialog
            StartCoroutine(DialogManager.Instance.ShowDialog(dialog, () => {
                // callback when dialog ends
                idleTimer = 0f; 
                state = NPCState.Idle;
            }));
        }
        else
        {
            
        }
    }

    private void Update()
    {
        // only move if npc not talking or something
        if (state == NPCState.Idle)
        {
            idleTimer += Time.deltaTime; 

            if (idleTimer > timeBetweenPattern) 
            {
                idleTimer = 0f;
                if (movementPattern.Count > 0) // only walk if pattern exists
                    StartCoroutine(Walk()); 
            }
        }

        character.HandleUpdate(); // update npc animations
    }

    IEnumerator Walk()
    {
        state = NPCState.Walking; // mark npc is walking

        var oldPos = transform.position; 

        yield return character.Move(movementPattern[currentPattern]); 

        // if npc moved, go to next step in pattern
        if (transform.position != oldPos)
            currentPattern = (currentPattern + 1) % movementPattern.Count;

        state = NPCState.Idle; // done walking
    }
}

// what state npc is in
public enum NPCState { Idle, Walking, Dialog }
