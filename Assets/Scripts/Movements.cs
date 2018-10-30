using UnityEngine;

public class Movements : MonoBehaviour
{
    public const float maxSpeedGrounded = 25f;
    public const float maxSpeedAir = 5f;
    public float maxSpeed;

    public const float speed = 90f;
    private Rigidbody2D rb2d;
    private bool jump;
    private bool boostJump;
    private float diffHoldJump;

    // Use this for initialization
    void Start ()
    {
        rb2d = GetComponent<Rigidbody2D>();
        jumpValues(false);
        maxSpeed = maxSpeedAir;
        diffHoldJump = Time.time;
    }
	
	// Update is called once per frame
	void Update () {
		if(jump && Input.GetKeyDown(KeyCode.Space))
        {
            rb2d.velocity = new Vector2(rb2d.velocity.x, 7f);
            diffHoldJump = Time.time;
        }

        if(boostJump && Input.GetKey(KeyCode.Space) && (Time.time - diffHoldJump) < 2)
        {
            rb2d.velocity = new Vector2(rb2d.velocity.x, 7f);
        }
	}

    private void FixedUpdate()
    {
        float h = Input.GetAxis("Horizontal");

        rb2d.AddForce(Vector2.right * speed * h);

        float limitedSpeed = Mathf.Clamp(rb2d.velocity.x, -maxSpeed, maxSpeed);
        rb2d.velocity = new Vector2(limitedSpeed, rb2d.velocity.y);

        if (h > 0.1f)
        {
            transform.localScale = new Vector2(100f, 100f);
        }

        if (h < 0.1f)
        {
            transform.localScale = new Vector2(-100f, 100f);
        }
    }

    // Collision events
    private void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.tag == "Ground")
        {
            jumpValues(true);
            maxSpeed = maxSpeedGrounded;
        }
    }

    private void OnCollisionStay2D(Collision2D col)
    {
        if (col.gameObject.tag == "Ground")
        {
            float limitedSpeed = Mathf.Clamp(rb2d.velocity.x, -maxSpeed, maxSpeed);
            rb2d.velocity = new Vector2(limitedSpeed*0.8f, rb2d.velocity.y);
        }
    }

    private void OnCollisionExit2D(Collision2D col)
    {
        if (col.gameObject.tag == "Ground")
        {
            jump = false;
            maxSpeed = maxSpeedAir;
        }
    }

    // Functions
    private void jumpValues(bool value)
    {
        jump = value;
        boostJump = value;
    }
}
