using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Experimental.GraphView.GraphView;

public class PlayerMovement : MonoBehaviour
{

    private Rigidbody2D player;
    private BoxCollider2D boxCollider;

    private SpriteRenderer sprite;
    private Animator anim;




    //Bool
    private bool isWallDetected;
    private bool canWallSlide;
    private bool facingRight = true;
    private bool canWallJump = true;
    private int facingDirection = 1;
    private bool isAttacking;
    private bool canAttack = true;

    


    private float horizontalDirection = 0f;
    private enum MovementState { idle, run, jump, fall, wallJump, attack }

    [SerializeField] private Vector2 wallJumpDirection = new Vector2(5, 15);

    [Header("Layer")]
    [SerializeField] private LayerMask jumpableGround;

    [Header("Player settings")]
    [SerializeField] private float moveSpeed = 7f;
    [SerializeField] private float jumpForce = 14f;
    [SerializeField] private float attackCoolDown = 1.5f;

    [Header("Attack settings")]
    public Transform attackPoint;
    [SerializeField] public float attackRange = 0.5f;
    [SerializeField] public int attackDamage = 20;
    public LayerMask enemyLayers;

    [Header("Collisions Info")]
    [SerializeField] private Transform wallCheck;
    [SerializeField] private float wallCheckDistance;

    [SerializeField] private bool isWallSliding;

    AudioManager audioManager;


    private void Awake()
    {
        audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();
    }

    // Start is called before the first frame update
    private void Start()
    {
        player = GetComponent<Rigidbody2D>();
        boxCollider = GetComponent<BoxCollider2D>();
        sprite = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    private void Update()
    {
        horizontalDirection = Input.GetAxisRaw("Horizontal");
        player.velocity = new Vector2(horizontalDirection * moveSpeed, player.velocity.y);



        PlayerInputController();
        CollisionCheck();
        FlipController();
        UpdateAnimationState();
    }


    private void FixedUpdate()
    {
        if (isWallDetected && canWallSlide)
        {
            isWallSliding = true;
            player.velocity = new Vector2(player.velocity.x, player.velocity.y * 0.1f);

        }
        else
        {
            isWallSliding = false;
            player.velocity = new Vector2(horizontalDirection * moveSpeed, player.velocity.y);
        }
    }

    private void UpdateAnimationState()
    {
        MovementState state;


        if (horizontalDirection > 0f) //if right
        {
            state = MovementState.run;
            //sprite.flipX = false;
        }
        else if (horizontalDirection < 0f) //if left
        {
            state = MovementState.run;
            //sprite.flipX = true;
        }
        else
        {
            state = MovementState.idle;
        }

        if (player.velocity.y > .1f) //if jump
        {
            state = MovementState.jump;
        }
        else if (player.velocity.y < -.1f) //if fall
        {
            state = MovementState.fall;
        }

        if (isWallSliding)
        {
            state = MovementState.wallJump;
        }
        else if (isAttacking)
        {
            state = MovementState.attack;
        }

        anim.SetInteger("state", (int)state);
    }

    private bool IsGrounded()
    {
        //create a second box below the player down by .1f then check if he collide the ground
        return Physics2D.BoxCast(boxCollider.bounds.center, boxCollider.bounds.size, 0f, Vector2.down, .1f, jumpableGround);
    }

    /*private bool IsWalled()
    {
        return Physics2D.Raycast(wallCheck.position, Vector2.right, wallCheckDistance, jumpableGround);
    }*/

    private void CollisionCheck()
    {
        isWallDetected = Physics2D.Raycast(wallCheck.position, Vector2.right, wallCheckDistance, jumpableGround) || Physics2D.Raycast(wallCheck.position, Vector2.left, wallCheckDistance, jumpableGround);

        if (!IsGrounded() && player.velocity.y < 0)
        {
            canWallSlide = true;
        }
    }

    private void Flip()
    {
        facingDirection = facingDirection * -1; //every time player flip set to 1 or -1 ton change the wall jump direction
        facingRight = !facingRight; //switch
        transform.Rotate(0, 180, 0);
    }

    private void FlipController()
    {
        if (IsGrounded() && isWallDetected)
        {
            if (facingRight && horizontalDirection < 0)
            {
                Flip();
            }
            else if (!facingRight && horizontalDirection > 0)
                Flip();
        }

        if (player.velocity.x < 0 && facingRight)
        {
            Flip();
        }
        else if (player.velocity.x > 0 && !facingRight)
        {
            Flip();
        }
    }

    private void PlayerInputController()
    {
        isAttacking = false;
        if (Input.GetButtonDown("Jump"))
        {
            if (isWallSliding && canWallJump)
            {
                WallJump();
                Debug.Log("Wall Jump");
            }
            else if (IsGrounded()) //Edit=>ProjectSettings=>InputManagers setup bindings from unity
            {
                Jump();
                Debug.Log("Jump");
            }
        }
        else if (Input.GetKeyDown(KeyCode.E) && canAttack)
        {
            Debug.Log("Attack");
            Attack();
        }
    }

    private void Attack()
    {
        isAttacking = true;
        canAttack = false;
        StartCoroutine(AttackCooldown());

        //Detect enemies in attack range
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, enemyLayers);

        foreach (Collider2D enemy in hitEnemies)
        {
            enemy.GetComponent<Enemy>().TakeDamage(attackDamage);
        }
    }

    private IEnumerator AttackCooldown()
    {
        yield return new WaitForSecondsRealtime(attackCoolDown);
        canAttack = true;
    }

    private void Jump()
    {
        audioManager.PlaySFX(audioManager.jump);
        player.velocity = new Vector2(player.velocity.x, jumpForce); //Vector2 2D Game
    }

    private void WallJump()
    {

        //TODO change code its so bad
        audioManager.PlaySFX(audioManager.jump);

        // Flip the player's direction
        //Flip();

        // Calculate the wall jump direction
        Vector2 jumpDirection = new Vector2(wallJumpDirection.x * -facingDirection, wallJumpDirection.y);

        // Apply the jump force
        //player.velocity = new Vector2(0f, 0f); // Set current velocity to zero before applying the force
        player.AddForce(jumpDirection, ForceMode2D.Impulse);

    }



    //Use to draw the wall detector hitbox for debug
    private void OnDrawGizmos()
    {
        Gizmos.DrawLine(wallCheck.position, new Vector3(wallCheck.position.x + wallCheckDistance, wallCheck.position.y, wallCheck.position.z));
        if (attackPoint == null)
        {
            return;
        }
        else
        {
            Gizmos.DrawWireSphere(attackPoint.position, attackRange);
        }
    }
}
