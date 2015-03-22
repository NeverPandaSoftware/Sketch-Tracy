using UnityEngine;
using System.Collections;

[AddComponentMenu("Scripts/Character Scripts/Player Controller")]
public class PlayerController : MonoBehaviour
{
    #region Variables

    public CharacterState myCharacterState;

    public GameObject ballPreFab;

    private Rigidbody2D rigidbody2d;

    [HideInInspector]
    public bool facingRight = true;
    [HideInInspector]
    public bool jump = false;

    public float maxSpeed = 5f;
    public float jumpForce = 1000f;
    public LayerMask whatIsGround;
    [HideInInspector]
    public FollowPath activePlatform;
    private Vector2 activePlatformPrevLoc;

    public bool atDoor = false;
    [HideInInspector]
    public GameObject doorAt;

    public bool atWell = false;
    //[HideInInspector]
    public GameObject wellAt;

    public bool atCheckpoint = false;
    public GameObject checkpointAt;

    private Transform groundCheck;
    private RaycastHit2D groundHit;
    private bool grounded = false;
    private RaycastHit2D[] hits;
    private Animator anim;
    private bool ball = false;

    private CheckpointManager checkpointManager;

    #endregion

    #region Initialization

    void Awake()
    {
        groundCheck = transform.Find("GroundCheck");
        anim = GetComponent<Animator>();
        rigidbody2d = GetComponent<Rigidbody2D>();
        checkpointManager = GameObject.Find("Checkpoints").GetComponent<CheckpointManager>();
        if (GameManager.Instance.GetCharacterState() == CharacterState.Ball)
            SwitchFromBall();
    }

    #endregion

    #region Update

    void Update()
    {
        // The player is grounded if a linecast to the groundcheck position hits anything on the ground layer.
        groundHit = Physics2D.Linecast(transform.position, groundCheck.position, whatIsGround);
        grounded = groundHit;
        anim.SetBool("Ground", grounded);

        if (grounded)
        {
            if (groundHit.transform.tag == "MovingPlatform")
            {
                activePlatform = groundHit.transform.GetComponent<FollowPath>();
            }
            else
                activePlatform = null;
        }
        else
            activePlatform = null;

        if (IsControllable())
        {
            if (Input.GetButtonDown("Jump") && grounded)
                jump = true;

            if (myCharacterState == CharacterState.Team && Input.GetButtonDown("SwitchBall"))
                anim.SetTrigger("SwitchToBall");

            if (Input.GetButtonDown("SpawnOtherCharacter") && atCheckpoint &&
                myCharacterState != CharacterState.Team)
            {
                SpawnOtherCharacter();
            }

            //TEMPORARY RESPAWN KEY
            if (Input.GetKeyDown(KeyCode.R))
            {
                Respawn();
            }
        }

        anim.SetFloat("vSpeed", rigidbody2d.velocity.y);

        HandleInteractiveObjects();
    }

    void FixedUpdate()
    {
        if (IsControllable())
        {
            float h = Input.GetAxis("Horizontal");

            anim.SetFloat("Speed", Mathf.Abs(h));

            rigidbody2d.velocity = new Vector2(h * maxSpeed, rigidbody2d.velocity.y);

            HandleMovingPlatforms();

            if (h > 0 && !facingRight)
                Flip();
            else if (h < 0 && facingRight)
                Flip();

            if (jump)
            {
                rigidbody2d.AddForce(new Vector2(0f, jumpForce));
                jump = false;
            }
        }
        else
        {
            anim.SetFloat("Speed", 0);
            rigidbody2d.velocity = new Vector2(0, rigidbody2d.velocity.y);
        }
    }

    #endregion

    #region Interactive Object Handling

    private void HandleInteractiveObjects()
    {
        HandleDoors();
        HandleWells();
    }

    private void HandleDoors()
    {
        Door theDoor;
        if (atDoor && Input.GetButtonDown("OpenDoor"))
        {
            if (doorAt != null)
            {
                theDoor = doorAt.GetComponent<Door>();
                if (!theDoor.isOpen)
                {
                    theDoor.OpenDoor();
                    //theDoor.CloseDoor();
                }
                else
                {
                    //playerControl = false;
                    theDoor.CloseDoor();
                }
            }
        }
    }

    private void HandleWells()
    {
        Well theWell;
        if (atWell && Input.GetButtonDown("EnterWell"))
        {
            if (wellAt != null)
            {
                theWell = wellAt.GetComponent<Well>();
                transform.position = new Vector2(theWell.transform.position.x, transform.position.y);
                //playerControl = false;
                theWell.EnterWell();
            }
        }
    }

