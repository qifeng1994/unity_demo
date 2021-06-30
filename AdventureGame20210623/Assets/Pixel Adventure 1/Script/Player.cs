using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public GameObject endPoint;
    // public float delayTime = 2f;
    public float speed = 5f;
    private Rigidbody2D _rigidbody2D;
    private Animator _animator;

    private float x;
    private float y;

    // Start is called before the first frame update
    void Start()
    {
        _rigidbody2D = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        x = Input.GetAxis("Horizontal");
        y = Input.GetAxis("Vertical");

        //2.1当左右有输入的时候，播放动画run
        if (x > 0)
        {
            _rigidbody2D.transform.eulerAngles = new Vector3(0f, 0f, 0f);
            _animator.SetBool("run", true);
        }
        if (x < 0)
        {
            _rigidbody2D.transform.eulerAngles = new Vector3(0f, 180f, 0f);
            _animator.SetBool("run", true);
        }
        if (x > -0.01f && x < 0.01f)
        {
            _animator.SetBool("run", false);
        }
        Run();
    }

    //2.2player跑了之后位移改变
    private void Run()
    {
        Vector3 movement = new Vector3(x, y, 0);
        _rigidbody2D.transform.position += movement * speed * Time.deltaTime;
    }

    
    private void OnCollisionEnter2D(Collision2D collision)
    {
        //7.1人物碰到陷阱之后游戏结束，这里为什么不把spike设定为trigger??
        if (collision.gameObject.tag == "Spike" || collision.gameObject.tag == "Saw")
        {
            GameController.Instance.ShowGameOverPanel();
            Destroy(gameObject);
        }
        else if (collision.gameObject.tag == "EndPoint")
        {
            //展示胜利界面，延时2秒，把终点的碰撞器取消
            endPoint.GetComponent<BoxCollider2D>().enabled = false;
            GameController.Instance.ShowGameWinPanel();
        }

    }
}
