using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using DG.Tweening;

public class PlayerMovement1 : MonoBehaviour
{
    private Collision coll;
    public Rigidbody2D rb;
    public GameObject LevelManager;
    private AnimationScript anim;

    public float speed = 7;
    public float jumpForce = 15;
    public float slideSpeed = 5;
    public float dashSpeed = 20;
    public float wallJumpLerp = 10;

    public bool canMove;
    public bool wallGrab;
    public bool wallSlide;
    public bool wallJumped;
    public bool isDashing;
    [SerializeField]public bool isDead;
    [SerializeField]public bool isAlive;

    private bool groundTouch;
    private bool hasDashed;

    public int side = 1;

    void Start()
    {
       coll = GetComponent<Collision>();
       rb = GetComponent<Rigidbody2D>(); 
       anim = GetComponentInChildren<AnimationScript>();
       isAlive = true;
    }

    // Update is called once per frame
    void Update()
    {
        float x = Input.GetAxis("Horizontal");
        float y = Input.GetAxis("Vertical");
        float xRaw = Input.GetAxisRaw("Horizontal");
        float yRaw = Input.GetAxisRaw("Vertical");
        Vector2 dir = new Vector2(x, y);

        Walk(dir);
        anim.SetHorizontalMovement(x, y, rb.velocity.y);
        //wallGrab = coll.onWall && Input.GetKey(KeyCode.LeftShift);

        if(coll.onWall && Input.GetButton("Fire3") && canMove)
        {
            if(side != coll.wallSide)
            {
                anim.Flip(side*-1);
            }
            wallGrab =true;
            wallSlide = false;
            //if on wall and press leftshift, you grab the wall
        }

        if(Input.GetButtonUp("Fire3") || !coll.onWall || !canMove) 
        {
            wallGrab = false;
            wallSlide = false;
            //if press lshift only or not on wall or can't move, wallmechanics are off
        }

        if(coll.onGround && !isDashing && !isDead)
        {
            wallJumped = false;
            GetComponent<BetterJump>().enabled = true;
            //if on ground and not dashing, activate the better jump mechanics script
        }

        if(wallGrab && !isDashing && !isDead)
        {
            //if wall grab and not dashing
            //cant wall grab while dashing?
            rb.gravityScale = 0; //so player does not fall
            if(x > .2f || x < -.2f)
            {
                rb.velocity = new Vector2(rb.velocity.x,0);
            }

            float speedModifier = y > 0 ? .5f : 1;

            rb.velocity = new Vector2(rb.velocity.x, y*(speed * speedModifier));
        }

        else
        {
            rb.gravityScale = 3;
        }

        if(coll.onWall && !coll.onGround)
        {
            //if onWall and not onGround, execute code
            if(x!=0 && !wallGrab)
            {
                //if x is moving(left or right) and not using wallGrab mechanic
                wallSlide = true;
                WallSlide();
            }
        }

        if(!coll.onWall || coll.onGround)
        {
            //if not onWall or is onGround, turn off wallSlide
            wallSlide = false;
        }

        if(Input.GetButtonDown("Jump") && !isDead)
        {
            anim.SetTrigger("jump");
            if(coll.onGround)
            {
                Jump(Vector2.up, false);//up and not on wall
                SoundManager.PlaySound("playerJump");
            }
            if(coll.onWall && !coll.onGround)
            {
                WallJump(); //if press jump onwall but not onground, wallJump
                SoundManager.PlaySound("playerJump");
            }
        }

        if(Input.GetButtonDown("Fire1") && !hasDashed)
        {
            //if pressing 1 and dash not on cd
            //to avoid multiple dashes
            if(xRaw != 0 || yRaw != 0) //cant dash in place or while idle
            {
                Dash(xRaw,yRaw);
            }
        }

        if(coll.onGround && !groundTouch)
        {
            GroundTouch();
            groundTouch = true;
        }

        if(!coll.onGround && groundTouch)
        {
            groundTouch = false;
        }

        if(wallGrab || wallSlide || !canMove)
        {
            return;
        }
        if(x>0)
        {
            side = 1; //if moving right, face right
            anim.Flip(side);
        }
        if(x<0)
        {
            side = -1;//if moving left, face left
            anim.Flip(side);
        }
        if(isDead == true)
        {
            rb.bodyType = RigidbodyType2D.Static;
        }
    }

