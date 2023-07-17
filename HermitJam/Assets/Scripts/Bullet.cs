using System;
using System.Collections;
using System.Collections.Generic;
using HermitJam;
using UnityEngine;

public class Bullet : MonoBehaviour
{

    [SerializeField] private float _speed = 50f;
    private bool moveRight = true;
    public Action<Bullet> OnDeath;
    
    
    private const string HitAnimatorTrigger = "Hit";
    private const string IdleAnimatorState = "Idle";
    private Animator m_Animator;
    private static readonly int Hit = Animator.StringToHash(HitAnimatorTrigger);
    
    private Rigidbody2D _rigidbody2D;

    // Start is called before the first frame update
    void Start()
    {
        m_Animator = GetComponent<Animator>();
        if(_rigidbody2D == null) _rigidbody2D = GetComponent<Rigidbody2D>();
        
        _rigidbody2D.velocity = new Vector2(_speed * (moveRight ? 1f : -1f), 0);
    }

    private void OnEnable()
    {
        if(_rigidbody2D != null)
            _rigidbody2D.velocity = new Vector2(_speed * (moveRight ? 1f : -1f), 0);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.transform.CompareTag("Obstacle"))
        {
            Obstacle obstacle = col.transform.GetComponent<Obstacle>();
            if (obstacle != null)
            {
                if ((obstacle.ObstacleType == ObstacleType.Zombie))
                {
                   obstacle.gameObject.GetComponent<Zombie>()?.Die();
                }
                
                Die();

            }
        }
    }

    public void Die(bool skipAnimation = false)
    {
        if (m_Animator)
        {
            if (skipAnimation == false)
            {
                m_Animator.SetTrigger(Hit);
                float duration = m_Animator.GetCurrentAnimatorClipInfo(0)[0].clip.length;

                Timer.Register(duration, (() => OnDeath?.Invoke(this)));
            }
            else OnDeath?.Invoke(this);
           
        }
        
        
    }

    public void ResetAnimatorState()
    {
        if (m_Animator != null) 
            m_Animator.Play(IdleAnimatorState);
    }
}
