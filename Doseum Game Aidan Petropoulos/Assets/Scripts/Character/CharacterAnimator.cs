using System.Collections; // unity stuff
using System.Collections.Generic; // lists
using UnityEngine; // engine stuff

// this basically handles switching sprites when the player/NPC walks
public class CharacterAnimator : MonoBehaviour
{
    [SerializeField] List<Sprite> walkDownSprites;  // frames for walking down
    [SerializeField] List<Sprite> walkUpSprites;    // frames for walking up
    [SerializeField] List<Sprite> walkRightSprites; // frames for walking right
    [SerializeField] List<Sprite> walkLeftSprites;  // frames for walking left
    [SerializeField] FacingDirection defaultDirection = FacingDirection.Down; // idle dir

    // movement values the controller sets
    public float MoveX { get; set; } // horizontal (-1 left, 1 right)
    public float MoveY { get; set; } // vertical
    public bool IsMoving { get; set; } // if char is walking rn

    // the animators that flip thru sprite frames
    SpriteAnimator walkDownAnim;
    SpriteAnimator walkUpAnim;
    SpriteAnimator walkRightAnim;
    SpriteAnimator walkLeftAnim;

    SpriteAnimator currentAnim; // whichever animation we're using rn
    bool wasPreviouslyMoving; // so we know when to restart animation

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
