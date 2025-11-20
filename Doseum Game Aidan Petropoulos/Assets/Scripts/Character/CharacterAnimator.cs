using System.Collections; 
using System.Collections.Generic; 
using UnityEngine; 

// this basically handles switching sprites when the player/NPC walks
public class CharacterAnimator : MonoBehaviour
{
    [SerializeField] List<Sprite> walkDownSprites;  // frames for walking down
    [SerializeField] List<Sprite> walkUpSprites;    
    [SerializeField] List<Sprite> walkRightSprites; 
    [SerializeField] List<Sprite> walkLeftSprites;  
    [SerializeField] FacingDirection defaultDirection = FacingDirection.Down; // idle dir

    // movement values the controller sets
    public float MoveX { get; set; } 
    public float MoveY { get; set; } 
    public bool IsMoving { get; set; } 

    // the animators that flip thru sprite frames
    SpriteAnimator walkDownAnim;
    SpriteAnimator walkUpAnim;
    SpriteAnimator walkRightAnim;
    SpriteAnimator walkLeftAnim;

    SpriteAnimator currentAnim; // whichever animation we're using rn
    bool wasPreviouslyMoving; 

    // reference to spriteRenderer in scene
    SpriteRenderer spriteRenderer;

    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>(); // grab renderer on start

        // create a SpriteAnimator for each direction
        walkDownAnim = new SpriteAnimator(walkDownSprites, spriteRenderer);
        walkUpAnim = new SpriteAnimator(walkUpSprites, spriteRenderer);
        walkRightAnim = new SpriteAnimator(walkRightSprites, spriteRenderer);
        walkLeftAnim = new SpriteAnimator(walkLeftSprites, spriteRenderer);

        currentAnim = walkDownAnim; // start facing down by default
    }

    private void Update()
    {
        var prevAnim = currentAnim; // store old anim so we can check if changed

        // pick animation based on movement direction
        if (MoveX == 1)
            currentAnim = walkRightAnim;
        else if (MoveX == -1)
            currentAnim = walkLeftAnim;
        else if (MoveY == 1)
            currentAnim = walkUpAnim;
        else if (MoveY == -1)
            currentAnim = walkDownAnim;

        // if the animation changed OR if we stopped/started moving, restart animation
        if (currentAnim != prevAnim || IsMoving != wasPreviouslyMoving)
            currentAnim.Start();

        // if moving, update animation frames
        if (IsMoving)
            currentAnim.HandleUpdate();
        else
            spriteRenderer.sprite = currentAnim.Frames[0]; // show first frame when idle

        wasPreviouslyMoving = IsMoving; // store movement state for next frame
    }

    public void SetFacingDirection(FacingDirection dir)
    {
        // this makes the idle sprite face the correct direction
        if (dir == FacingDirection.Right)
            MoveX = 1;
        else if (dir == FacingDirection.Left)
            MoveX = -1;
        else if (dir == FacingDirection.Down)
            MoveY = -1;
        else if (dir == FacingDirection.Up)
            MoveY = 1;
    }

    // getter for default direction
    public FacingDirection DefaultDirection
    {
        get => defaultDirection;
    }
}

// possible directions char can face
public enum FacingDirection { Up, Down, Left, Right }
