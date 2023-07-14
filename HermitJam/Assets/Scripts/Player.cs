using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using HermitJam;
using UnityEngine;
using UnityEngine.InputSystem;
using Zenject;

public class Player : MonoBehaviour
{
    private TouchAction _touchInput;
    [SerializeField] private Rigidbody2D _rigidbody2D;
    [SerializeField] private float jumpPower;
    [SerializeField] private float rotationDuration;
    [SerializeField] private LayerMask _groundLayer;

    private Tween rotateTween;
    public bool DownGravity => _rigidbody2D?.gravityScale > 0;
    [field: SerializeField] public bool Grounded { get; private set; }

    [Inject]
    public void Construct(TouchAction touchInput)
    {
        _touchInput = touchInput;
        
    }
    // Start is called before the first frame update
    void Start()
    {
        _rigidbody2D = GetComponent<Rigidbody2D>();
        _touchInput.Touch.Enable();
        _touchInput.Touch.TouchPress.performed += OnTapPerformed;
        _touchInput.Touch.TouchHold.performed += OnTapHoldPerformed;
        _touchInput.Touch.TouchSwipe.performed += OnSwipePerformed;
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
        //Debug.Log(context);
    }
    
    public void OnSwipePerformed(InputAction.CallbackContext context)
    {
        //Debug.Log(context);
    }

    void Jump()
    {
        if(!Grounded) return;
        if (_rigidbody2D)
        {
            _rigidbody2D.AddForce(DownGravity ? Vector2.up * jumpPower : Vector2.down * jumpPower,ForceMode2D.Impulse);
        }

        //List<Collider2D> contacts = new List<Collider2D>();
        //_rigidbody2D.GetContacts(contacts);
        
    }

    public void SwitchGravity()
    {
        if(rotateTween?.IsPlaying() == true) return;
        
        
        if(_rigidbody2D)
            _rigidbody2D.gravityScale *= -1;
        Rotate(!DownGravity);
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
                    Debug.Log("morri para " + platform.PlatformType + " "+ platform.PlatformPosition);
                }
            }
            
        }
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.transform.CompareTag("Zombie"))
        {
            
        }
        else if (col.transform.CompareTag("Slide"))
        {
            
        }
    }
}
