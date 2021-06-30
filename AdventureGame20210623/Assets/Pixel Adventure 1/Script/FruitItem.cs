using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FruitItem : MonoBehaviour
{
    public GameObject collectedEffect;
    public int Score = 100;

    private SpriteRenderer _spriteRenderer;
    private CircleCollider2D _circleCollider2D;
    private AudioSource _audioSource;

    // Start is called before the first frame update
    void Start()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _circleCollider2D = GetComponent<CircleCollider2D>();
        _audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //5.
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            _spriteRenderer.enabled = false;
            _circleCollider2D.enabled = false;

            _audioSource.Play();
            collectedEffect.SetActive(true);

            //6.3
            GameController.Instance.totalScore += Score;
            GameController.Instance.UpdateTotalScore();

            Destroy(gameObject, 0.5f);
        }

    }
}
