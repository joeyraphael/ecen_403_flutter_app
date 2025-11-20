using System; 
using System.Collections; 
using System.Collections.Generic; 
using System.Linq; 
using UnityEngine; 


// this script controls the player moving, touching, interacting, saving
public class PlayerController : MonoBehaviour, ISavable
{
    public Joystick joystick;   // joystick for phone
    [SerializeField] string name; 
    [SerializeField] Sprite sprite; 

    private Vector2 input; 
    private Character character; 
    private Vector2 touchStartPos; 
    private bool isTouching; 
    private float touchInteractThreshold = 0.1f; 

    private void Awake()
    {
        character = GetComponent<Character>(); // grab char script on start
    }

    public void HandleUpdate()
    {
        if (Camera.main == null) 
            Debug.LogError("no camera??");

        // if im not already mid-move
        if (!character.IsMoving)
        {
            // if joystick exists AND actually being touched
            if (joystick != null && (Mathf.Abs(joystick.Horizontal) > .1f || Mathf.Abs(joystick.Vertical) > .1f))
                input = new Vector2(joystick.Horizontal, joystick.Vertical); // joystick movement
            else
            {
                input.x = Input.GetAxisRaw("Horizontal"); // keyboard left/right
                input.y = Input.GetAxisRaw("Vertical");   // keyboard up/down
            }

            // no diagonals allowed (tile game)
            if (input.x != 0)
                input.y = 0;

            bool touchActive = false; // used to tell if user is dragging/tapping
            Vector2 currentPos = Vector2.zero; // track where finger is

            // PHONE TOUCH LOGIC
            if (Input.touchCount > 0)
            {
                Touch t = Input.GetTouch(0); // only care about first finger
                currentPos = Camera.main.ScreenToWorldPoint(t.position); // finger pos but world coords
                touchActive = true;

                if (t.phase == TouchPhase.Began) // finger just started touching
                {
                    touchStartPos = currentPos; // mark where drag started
                    isTouching = true; // finger is down
                }
                else if (t.phase == TouchPhase.Moved && isTouching) // dragging
                    HandleDrag(currentPos); // figure swipe direction
                else if (t.phase == TouchPhase.Ended && isTouching) // finger lifted
                    HandleRelease(currentPos); // see if tap or swipe
            }
            // MOUSE INPUT (web build / editor)
            else if (Input.GetMouseButtonDown(0) || Input.GetMouseButton(0) || Input.GetMouseButtonUp(0))
            {
                currentPos = Camera.main.ScreenToWorldPoint(Input.mousePosition); // where mouse is
                touchActive = true;

                if (Input.GetMouseButtonDown(0)) // mouse clicked
                {
                    touchStartPos = currentPos; // mark start
                    isTouching = true;
                }
                else if (Input.GetMouseButton(0) && isTouching) // holding + dragging
                    HandleDrag(currentPos);
                else if (Input.GetMouseButtonUp(0) && isTouching) // click released
                    HandleRelease(currentPos);
            }

            // movement logic (only move if input is not zero)
            if ((touchActive || input != Vector2.zero) && input != Vector2.zero)
            {
                if (!character.IsMoving) // double check not already walking
                    StartCoroutine(character.Move(input, OnMoveOver)); // move coroutine
            }
        }

        character.HandleUpdate(); // update animations etc

        if (Input.GetKeyDown(KeyCode.Z)) // keyboard interact
            Interact();

        if (!character.IsMoving) // after move ends, clear input
            input = Vector2.zero;
    }

    private void HandleDrag(Vector2 pos)
    {
        Vector2 d = pos - touchStartPos; // get swipe distance

        // figure out which way swipe is more in
        if (Mathf.Abs(d.x) > Mathf.Abs(d.y)) // left/right swipe
            input = new Vector2(Mathf.Sign(d.x), 0);
        else // up/down swipe
            input = new Vector2(0, Mathf.Sign(d.y));
    }

    private void HandleRelease(Vector2 pos)
    {
        isTouching = false; // no longer touching
        float dist = (pos - touchStartPos).magnitude; // swipe length

        if (dist < touchInteractThreshold) 
            Interact(); // treat tap as interact

        input = Vector2.zero; // reset input
    }

    private void OnMoveOver()
    {
        // after moving, check if stepped on trigger area
        var hits = Physics2D.OverlapCircleAll(
            transform.position - new Vector3(0, character.OffsetY), // correct hitbox pos
            0.2f, 
            GameLayers.i.TriggerableLayers); 

        // check all hits
        foreach (var h in hits)
        {
            var trig = h.GetComponent<IPlayerTriggerable>(); // things player can trigger
            if (trig != null)
            {
                trig.OnPlayerTriggered(this); 
                break; // only do one
            }
        }
    }

    void Interact()
    {
        var dir = new Vector3(character.Animator.MoveX, character.Animator.MoveY); // direction player is facing
        var p = transform.position + dir; // tile in front

        var hit = Physics2D.OverlapCircle(p, 0.3f, GameLayers.i.InteractableLayer); // check if smth there
        if (hit != null)
            hit.GetComponent<Interactable>()?.Interact(transform); 
    }

    public object CaptureState()
    {
        // save player pos for loading later
        return new PlayerSaveData()
        {
            position = new float[] { transform.position.x, transform.position.y }
        };
    }

    public void RestoreState(object state)
    {
        var save = (PlayerSaveData)state; // load save data
        var pos = save.position; 
        transform.position = new Vector3(pos[0], pos[1]); // set pos back
    }

    public string Name => name; 
    public Sprite Sprite => sprite; 
    public Character Character => character; // return char
}

[Serializable]
public class PlayerSaveData
{
    public float[] position; // just store x,y
}