    void GroundTouch()
    {
        hasDashed = false;
        isDashing = false;
    }

    void Jump(Vector2 dir, bool wall)
    {
        rb.velocity = new Vector2(rb.velocity.x, 0);
        rb.velocity += dir*jumpForce;
    }

    IEnumerator DisableMovement(float time)
    {
        canMove = false;
        yield return new WaitForSeconds(time);
        canMove = true;
    }

    void Walk(Vector2 dir)
    {
        if(!canMove)
        {
            return;
        }
        if(wallGrab)
        {
            return;
        }
        if(!wallJumped)
        {
            rb.velocity = new Vector2(dir.x* speed, rb.velocity.y);
        }
        else
        {
            rb.velocity = Vector2.Lerp(rb.velocity, (new Vector2(dir.x * speed, rb.velocity.y)), wallJumpLerp * Time.deltaTime);
        } 
    }

    void WallSlide()
    {
        if(coll.wallSide != side)
        {
            anim.Flip(side*-1);
        }
        if(!canMove)
        {
            return;
        }

        bool pushingWall = false;
        if((rb.velocity.x > 0 && coll.onRightWall) || (rb.velocity.x < 0 && coll.onLeftWall))
        {
            pushingWall = true;
        }

        float push = pushingWall ? 0 : rb.velocity.x;
        rb.velocity = new Vector2(rb.velocity.x, -slideSpeed);
    }

    void WallJump()
    {
        if ((side == 1 && coll.onRightWall) || side == -1 && !coll.onRightWall)
        {
            side *= -1;
            anim.Flip(side);
        }

        StopCoroutine(DisableMovement(0));
        StartCoroutine(DisableMovement(.1f));

        Vector2 wallDir = coll.onRightWall ? Vector2.left : Vector2.right;

        Jump((Vector2.up / 1.5f + wallDir / 1.5f), true);

        wallJumped = true;
        
    }

    private void Dash(float x, float y)
    {
        FindObjectOfType<RippleEffect>().Emit(Camera.main.WorldToViewportPoint(transform.position));
        
        hasDashed = true;

        anim.SetTrigger("dash");

        rb.velocity = Vector2.zero;
        Vector2 dir = new Vector2(x,y);

        rb.velocity += dir.normalized * dashSpeed;
        StartCoroutine(DashWait());
    }

    IEnumerator DashWait()
    {
        //cd after using dash
        StartCoroutine(GroundDash());
        DOVirtual.Float(14, 0, .8f, RigidbodyDrag);//from 14 to 0 in .8f, callback?
        //disable movement 
        rb.gravityScale = 0;
        GetComponent<BetterJump>().enabled = false;
        wallJumped = true;
        isDashing = true;

        yield return new WaitForSeconds(.3f);
        //return to normal
        rb.gravityScale = 3;
        GetComponent<BetterJump>().enabled = true;
        wallJumped = false;
        isDashing = false;
    }

    IEnumerator GroundDash()
    {
        yield return new WaitForSeconds(.15f);
        if(coll.onGround)
        {
            hasDashed = false; //if landed on ground after using dash, can dash again
        }
    }

    void RigidbodyDrag(float x)
    {
        rb.drag = x; //used to slow down object, higher drag = slower
    }

    public void Dead()
    {
        SoundManager.PlaySound("playerDeath");
        anim.SetTrigger("death");
        rb.bodyType = RigidbodyType2D.Static;
        canMove = false;
        GetComponent<BetterJump>().enabled = false;
        isDead = true;
        isAlive = false;
    }

    public void Spawn()
    {
        isAlive = true;
        isDead = false;
        canMove = true;
        rb.bodyType = RigidbodyType2D.Dynamic;
        GetComponent<BetterJump>().enabled = true;
    }
}
