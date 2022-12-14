using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 0.9f;
    public float collisionOffset = 0.02f;
    public ContactFilter2D movementFilter;

    Vector2 movementInput;

    SpriteRenderer spriteRenderer;

    Rigidbody2D rb;

    Animator animator;

    List<RaycastHit2D> castCollisions = new List<RaycastHit2D>();

    bool canMove = true;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void FixedUpdate(){
        if(canMove){
            if(movementInput != Vector2.zero){
                bool success = TryMove(movementInput);
                if(!success){
                    success = TryMove(new Vector2(movementInput.x,0));
                }
                if(!success){
                    success = TryMove(new Vector2(0,movementInput.y));
                }
                animator.SetBool("isMoving",success);
            }else{
                animator.SetBool("isMoving",false);
            }
            if(movementInput.x > 0){
                spriteRenderer.flipX = false;
            }else if(movementInput.x < 0){
                spriteRenderer.flipX = true;
            }
        }
    }

    private bool TryMove(Vector2 direction){
        if(direction != Vector2.zero) {
            int count = rb.Cast(
                direction,
                movementFilter,
                castCollisions,
                moveSpeed * Time.fixedDeltaTime + collisionOffset
            );
            if(count == 0){
                rb.MovePosition(rb.position + direction * moveSpeed * Time.fixedDeltaTime);
                return true;
            }
            return false;
        }else{
            return false;
        }
    }

    void OnMove(InputValue movementValue){
        movementInput = movementValue.Get<Vector2>();
    }

    void OnFire() {
        animator.SetTrigger("swordAttack");
    }

    public void LockMovement(){
        canMove = false;
    }

        public void UnlockMovement(){
        canMove = true;
    }
}
