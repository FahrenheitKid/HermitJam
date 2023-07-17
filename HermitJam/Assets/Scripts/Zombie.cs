using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Zombie : MonoBehaviour
{
    
    private Animator m_Animator;
    private const string DeadAnimatorState = "Dead";
    // Start is called before the first frame update
    void Start()
    {
        if(m_Animator == null)
         m_Animator = GetComponentInChildren<Animator>() ?? GetComponent<Animator>();
    }

    public void Die()
    {
        if (m_Animator)
        {
            m_Animator.SetBool(DeadAnimatorState,true);
           
        }

        BoxCollider2D box = GetComponentInChildren<BoxCollider2D>() ?? GetComponent<BoxCollider2D>();
            if(box != null) box.enabled = false;
    }

    public void ResetState()
    {
        if (m_Animator)
        {
            m_Animator.SetBool(DeadAnimatorState, false);
        }
        BoxCollider2D box = GetComponentInChildren<BoxCollider2D>() ?? GetComponent<BoxCollider2D>();
        if(box != null) box.enabled = true;
    }
}
