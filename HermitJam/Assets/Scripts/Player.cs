using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using HermitJam;
using UnityEngine;
using UnityEngine.InputSystem;
using Zenject;

public partial class Player : MonoBehaviour
{
    private TouchAction _touchInput;
    [SerializeField] private Rigidbody2D _rigidbody2D;
    [SerializeField] private float jumpPower;
    [SerializeField] private float rotationDuration;
    [SerializeField] private LayerMask _groundLayer;

    private Tween rotateTween;
    [SerializeField] private bool _grounded;
    [SerializeField] private bool _sliding;
    [SerializeField] private bool _shooting;
    [SerializeField] private bool _dead;
    [SerializeField] private bool _poisoned;
    [SerializeField] private GameObject lastAcidTouched;
    private const float DistanceBetweenPlatforms = 5f;

    public bool Grounded
    {
        get => _grounded;
        private set
        {
            _grounded = value; 
            if(_animator) _animator.SetBool(GroundedAnimatorBool,_grounded);
        }
    }

    public bool Sliding
    {
        get => _sliding;
        private set
        {
            _sliding = value;
            if(_animator) _animator.SetBool(SlidingAnimatorBool,_sliding);
        }
    }

    public bool Shooting
    {
        get => _shooting;
        private set
        {
            _shooting = value;
            if(_animator) _animator.SetBool(ShootingAnimatorBool,_shooting);
        }
    }
    
    public bool Dead
    {
        get => _dead;
        private set
        {
            _dead = value;
            if(_animator) _animator.SetBool(DeadAnimatorBool,_dead);
        }
    }
    
    public bool Poisoned
    {
        get => _poisoned;
        private set
        {
            _poisoned = value;
        }
    }

    public bool DownGravity
    {
        get { return _rigidbody2D?.gravityScale > 0; }
    }

    [SerializeField] private float slideDuration = 1.5f;
    [SerializeField] private float poisonDuration = 5f;
    [SerializeField] private float rateOfFire = 0.25f;
    [SerializeField] private Weapon _weapon;
    private Timer m_SlideTimer;
    private Timer m_PoisonTimer;
    private Timer m_WeaponTimer;

    [Inject]
    public void Construct(TouchAction touchInput)
    {
        _touchInput = touchInput;
        
    }
    // Start is called before the first frame update
    void Start()
    {
        _rigidbody2D = GetComponent<Rigidbody2D>();
        if (!_animator) _animator = GetComponentInChildren<Animator>() ?? GetComponent<Animator>();
        if (!_weapon) _weapon = GetComponentInChildren<Weapon>() ?? GetComponent<Weapon>();
        
        _touchInput.Touch.Enable();
        _touchInput.Touch.TouchPress.performed += OnTapPerformed;
        _touchInput.Touch.TouchHold.performed += OnTapHoldPerformed;
        _touchInput.Touch.TouchHold.canceled += OnTapHoldCanceled;
        _touchInput.Touch.TouchSwipe.performed += OnSwipePerformed;
    }

    private void Update()
    {
        //Grounded = Physics2D.Raycast(transform.position, DownGravity ? Vector3.down : Vector3.up, 1.37f, _groundLayer);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Grounded = Physics2D.Raycast(transform.position, DownGravity ? Vector3.down : Vector3.up, 1.37f, _groundLayer);
    }
    
    public void OnTapPerformed(InputAction.CallbackContext context)
    {
        Jump();
        //Debug.Log(context);
    }
    
    public void OnTapHoldPerformed(InputAction.CallbackContext context)
    {
        ToggleShooting(true);
        //Debug.Log(context);
    }
    
    public void OnTapHoldCanceled(InputAction.CallbackContext context)
    {
        ToggleShooting(false);
        //Debug.Log(context);
    }
    
    public void OnSwipePerformed(InputAction.CallbackContext context)
    {
        Slide();
        //Debug.Log(context);
    }

    void Jump()
    {
        if(!Grounded || Sliding || Dead) return;
        if (_rigidbody2D)
        {
            _rigidbody2D.AddForce(DownGravity ? Vector2.up * jumpPower : Vector2.down * jumpPower,ForceMode2D.Impulse);
        }

        //List<Collider2D> contacts = new List<Collider2D>();
        //_rigidbody2D.GetContacts(contacts);
        
    }

    void Shoot()
    {
        if(Sliding || !Shooting || Dead) return;
        
        _weapon.Shoot();
    }