    private void HandleMovingPlatforms()
    {
        if (activePlatform != null)
        {
            if (activePlatform.velocity.x > 0 && facingRight || activePlatform.velocity.x < 0 && !facingRight)
                rigidbody2d.velocity = new Vector2(rigidbody2d.velocity.x * 2, rigidbody2d.velocity.y);
            else
                rigidbody2d.velocity = new Vector2(rigidbody2d.velocity.x, rigidbody2d.velocity.y);
        }
    }

    #endregion

    #region Triggers & Collisions

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.tag == "Door")
        {
            atDoor = true;
            doorAt = col.gameObject;
        }

        if (col.gameObject.tag == "Checkpoint")
        {
            atCheckpoint = true;
            checkpointAt = col.gameObject;
            col.GetComponent<Checkpoint>().ActivateCheckpoint();
        }
    }

    void OnTriggerExit2D(Collider2D col)
    {
        if (col.gameObject.tag == "Door")
        {
            atDoor = false;
            doorAt = null;
        }

        if (col.gameObject.tag == "Checkpoint")
        {
            atCheckpoint = false;
            checkpointAt = null;
        }
    }

    void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.tag == "Well")
        {
            atWell = true;
            wellAt = col.gameObject;
        }
    }

    void OnCollisionExit2D(Collision2D col)
    {
        if (col.gameObject.tag == "Well")
        {
            atWell = false;
            wellAt = null;
        }
    }

    #endregion

    #region Helper Methods

    void Flip()
    {
        facingRight = !facingRight;

        Vector3 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;
    }

    void OnStartDraw()
    {
        rigidbody2d.isKinematic = true;
        anim.SetBool("Draw", false);
        anim.SetBool("Drawing", true);
    }

    void OnEndDraw()
    {
        rigidbody2d.isKinematic = false;
        anim.SetBool("Drawing", false);
    }

    void SwitchBall()
    {
        int direction = (facingRight) ? 1 : -1;
        Vector2 pos = new Vector2(transform.position.x + (direction * 0.037f), transform.position.y - 0.486f);
        GameObject ball = (GameObject)Instantiate(ballPreFab, pos, transform.rotation);
        ball.name = "Ball";
        ball.GetComponent<BallController>().facingRight = facingRight;

        CameraFollow cam = Camera.main.GetComponent<CameraFollow>();
        if (cam != null)
            cam.SetTarget(ball.transform);

        GameManager.Instance.SetCharacterState(CharacterState.Ball);
        Destroy(gameObject);
    }

    public void SwitchFromBall()
    {
        anim.SetTrigger("SwitchFromBall");
    }

    private bool IsControllable()
    {
        if (myCharacterState == GameManager.Instance.GetCharacterState())
            return true;
        else
            return false;
    }

    void SpawnOtherCharacter()
    {
        if (myCharacterState == CharacterState.Sketch)
        {
            GameObject tracyObj = GameObject.FindGameObjectWithTag("Tracy");

            if (tracyObj != null)
            {
                tracyObj.transform.position = checkpointAt.transform.position;
                tracyObj.GetComponent<Animator>().SetBool("Draw", true);
            }
            else
            {
                GameObject tracyPreFab = GameObject.Find("GameController").GetComponent<CharacterSwitch>().Characters.Tracy;
                GameObject tracy = (GameObject)Instantiate(tracyPreFab, checkpointAt.transform.position, checkpointAt.transform.rotation);
                tracy.name = "Tracy";
                tracy.GetComponent<Animator>().SetBool("Draw", true);
            }
        }
        else if (myCharacterState == CharacterState.Tracy)
        {
            GameObject sketchObj = GameObject.FindGameObjectWithTag("Sketch");

            if (sketchObj != null)
            {
                Debug.Log("SKETCH FOUND");
                //sketchObj.transform.position = checkpointAt.transform.position;
                //sketchObj.GetComponent<Animator>().SetBool("Draw", true);
                Destroy(sketchObj);
            }
            //else
            //{
                GameObject sketchPreFab = GameObject.Find("GameController").GetComponent<CharacterSwitch>().Characters.Sketch;
                GameObject sketch = (GameObject)Instantiate(sketchPreFab, checkpointAt.transform.position, checkpointAt.transform.rotation);
                sketch.GetComponent<Rigidbody2D>().isKinematic = true;
                sketch.name = "Sketch";
                sketch.GetComponent<Animator>().SetBool("Draw", true);
           // }
        }
    }

    void Respawn()
    {
        transform.position = checkpointManager.CurrentCheckpoint.Location;
    }

    #endregion
}

