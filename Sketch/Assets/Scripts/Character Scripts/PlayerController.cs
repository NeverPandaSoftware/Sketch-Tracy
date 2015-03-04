using UnityEngine;
using System.Collections;

[AddComponentMenu("Scripts/Character Scripts/Player Controller")]
public class PlayerController : MonoBehaviour
{
    #region Variables

    public CharacterState myCharacterState;

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
        //checkpointManager = GameObject.Find("Checkpoints").GetComponent<CheckpointManager>();
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
                SwitchBall();

            //TEMPORARY RESPAWN KEY
            if (Input.GetKeyDown(KeyCode.R))
            {
                Respawn();
            }
        }

        anim.SetFloat("vSpeed", GetComponent<Rigidbody2D>().velocity.y);

        HandleInteractiveObjects();
    }

    void FixedUpdate()
    {
        if (IsControllable())
        {
            float h = Input.GetAxis("Horizontal");

            anim.SetFloat("Speed", Mathf.Abs(h));

            GetComponent<Rigidbody2D>().velocity = new Vector2(h * maxSpeed, GetComponent<Rigidbody2D>().velocity.y);

            HandleMovingPlatforms();

            if (h > 0 && !facingRight)
                Flip();
            else if (h < 0 && facingRight)
                Flip();

            if (jump)
            {
                GetComponent<Rigidbody2D>().AddForce(new Vector2(0f, jumpForce));
                jump = false;
            }
        }
        else
        {
            anim.SetFloat("Speed", 0);
            GetComponent<Rigidbody2D>().velocity = new Vector2(0, GetComponent<Rigidbody2D>().velocity.y);
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
                GetComponent<Rigidbody2D>().velocity = new Vector2(GetComponent<Rigidbody2D>().velocity.x * 2, GetComponent<Rigidbody2D>().velocity.y);
            else
                GetComponent<Rigidbody2D>().velocity = new Vector2(GetComponent<Rigidbody2D>().velocity.x, GetComponent<Rigidbody2D>().velocity.y);
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

    void SwitchBall()
    {
        
    }

    private bool IsControllable()
    {
        if (myCharacterState == GameManager.Instance.GetCharacterState())
            return true;
        else
            return false;
    }

    void Respawn()
    {
        transform.position = checkpointManager.CurrentCheckpoint.Location;
    }

    #endregion
}

