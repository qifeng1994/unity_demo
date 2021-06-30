using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float speed = 5f;
    public int maxHealth = 5;
    private int _currentHealth;

    public float invincibleTime = 2;
    private float invincibleTimer;
    private bool isInvincible = false;

    public Transform respawnPosition;

    private Rigidbody2D _rigidbody2D;
    private Animator _animator;

    private Vector2 _lookDirection = Vector2.down;
    private Vector2 _currentInput;

    private float _x;
    private float _y;

    public int Health => _currentHealth;
    public GameObject projectilePrefab;


    // Start is called before the first frame update
    void Start()
    {
        //变量初始化
        _rigidbody2D = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();
        _currentHealth = 1;


    }

    // Update is called once per frame
    void Update()
    {
        //
        if (isInvincible)
        {
            invincibleTimer -= Time.deltaTime;

            if (invincibleTimer <= 0)
            {
                isInvincible = false;
            }
        }


        _x = Input.GetAxis("Horizontal");
        _y = Input.GetAxis("Vertical");

        Vector2 movement = new Vector2(_x, _y);

        //判断player是否在移动
        if (!Mathf.Approximately(movement.x, 0.0f) || !Mathf.Approximately(movement.y, 0.0f))
        {
            _lookDirection.Set(movement.x, movement.y);
            _lookDirection.Normalize(); //向量单位化？？向量长度是1
        }

        //这里用来决定player的朝向
        _animator.SetFloat("lookX", _lookDirection.x);
        _animator.SetFloat("lookY", _lookDirection.y);

        //当x y都没有输入的时候，向量movement会渐渐回归到0
        _animator.SetFloat("speed", movement.magnitude); //为什么不能是_lookDirection.magnitude 因为他的模已经被初始为1

        _currentInput = movement; //把局部变量赋值给全局变量


        //当键盘按C时发射子弹
        if (Input.GetKeyDown(KeyCode.C))
        {
            LaunchProjectile();
        }

        //当按键盘X时与NPC对话
        if (Input.GetKeyDown(KeyCode.X))
        {
            RaycastHit2D hit = Physics2D.Raycast(_rigidbody2D.position + Vector2.up * 0.2f, _lookDirection, 2f, LayerMask.GetMask("NPC"));

            if (hit)
            {
                NPCController npcController = hit.collider.GetComponent<NPCController>();

                if (npcController)
                {
                    npcController.DisplayDialog();
                }
            }
        }

    }

    //跟物理有关的操作放在fixupdate
    private void FixedUpdate()
    {
        //
        Vector2 position = _rigidbody2D.position;
        position += _currentInput * speed * Time.deltaTime;
        _rigidbody2D.MovePosition(position);


    }

    public void ChangeHealth(int amount)
    {
        if (amount < 0) //当要掉血时判断是否是无敌状态
        {
            //如果在无敌状态则不掉血，退出判断
            if (isInvincible)
                return;

            //如果不是无敌状态
            isInvincible = true;
            invincibleTimer = invincibleTime;
        }
        _currentHealth = Mathf.Clamp(_currentHealth + amount, 0, maxHealth);
        print("current health: " + _currentHealth);

        if (_currentHealth == 0)
        {
            Respawn();
        }
    }

    private void Respawn()
    {
        ChangeHealth(maxHealth);
        _rigidbody2D.position = respawnPosition.position;
    }

    private void LaunchProjectile()
    {
        GameObject projectileGameObject = null;

        if (_lookDirection == Vector2.down)
        {
            projectileGameObject = Instantiate(projectilePrefab, _rigidbody2D.position + Vector2.down, Quaternion.identity);
        }
        else
        {
            projectileGameObject = Instantiate(projectilePrefab, _rigidbody2D.position + Vector2.up *0.5f, Quaternion.identity);
        }

        Projectile projectile = projectileGameObject.GetComponent<Projectile>();
        projectile.Launch(_lookDirection, 300);
    }
}
