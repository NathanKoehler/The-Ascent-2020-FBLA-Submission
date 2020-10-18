using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_S : MonoBehaviour
{
    public MasterController_S masterController_S;

    public Rigidbody2D rigid;
    public Cinemachine.CinemachineVirtualCamera cinemachine;
    public float jumpHeight;
    public float jumpingGravity;
    public float normalGravity;
    public float locationInherit;
    public float locationPrevious;
    public float wallStunTime;
    public float stunTime;
    public bool isCrouch;
    public Vector3 playerStartPoint;
    public GameObject projectile;


    [HideInInspector]
    public bool stunned;
    [HideInInspector]
    public bool facingDir = true;

    
    private IEnumerator Jumping;
    private Vector3 shootingOffset = new Vector3 (1.2f, 0, 0);
    private ArrayList collider2Ds;
    private Collider2D colliderObj;
    private Vector2 rawInputs;
    private Vector2 playerCheckpoint;
    private Animator anim;
    private Transform parentObj;
    private Transform childObj;
    private GameObject player;
    private SpriteRenderer rend;
    private Color playerColor;
    private bool disableSlow;
    
    private bool slowDown;
    private bool cancelX;
    private bool lockInputs;
    private bool resetVel;
    private bool isWallSliding_Left;
    private bool isWallSliding_Right;
    private bool slippery;
    [SerializeField]
    private bool canJump = false;
    private bool canClimb;
    private bool isJumpPress = false;
    private bool playerEnabled = true;
    private bool canAttack = false;



    private void Awake()
    {
        MasterController_S.player_S = this;
    }


    // Start is called before the first frame update
    void Start()
    {
        collider2Ds = new ArrayList();
        rend = GetComponentInChildren<SpriteRenderer>();
        playerColor = rend.color;
        anim = GetComponentInChildren<Animator>();
        rend.enabled = true;
        player = GameObject.Find("Player Temp");
        playerStartPoint = player.transform.position;
        playerCheckpoint = playerStartPoint;
        //Debug.Log(playerStartPoint);
        
        rigid.gravityScale = normalGravity;
        childObj = rend.transform;
    }


    // Update is called once per frame
    void Update()
    {
        ModifyVelocity();
        WhereToMove();
        //Debug.Log(rigid.velocity.x);

        if (canJump)
        {
            if (!(Mathf.Abs(rawInputs.x) > 0))
            {
                anim.SetInteger("Status", 0);
            }
            else
            {
                anim.SetInteger("Status", 1);
            }
        }
        else if (rigid.velocity.y > 0)
        {
            anim.SetInteger("Status", 2);
        }
        else if (rigid.velocity.y <= 0)
        {
            anim.SetInteger("Status", 3);
        }
    }


    private void FixedUpdate()
    {
        Move();
    }
    

    private void ModifyVelocity()
    {
        
        float XVelocity = rigid.velocity.x;
        float YVelocity = rigid.velocity.y;
        //Debug.Log("Vel: " + YVelocity);
        //Debug.Log("For:" + rawInputs.y);
        if (!facingDir && rawInputs.x > 0)
        {
            facingDir = true; // Determines the player facing right
            rend.flipX = false;
            childObj.localPosition = new Vector2(-childObj.localPosition.x, childObj.localPosition.y);
        }
        else if (facingDir && rawInputs.x < 0)
        {
            facingDir = false; // Determines the player is facing left
            rend.flipX = true;
            childObj.localPosition = new Vector2(-childObj.localPosition.x, childObj.localPosition.y);
        }

        if (isJumpPress)
        {
            if (rawInputs.y > 0 && YVelocity > 0) // Is the player choosing to continue to jump?
            {
                rigid.gravityScale = jumpingGravity; // Adds a lighter jump when you hold jump
            }
            else
            {
                rigid.gravityScale = normalGravity;
                isJumpPress = false;
            }
        }

        if (!resetVel)
        {
            if (cancelX && !disableSlow)
            {
                rigid.velocity = new Vector2(0, rigid.velocity.y);
            }
            if (slowDown && !slippery && !disableSlow)
            {
                rigid.velocity = new Vector2(rigid.velocity.x * 6f / 10f, rigid.velocity.y);
            }

            // Structured to isolate the velocity and force it to pass certain parameters
            rigid.velocity = new Vector2(Mathf.Clamp(rigid.velocity.x, -10, 10), Mathf.Clamp(rigid.velocity.y, -100, 100)); // Prevents Crazy Fast Movement
        }
        else
        {
                resetVel = false;
        }

        if (canJump)
        {
            if (rawInputs.y > 0 && playerEnabled && Jumping == null) // Is the player choosing to jump up?
            {
                isJumpPress = true;
                if (rigid.velocity.y < 0)
                    rigid.velocity = new Vector2(rigid.velocity.x, 0);
                rigid.AddForce(new Vector2(0, rawInputs.y * jumpHeight), ForceMode2D.Impulse);
                Jumping = StartedJumping();
                StartCoroutine(Jumping);
            }
        }
        else if (canClimb && isWallSliding_Left)
        {
            if (rawInputs.y > 0 && playerEnabled && Jumping == null) // Is the player choosing to jump up?
            {
                StartCoroutine(DisableMovement(wallStunTime));
                isJumpPress = true;
                if (rigid.velocity.y < 0)
                    rigid.velocity = new Vector2(rigid.velocity.x, 0);
                rigid.AddForce(new Vector2(0.6f * jumpHeight, rawInputs.y * jumpHeight), ForceMode2D.Impulse);
                Jumping = StartedJumping();
                StartCoroutine(Jumping);
            }
        }
        else if (canClimb && isWallSliding_Right)
        {
            if (rawInputs.y > 0 && playerEnabled && Jumping == null) // Is the player choosing to jump up?
            {
                StartCoroutine(DisableMovement(wallStunTime));
                isJumpPress = true;
                if (rigid.velocity.y < 0)
                    rigid.velocity = new Vector2(rigid.velocity.x, 0);
                rigid.AddForce(new Vector2(0.6f * -jumpHeight, rawInputs.y * jumpHeight), ForceMode2D.Impulse);
                Jumping = StartedJumping();
                StartCoroutine(Jumping);
            }
        }

        if (parentObj != null)
        {
            rigid.position = new Vector2(rigid.position.x + parentObj.position.x - locationInherit - locationPrevious, rigid.position.y);
            locationPrevious = parentObj.position.x - locationInherit;
        }
    }


    private void WhereToMove()
    {
        if (playerEnabled)
        {
            rawInputs = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
            if (Input.GetButton("Jump") && !Shop_S.isPlayerShopping)
            {
                rawInputs.y = 1;
            }

            if (rawInputs.y < 0)
            {
                isCrouch = true;
            }
            else
            {
                isCrouch = false;
            }
        }
        else
        {
            rawInputs = Vector2.zero;
        }
        if ((!facingDir && Input.GetAxisRaw("Horizontal") > 0) || (facingDir && Input.GetAxisRaw("Horizontal") < 0)) // When to reset Horizontal Movement
        {
            cancelX = true;
        }
        else
        {
            cancelX = false;
            if (Input.GetAxisRaw("Horizontal") == 0)
            {
                slowDown = true;
            }
            else
            {
                slowDown = false;
            }
        }
    }


    private void Move()
    {
        Vector2 modifiedInputs = new Vector2(rawInputs.x * 120, rawInputs.y * 0); // Makes the Jump much heavier
        if (!lockInputs)
        {
            rigid.AddForce(modifiedInputs);
        }
            
    }


    public void HealthFromDialogue(string conditions)
    {
        String[] conditionsBroken = conditions.Split(' ');
        bool add = Boolean.Parse(conditionsBroken[0]);
        bool stun = Boolean.Parse(conditionsBroken[1]);
        int order = Int32.Parse(conditionsBroken[2]);
        MasterController_S.self.ChangeSpecificHealth(add, stun, order);
    }


    private void OnCollisionEnter2D(Collision2D collision)
    {

    }
    private void OnCollisionExit2D(Collision2D collision)
    {

    }


    public void SetCanAttack(bool newCanAttack, int ammo) // Usually called by the dialogue system in scene 1.5
    {
        MasterController_S.self.ChangeAmmo(ammo);
        canAttack = newCanAttack; // Enables the player to attack mode
    }


    public void SetCanAttack(bool newCanAttack) // Usually called by the dialogue system in scene 1.5
    {
        canAttack = newCanAttack; // Enables the player to attack mode
    }


    public void SetCanClimb(bool newWallJump) // Usually called by the dialogue system in scene 1.5
    {
        canClimb = newWallJump; // Enables the player to wall jump

        if (MasterController_S.self.hasClimb != canClimb)
        {
            MasterController_S.self.hasClimb = true; // Makes it so that every other scene, the player can now wall jump
        }
    }


    public void SetWallSlide(int side, bool newWallSlide)
    {
        if (side == -1)
        {
            isWallSliding_Left = newWallSlide;
        }
        else if (side == 1)
        {
            isWallSliding_Right = newWallSlide;
        }
    }

    public void SetCanJump(bool newCanJump)
    {
        canJump = newCanJump;
    }

    public void PositionInheritance(bool activate, Transform other) // Only works if never used by multiple "velocity" parents at once
    {
        if (activate)
        {
            transform.SetParent(other);
            parentObj = other;
            locationPrevious = 0;
            locationInherit = other.position.x;
        }
        else
        {
            transform.SetParent(null);
            parentObj = null;
        }
    }


    public void ReEnableCollision()
    {
        foreach (Collider2D collider in collider2Ds)
        {
            Debug.Log("Ignored");
            Physics2D.IgnoreCollision(colliderObj, collider, false);
        }
        collider2Ds.Clear();
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {

    }

    private void OnTriggerExit2D(Collider2D collision)
    {

    }

    public void RestartGame()
    {
        StartCoroutine(RestartGameDelay());
    }


    public void StopCollision(Collider2D collider, Collider2D other)
    {
        //Debug.Log("ignored");
        if (stunned)
        {
            Physics2D.IgnoreCollision(collider, other, true);

            colliderObj = collider;

            collider2Ds.Add(other);
        }
    }


    public void TakeStunDamage()
    {
        stunned = true;
        StartCoroutine(GotStunned());
        StartCoroutine(TempStunAnim());
    }


    public void TempAnimStunActivate(Vector2 forceToApply)
    {
        player.GetComponent<Rigidbody2D>().AddForce(forceToApply, ForceMode2D.Impulse);
    }


    public void MoneyFromDialogue(float amount)
    {
        MasterController_S.self.ChangeMoneyAmount(amount);
    }


    public void TestIfCanAttack()
    {
        if (canAttack && MasterController_S.self.hasAmmo)
        {
            Attack();
        }
    }

    public void Attack()
    {
        MasterController_S.self.ChangeAmmo(MasterController_S.self.ammo - 1);
        anim.SetTrigger("Attack");
        if (facingDir)
        {
            Instantiate(projectile, new Vector2(transform.position.x + shootingOffset.x, transform.position.y + shootingOffset.y), Quaternion.Euler(0, 0, 0));
            SoundManager_S.PlaySound("throw");
        }
        else
        {
            Instantiate(projectile, new Vector2(transform.position.x - shootingOffset.x, transform.position.y + shootingOffset.y), Quaternion.Euler(0, 0, 180));
            SoundManager_S.PlaySound("throw");
        }
    }

    public void ResetCheckpoint()
    {
        playerCheckpoint = playerStartPoint;
    }

    public void AddCheckpoint(Vector3 newCheckpoint)
    {
        playerCheckpoint = newCheckpoint;
    }


    IEnumerator DisableMovement(float diableTime)
    {
        lockInputs = true;
        disableSlow = true;
        yield return new WaitForSeconds(diableTime);
        disableSlow = false;
        lockInputs = false;
    }

    IEnumerator GotStunned()
    {
        float animTime = MasterController_S.self.HEALTH_STUN_TIME;

        slippery = true;
        lockInputs = true;
        yield return new WaitForSeconds(animTime * 2f / 7f);
        lockInputs = false;
        slippery = false;
        yield return new WaitForSeconds(animTime * 5f / 7f);
        ReEnableCollision();
        stunned = false;
    }

    IEnumerator TempStunAnim()
    {
        Color transparant = new Color(255, 0, 0, 255);

        float animTime = MasterController_S.self.HEALTH_STUN_TIME;

        rend.color = transparant;

        int totalFlashes = 14;

        float flashTime = (animTime / (totalFlashes * 2f));

        for (int i = 0; i < totalFlashes * 2f; i++)
        {
            yield return new WaitForSeconds(flashTime);
            if (i % 2 == 0)
                rend.color = transparant;
            else
                rend.color = playerColor;
        }

        rend.color = playerColor;
    }

    IEnumerator StartedJumping()
    {
        yield return new WaitForSeconds(0.4f);
        Jumping = null;
    }
    
    public IEnumerator RestartGameDelay()
    {
        rend.enabled = false;
        playerEnabled = false;
        rigid.gravityScale = 0;
        yield return new WaitForSeconds(0.5f); // If changed, remember to change PlayerFeet as well      
        cinemachine.OnTargetObjectWarped(transform, new Vector3(playerCheckpoint.x - transform.position.x, playerCheckpoint.y - transform.position.y, 0));
        player.transform.position = playerCheckpoint;
        rigid.gravityScale = normalGravity;
        rend.enabled = true;
        playerEnabled = true;
    }
        
}
