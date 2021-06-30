using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallingPlatform : MonoBehaviour
{
    public float fallingTime = 3;

    private TargetJoint2D _targetJoint2D;
    private BoxCollider2D _boxCollider2D;

    // Start is called before the first frame update
    void Start()
    {
        _targetJoint2D = GetComponent<TargetJoint2D>();
        _boxCollider2D = GetComponent<BoxCollider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //8.
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            Invoke("Falling", fallingTime);
        }

        if (collision.gameObject.tag == "Spike")
        {
            Destroy(gameObject);
        }
    }

    private void Falling()
    {
        _targetJoint2D.enabled = false;
        _boxCollider2D.isTrigger = false; //这句话不知道为什么，感觉不需要
    }
}
