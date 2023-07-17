using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public partial class Player : MonoBehaviour
{

    void SetupInput()
    {
        _touchInput.Touch.Enable();
        _touchInput.Touch.TouchPress.performed += OnTapPerformed;
        _touchInput.Touch.TouchHold.performed += OnTapHoldPerformed;
        _touchInput.Touch.TouchHold.canceled += OnTapHoldCanceled;
        _touchInput.Touch.TouchSwipe.performed += OnSwipePerformed;
    }
    public void OnTapPerformed(InputAction.CallbackContext context)
    {
        Jump();
    }
    
    public void OnTapHoldPerformed(InputAction.CallbackContext context)
    {
        ToggleShooting(true);
    }
    
    public void OnTapHoldCanceled(InputAction.CallbackContext context)
    {
        ToggleShooting(false);
    }
    
    public void OnSwipePerformed(InputAction.CallbackContext context)
    {
        Slide();
    }

    void UnsubscribeInput()
    {
        _touchInput.Touch.TouchPress.performed -= OnTapPerformed;
        _touchInput.Touch.TouchHold.performed -= OnTapHoldPerformed;
        _touchInput.Touch.TouchHold.canceled -= OnTapHoldCanceled;
        _touchInput.Touch.TouchSwipe.performed -= OnSwipePerformed;
    }
}