    void ToggleShooting(bool startShooting)
    {
        if(Sliding || Dead || _weapon == null) return;
        
        
        Shooting = startShooting;

        if (Shooting)
        {
            if (m_WeaponTimer?.isDone == null)
            {
                m_WeaponTimer = Timer.Register(rateOfFire, Shoot, null, true, true);
                Shoot();
            }
            else if (m_WeaponTimer?.isPaused == true ||m_WeaponTimer?.isDone == true)
            {
                m_WeaponTimer.Complete();
                m_WeaponTimer.Restart();
            }
        }
        else
        {
            m_WeaponTimer?.Pause();
        }
    }
    
    void Slide()
    {
        if(Sliding || !Grounded  || Dead) return;

        Sliding = true;
        if (m_SlideTimer?.isDone != true)
        {
            m_SlideTimer = Timer.Register(slideDuration, (() =>
            {
                Sliding = false;
                Debug.Log("terminei slide " + Sliding);
            }),null,true);
        }
        else
        {
            m_SlideTimer.Restart();
        }
            
    }

    public void SwitchGravity()
    {
        if(rotateTween?.IsPlaying() == true) return;
        
        
        if(_rigidbody2D)
            _rigidbody2D.gravityScale *= -1;
        Rotate(!DownGravity);
    }

    void Poison()
    {

        if (Poisoned)
        {
            Die();
            m_PoisonTimer?.Cancel();
            return;
        }
        SpriteRenderer child = transform.GetChild(0)?.GetComponent<SpriteRenderer>();
        if (child)
        {
            child.DOColor(Color.green, 1.5f);
            if (m_PoisonTimer?.isDone != true)
            {
                m_PoisonTimer = Timer.Register(poisonDuration, (() =>
                {
                    child.DOColor(Color.white, 1.5f);
                    Poisoned = false;
                }),null,true);
            }
            else
            {
                m_PoisonTimer.Restart();
            }
        }

        Poisoned = true;

    }

    public void Rotate(bool upsideDown)
    {
        SpriteRenderer child = transform.GetChild(0)?.GetComponent<SpriteRenderer>();
        if (child)
        {
            if (rotateTween != null)
            {
                rotateTween.Kill();
            }
            rotateTween = child.transform.DORotate(new Vector3(0, 0, upsideDown ? 180 : 0),rotationDuration).OnKill(()=> rotateTween = null);
            child.flipX = upsideDown;

        }
    }

    private void OnCollisionEnter2D(Collision2D col)
    {
        if (col.transform.CompareTag("Platform"))
        {
            Platform platform = col.transform.GetComponent<Platform>();
            if (platform != null)
            {
                if (platform.IsHazard)
                {
                    if (platform.PlatformType == PlatformType.Spike)
                    {
                        Die();
                        Debug.Log("morri para " + platform.PlatformType + " "+ platform.PlatformPosition);
                    }
                    else if (platform.PlatformType == PlatformType.Acid)
                    {
                        bool isAcidRightNextToLast = false;
                        if(lastAcidTouched != null)
                            isAcidRightNextToLast = Vector3.Distance(platform.transform.position, lastAcidTouched.transform.position) <= DistanceBetweenPlatforms + DistanceBetweenPlatforms * 0.05f;
                        
                        //we do this so if there is acids in a row, we treat it as one big "acid"
                        if (isAcidRightNextToLast == false)
                        {
                            Debug.Log("triggei poison " + Poisoned);
                            Poison();
                        }
                        lastAcidTouched = platform.gameObject;
                    }
                    
                }
            }
            
        }
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.transform.CompareTag("Obstacle"))
        {
            Obstacle obstacle = col.transform.GetComponent<Obstacle>();
            if (obstacle != null)
            {
                if ((obstacle.ObstacleType == ObstacleType.Slide && !Sliding))
                {
                    Die();
                }
                
            }
            
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.transform.CompareTag("Obstacle"))
        {
            Obstacle obstacle = other.transform.GetComponent<Obstacle>();
            if (obstacle != null)
            {
                if (obstacle.ObstacleType == ObstacleType.Slide)
                {
                    m_SlideTimer?.Complete();
                }
            }
        }
    }

    public void Die()
    {
        //Dead = true;
        
    }

    private void OnDisable()
    {
        _touchInput.Touch.TouchPress.performed -= OnTapPerformed;
        _touchInput.Touch.TouchHold.performed -= OnTapHoldPerformed;
        _touchInput.Touch.TouchHold.canceled -= OnTapHoldCanceled;
        _touchInput.Touch.TouchSwipe.performed -= OnSwipePerformed;
    }
}
