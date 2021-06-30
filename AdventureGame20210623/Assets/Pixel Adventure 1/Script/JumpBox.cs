using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpBox : MonoBehaviour
{
    [Range(0, 10)] public float jumpVelocity = 5f;
    public LayerMask mask;
    public float boxHeight;

    private Vector2 playerSize;
    private Vector2 boxSize;

    private bool jumpRequest = false;
    private bool grounded = false;

    public float fallMultiplier = 2.5f;
    public float lowJumpMultiplier = 2f;

    private Rigidbody2D _rigidbody2D;

    // Start is called before the first frame update
    void Start()
    {
        _rigidbody2D = GetComponent<Rigidbody2D>();
        playerSize = GetComponent<SpriteRenderer>().bounds.size;
        boxSize = new Vector2(playerSize.x * 0.8f, boxHeight);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Jump") && grounded)
        {
            jumpRequest = true;
        }
    }

    private void FixedUpdate()
    {
        if (jumpRequest)
        {
            _rigidbody2D.AddForce(Vector2.up * jumpVelocity, ForceMode2D.Impulse);

            jumpRequest = false;
            grounded = false;
        }
        else
        {
            //(vector2)transform.position _rigidbody2D.transform.position ??
            Vector2 boxCenter = (Vector2)transform.position + (Vector2.down * playerSize * 0.5f);

            //
            if (Physics2D.OverlapBox(boxCenter, boxSize, 0, mask) != null)
            {
                grounded = true;
            }
            else
            {
                grounded = false;
            }
        }

        //3.3修改重力值，在unity-inspector-rigidbody2D中默认是1
        if (_rigidbody2D.velocity.y < 0)
        {
            _rigidbody2D.gravityScale = fallMultiplier;
        }
        else if (_rigidbody2D.velocity.y > 0 && !Input.GetButton("Jump"))
        {
            _rigidbody2D.gravityScale = lowJumpMultiplier;
        }
        else
        {
            _rigidbody2D.gravityScale = 1f;
        }
    }
}
