using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class Player : MonoBehaviour
{
    [SerializeField]  Animator _animator;

    private const string RUN_ANIMATION = "Run";
    private const string RUN_SHOOT_ANIMATION = "RunShoot";
    private const string SLIDE_ANIMATION = "Slide";
    private const string DEAD_ANIMATION = "Dead";
    private const string JUMP_SHOOT_ANIMATION = "JumpShoot";
    private const string JUMP_ANIMATION = "Jump";
    
    private const string GroundedAnimatorBool = "Grounded";
    private const string ShootingAnimatorBool = "Shooting";
    private const string SlidingAnimatorBool = "Sliding";
    private const string DeadAnimatorBool = "Dead";
    
}
