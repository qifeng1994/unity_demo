using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    private Rigidbody2D _rigidbody2D;

    //Awake早于start
    private void Awake()
    {
        _rigidbody2D = GetComponent<Rigidbody2D>();
    }

    //发射子弹
    public void Launch(Vector2 direction, float force)
    {
        _rigidbody2D.AddForce(direction * force);
    }

    //当发生碰撞时销毁物体
    private void OnCollisionEnter2D(Collision2D collision)
    {
        //如果碰到的是敌人，则触发fixed函数特效
        EnemyController enemyController = collision.collider.GetComponent<EnemyController>();

        if (enemyController)
        {
            enemyController.Fixed();
        }

        Destroy(gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
