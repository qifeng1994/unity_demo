using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public float speed = 2f;
    public float timeToChangeDirection = 2f;
    public bool horizontal;

    private float _remainingTime;
    private Vector2 _direction = Vector2.left;

    private Rigidbody2D _rigidBody2D;
    private Animator _animator;
    private static readonly int lookX = Animator.StringToHash("lookX");
    private static readonly int lookY = Animator.StringToHash("lookY");

    private bool _repaired;
    public GameObject somkeEffect;
    public ParticleSystem fixedEffect;

    // Start is called before the first frame update
    void Start()
    {
        //变量初始化
        _rigidBody2D = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();

        _remainingTime = timeToChangeDirection;

        _direction = horizontal ? Vector2.left : Vector2.down;

    }

    // Update is called once per frame
    void Update()
    {
        //如果敌人在被击中的状态，则敌人不动
        if (_repaired)
            return;

        _remainingTime -= Time.deltaTime;
        if (_remainingTime <= 0) 
        {
            _remainingTime = timeToChangeDirection;
            _direction *= -1;
        }

        //
        _animator.SetFloat(lookX, _direction.x);
        _animator.SetFloat(lookY, _direction.y);

        //

    }

    private void FixedUpdate()
    {
        _rigidBody2D.MovePosition(_rigidBody2D.position + _direction * speed * Time.deltaTime);

    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        //如果敌人处在被击中的状态，则不进行碰撞操作
        if (_repaired)
            return;

        PlayerController controller = collision.collider.GetComponent<PlayerController>();

        if (controller)
        {
            controller.ChangeHealth(-1);
        }
    }

    public void Fixed()
    {
        _animator.SetTrigger("fixed");
        somkeEffect.SetActive(false);
        //产生爆炸效果
        Instantiate(fixedEffect, _rigidBody2D.position + Vector2.up * 0.5f, Quaternion.identity);
        //让刚体失活
        _rigidBody2D.simulated = false;
        //敌人处于被击中的状态
        _repaired = true;

    }
}
