using UnityEngine;

public class Movements : MonoBehaviour
{
    private const float fallMultiplier = 2.5f;
    private const float lowJumpMultiplier = 2f;

    private const float jumpForce = 9f;
    private const float playerSize = 100f;

    private const float maxSpeedGrounded = 25f;
    private const float maxSpeedAir = 5f;
    private float maxSpeed;

    private const float onFrictionPercent = 0.8f;

    public const float speed = 90f;
    private Rigidbody2D rb2d;
    private bool jump;

    // Use this for initialization
    void Start ()
    {
        rb2d = GetComponent<Rigidbody2D>();
        jump = false;
        maxSpeed = maxSpeedAir;
    }
	
	// Update is called once per frame
	void Update () {
		if(jump && Input.GetKey(KeyCode.Space))
        {
            rb2d.velocity = new Vector2(rb2d.velocity.x, jumpForce);
        }

        if(rb2d.velocity.y < 0)
        {
            rb2d.velocity += Vector2.up * Physics2D.gravity.y * (fallMultiplier - 1) * Time.deltaTime;
        } else if (rb2d.velocity.y > 0 && !Input.GetKey(KeyCode.Space))
        {
            rb2d.velocity += Vector2.up * Physics2D.gravity.y * (lowJumpMultiplier - 1) * Time.deltaTime;
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
            transform.localScale = new Vector2(playerSize, playerSize);
        }

        if (h < -0.1f)
        {
            transform.localScale = new Vector2(-playerSize, playerSize);
        }
    }

    // Collision events
    private void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.tag == "Ground")
        {
            jump = true;
            maxSpeed = maxSpeedGrounded;
        }
    }

    private void OnCollisionStay2D(Collision2D col)
    {
        if (col.gameObject.tag == "Ground")
        {
            float limitedSpeed = Mathf.Clamp(rb2d.velocity.x, -maxSpeed, maxSpeed);
            rb2d.velocity = new Vector2(limitedSpeed * onFrictionPercent, rb2d.velocity.y);
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
}
