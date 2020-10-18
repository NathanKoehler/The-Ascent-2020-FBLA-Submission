using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Wolf_S : MonoBehaviour
{
    public Transform eyes;
    public LineRenderer lineRenderer;

    //[HideInInspector]
    public bool Aggro;
    [HideInInspector]
    public float locationInherit;
    [HideInInspector]
    public float locationPrevious;
    public float normalSpeed;
    public float aggroSpeed;
    public float TURN_AROUND_TIME;
    public float FRICTION_WHEN_STOPPING;
    public float health = 2f;


    private Transform parentObj;
    private Rigidbody2D rigid;
    private GameObject player;
    private Animator anim;
    private SpriteRenderer spriteRenderer;
    [SerializeField]
    private float disableTime;
    private float frictionNormal = 0;
    private float RawXDirection = 1;
    [SerializeField]
    private float ModXDirection = 1;
    private float ModEyesPos = 1;
    private int layer_mask;
    [SerializeField]
    private bool canTurnAround = true;
    private bool active = true;
    private bool lockInputs;
    private bool seePlayer;
    [SerializeField]
    private bool facingPlayer;
    [SerializeField]
    private bool facingRight = true;
    [SerializeField]
    private bool nearCliff;
    private bool noGround = false;

    // Start is called before the first frame update
    void Start()
    {
        player = MasterController_S.player_S.gameObject;
        rigid = GetComponent<Rigidbody2D>();
        layer_mask = LayerMask.GetMask("Player and Terrain");
        anim = GetComponentInChildren<Animator>();
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if (active && spriteRenderer.isVisible)
        {
            AnimFox();
            ModifyVelocity();
            StartCoroutine("TestForPlayer");
        }
    }

    private void FixedUpdate()
    {
        if (active && spriteRenderer.isVisible)
        {
            Move();
        }
    }

    private void AnimFox()
    {
        if (nearCliff)
        {
            anim.SetInteger("Status", 0);
        }
        else if (Aggro)
        {
            anim.SetInteger("Status", 1);
        }
        else
        {
            anim.SetInteger("Status", 2);
        }
    }

    private void ModifyVelocity()
    {
        if (canTurnAround)
            ModXDirection = 1;

        if (Aggro)
        {
            if (nearCliff && facingPlayer)
            {
                ModXDirection = 0;
                StartCoroutine("StopMoving");
            }

            ChasePlayer();
        }

            if (parentObj != null)
        {
            rigid.position = new Vector2(rigid.position.x + parentObj.position.x - locationInherit - locationPrevious, rigid.position.y);
            locationPrevious = parentObj.position.x - locationInherit;
        }
    }

        private void Move()
    {
        Vector2 modifiedInputs = new Vector2(RawXDirection * ModXDirection * 10, 0);

        if (Aggro)
        {
            modifiedInputs = new Vector2(RawXDirection * ModXDirection * 20, 0);
            rigid.velocity = new Vector2(Mathf.Clamp(rigid.velocity.x, -aggroSpeed, aggroSpeed), Mathf.Clamp(rigid.velocity.y, -100, 100)); // Prevents Crazy Fast Movement
        }
        else
        {
            rigid.velocity = new Vector2(Mathf.Clamp(rigid.velocity.x, -normalSpeed, normalSpeed), Mathf.Clamp(rigid.velocity.y, -100, 100)); // Prevents Crazy Fast Movement

            if (Mathf.Abs(rigid.velocity.x) < 0.01)
            {
                StartCoroutine("Asleep");
            }
        }

        if (!lockInputs)
            rigid.AddForce(modifiedInputs);
    }

    private void ChasePlayer()
    {
        if (player == null)
        {
            Debug.LogError("Player Not Found in ChasePlayer() Script in Enemy_Wolf_S");
        }


        if (canTurnAround)
        {
            if (isMissaligned())
            {
                facingPlayer = false;

                //Debug.Log("A");
                StartCoroutine("TurnAround");
            }
            else
            {
                facingPlayer = true;
            }
        }
    }

    private bool isMissaligned()
    {
        if (transform.position.x > player.transform.position.x && facingRight)
        {
            return true;
        }
        else if (transform.position.x < player.transform.position.x && !facingRight)
        {
            return true;
        }

        return false;
    }

    private void Flip()
    {
        facingRight = !facingRight;

        Vector3 scale = transform.localScale;
        scale.x *= -1;
        transform.localScale = scale;
        //spriteRenderer.flipX = facingRight;
        if (isMissaligned())
            facingPlayer = false;
    }

    public void HittingWall()
    {
        if (canTurnAround)
        {
            StartCoroutine("TurnAround");
        }
    }

    public void Die()
    {
        active = false;
        rigid.freezeRotation = false;
        rigid.rotation = 180;
        spriteRenderer.color = Color.gray;
        anim.SetInteger("Status", 0);
        transform.GetChild(0).GetComponent<ParticleSystem>().Play();
    }

    public void ChangeHealth(int change)
    {
        health += change;
        if (health <= 0)
        {
            health = 0;
            Die();
        }
    }

    public void OnGround()
    {
        noGround = false;
    }

    public void BelowYou()
    {
        noGround = true;
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


    // Coroutines

    IEnumerator TestForPlayer() // Uses Raycasts and the physics engine to see if the player is horizontal to the AI, also sets aggro
    {
        RaycastHit2D rayDesc = Physics2D.Raycast(eyes.position, eyes.right * RawXDirection, layer_mask);

        if (rayDesc)
        {
            Player_S playerScript_RAY = rayDesc.transform.GetComponent<Player_S>();
            if (playerScript_RAY != null)
            {
                player = playerScript_RAY.gameObject;
                seePlayer = true;
                Aggro = true;
                StopCoroutine("ResetVision");
            }
            else
            {
                seePlayer = false;
                if (Aggro)
                    StartCoroutine("ResetVision");
            }

            if (lineRenderer != null)
            {
                lineRenderer.SetPosition(0, eyes.position);
                lineRenderer.SetPosition(1, rayDesc.point);
            }
        }
        else
        {
            if (lineRenderer != null)
            {
                lineRenderer.SetPosition(0, eyes.position);
                lineRenderer.SetPosition(1, eyes.position + eyes.right * RawXDirection * 100);
            }
        }

        if (lineRenderer != null)
        {
            lineRenderer.enabled = true;
        }

        yield return new WaitForEndOfFrame();

        if (lineRenderer != null)
        {
            lineRenderer.enabled = false;
        }

    }

    IEnumerator StopMoving() // Use friction to stop the AI from moving
    {
        if (!noGround)
            rigid.drag = FRICTION_WHEN_STOPPING;
        yield return new WaitForSeconds(TURN_AROUND_TIME * 2f);
        rigid.drag = frictionNormal;
    }


    IEnumerator TurnAround() // Turn the AI around
    {
        canTurnAround = false;

        yield return new WaitForEndOfFrame();

        if (!noGround)
            rigid.drag = FRICTION_WHEN_STOPPING;

        //ModXDirection = 0;

        RawXDirection = RawXDirection * -1;

        Flip();

        //yield return new WaitWhile(() => Aggro && !noGround);

        yield return new WaitForSeconds(TURN_AROUND_TIME);

        
        rigid.drag = frictionNormal;
        
        canTurnAround = true;
        ModXDirection = 1;
    }

    IEnumerator ResetVision() // Drops Aggro after certain time has passed
    {
        yield return new WaitForSeconds(2f);

        yield return new WaitWhile(() => seePlayer);

        Aggro = false;

        if (canTurnAround && nearCliff)
        {
            //Debug.Log("B");
            StartCoroutine("TurnAround");
        }
    }


    IEnumerator IsNearCliff(Collider2D collision)
    {
        yield return new WaitForSeconds(0.2f);
        if (nearCliff && GetComponent<EdgeCollider2D>().IsTouchingLayers(layer_mask))
        {
            nearCliff = false;
        }
    }


    IEnumerator Asleep()
    {
        yield return new WaitForSeconds(1f);
        if (!Aggro && canTurnAround && Mathf.Abs(rigid.velocity.x) < 0.01)
        {
            StartCoroutine("TurnAround");
        }
        yield return new WaitForSeconds(1f);
        if (!Aggro && !lockInputs && Mathf.Abs(rigid.velocity.x) < 0.01)
        {
            lockInputs = true;
            canTurnAround = false;
            StartCoroutine(DisableMovement(disableTime));
            if (facingRight)
                rigid.AddForce(new Vector2(5f, 10f), ForceMode2D.Impulse);
            else
                rigid.AddForce(new Vector2(-5f, 10f), ForceMode2D.Impulse);
        }
    }

    IEnumerator DisableMovement(float newTime)
    {
        yield return new WaitForSeconds(newTime);
        lockInputs = false;
        canTurnAround = true;
    }

    // Triggers and Collision

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Shootable"))
        {
            ChangeHealth(-1);

            Destroy(collision.gameObject);
        }
        else if (collision.gameObject.CompareTag("Player"))
        {
            if (active)
            {
                if (Aggro)
                {
                    anim.SetTrigger("Attack");
                }

                Player_S playerScript = MasterController_S.player_S;

                MasterController_S.self.ChangeHealth(-1, true, new Vector2((player.transform.position.x - transform.position.x) * 20f, 15f));
                SoundManager_S.PlaySound("bark");
                playerScript.StopCollision(collision.collider, collision.otherCollider);
            }
            else
                Physics2D.IgnoreCollision(collision.otherCollider, collision.collider);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Floor") || collision.gameObject.CompareTag("Elusive"))
        {
            nearCliff = false;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Floor") || collision.gameObject.CompareTag("Elusive"))
        {
            nearCliff = true;

            StartCoroutine(IsNearCliff(collision));

            if (canTurnAround && !Aggro)
            {
                //Debug.Log("C");
                StartCoroutine("TurnAround");
            }
        }
    }


    // Depricated Scripts

    
    public void HeadUp() // Depricated
    {
        Aggro = true;
        StopCoroutine("TurnAround");
        StopCoroutine("ResetVision");
        RawXDirection = RawXDirection * -1;
        rigid.drag = frictionNormal;
        rigid.Sleep();
        rigid.WakeUp();
        GetComponentInChildren<SpriteRenderer>().flipX = true;
        Flip();
        foreach (BoxCollider2D boxCollider2D in gameObject.GetComponents<BoxCollider2D>())
        {
            boxCollider2D.enabled = true;
        }
        canTurnAround = true;
        ModXDirection = 1;
    }

}
