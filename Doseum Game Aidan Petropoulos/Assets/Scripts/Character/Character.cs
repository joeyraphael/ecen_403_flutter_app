using System; 
using System.Collections; 
using System.Collections.Generic;
using UnityEngine; 

// this script handles actual moving on grid, blocking checks, animation directions
public class Character : MonoBehaviour
{
    public float moveSpeed; // how fast character walks
    public bool IsMoving { get; private set; } 
    public float OffsetY { get; private set; } = .1f; // little offset so sprite sits nice on tile

    CharacterAnimator animator; // ref to animator
    [SerializeField] private LayerMask solidObjectsLayer; 
    [SerializeField] private LayerMask interactableLayer; // stuff that also blocks (npcs etc)

    private void Awake()
    {
        animator = GetComponent<CharacterAnimator>(); 
        SetPositionAndSnapToTile(transform.position); // snap starting pos to tile grid
    }

    public void SetPositionAndSnapToTile(Vector2 pos)
    {
        // snap X and Y to nearest tile center
        pos.x = Mathf.Floor(pos.x) + 0.5f;
        pos.y = Mathf.Floor(pos.y) + 0.5f + OffsetY;
        transform.position = pos; // set pos
    }

    public IEnumerator Move(Vector2 moveVec, Action OnMoveOver = null)
    {
        // if somehow movement gets spam called, skip it
        if (IsMoving)
        {
            Debug.Log("Move called but already moving");
            yield break;
        }

        // set facing direction for animator
        animator.MoveX = Mathf.Clamp(moveVec.x, -1f, 1f);
        animator.MoveY = Mathf.Clamp(moveVec.y, -1f, 1f);

        // snap where we're trying to go to next tile
        var targetPos = new Vector3(
            Mathf.Floor(transform.position.x + moveVec.x) + 0.5f,
            Mathf.Floor(transform.position.y + moveVec.y) + 0.5f + OffsetY,
            transform.position.z
        );

        // check if wall or something in the way
        if (!IsPathClear(targetPos))
        {
            Debug.Log("Path blocked"); // wall detected
            yield break;
        }

        IsMoving = true; // mark moving

        float tolerance = 0.01f; 
        float timeout = 1.0f; // safety timeout so character doesnt freeze
        float elapsed = 0f; 

        // move until I reach tile
        while ((targetPos - transform.position).sqrMagnitude > tolerance * tolerance)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetPos, moveSpeed * Time.deltaTime);
            elapsed += Time.deltaTime;

            if (elapsed > timeout) break; 

            yield return null; 
        }

        transform.position = targetPos; // make sure exactly on tile

        IsMoving = false; 
        animator.IsMoving = false; // stop anim

        OnMoveOver?.Invoke(); 
    }

    public void HandleUpdate()
    {
        // just feed animator current movement state
        animator.IsMoving = IsMoving;
    }

    // checks if next tile has wall/npc/etc
    private bool IsPathClear(Vector3 targetPos)
    {
        var diff = targetPos - transform.position; // dist to check
        var dir = diff.normalized; 

        // raycast from current tile to next tile
        if (Physics2D.Raycast(transform.position, dir, diff.magnitude, solidObjectsLayer | interactableLayer))
            return false; 

        return true; 
    }

    public bool IsWalkable(Vector3 targetPos)
    {
        // rn everything is walkable bc I didnt need this yet 
        return true;
    }

    public void LookTowards(Vector3 targetPos)
    {
        // figure out direction we should face
        var xdiff = Mathf.Floor(targetPos.x) - Mathf.Floor(transform.position.x);
        var ydiff = Mathf.Floor(targetPos.y) - Mathf.Floor(transform.position.y);

        // must be straight (no diagonal)
        if (xdiff == 0 || ydiff == 0)
        {
            animator.MoveX = Mathf.Clamp(xdiff, -1f, 1f);
            animator.MoveY = Mathf.Clamp(ydiff, -1f, 1f);
        }
        else
        {
            Debug.LogError("Error in LookTowards: cant look diagonally man");
        }
    }

    public CharacterAnimator Animator => animator; 
}
