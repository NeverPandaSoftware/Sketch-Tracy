using UnityEngine;
using System.Collections;

[AddComponentMenu("Scripts/Character Scripts/Ball Controller")]
public class BallController : MonoBehaviour
{
    public CharacterState myCharacterState = CharacterState.Ball;

    [SerializeField] private float movePower = 5;
    [SerializeField] private bool useTorque = true;
    [SerializeField] private float maxAngularVelocity = 1500;
    [SerializeField] private float jumpPower = 50;
    [SerializeField] private LayerMask whatIsGround;

    private float move;
    private bool jump;

    private const float GROUND_RAY_LENGTH = 0.2f;
    private Rigidbody2D rigidbody2d;

	// Use this for initialization
	void Start ()
    {
        rigidbody2d = GetComponent<Rigidbody2D>();
	}

    void Update()
    {
        move = Input.GetAxis("Horizontal");
        if (Physics2D.Raycast(transform.position, -Vector2.up, GROUND_RAY_LENGTH, whatIsGround) && Input.GetButtonDown("Jump"))
        {
            jump = true;
        }
    }
	
	// Update is called once per frame
	void FixedUpdate ()
    {
        if (IsControllable())
            Move();

        jump = false;
	}

    void Move()
    {
        if (useTorque)
        {
            rigidbody2d.AddTorque(-move * movePower);
            rigidbody2d.angularVelocity = Mathf.Clamp(rigidbody2d.angularVelocity, -maxAngularVelocity, maxAngularVelocity);
            //Debug.Log(rigidbody.angularVelocity);
        }
        else
        {
            rigidbody2d.AddForce((move * transform.right) * movePower);
        }

        if (jump)
        {
            Debug.Log("GROUNDED + JUMP");
            rigidbody2d.AddForce(Vector2.up * jumpPower, ForceMode2D.Impulse);
            jump = false;
        }
    }

    private bool IsControllable()
    {
        if (myCharacterState == GameManager.Instance.GetCharacterState())
            return true;
        else
            return false;
    }
}
