using System.Collections;
using System.Collections.Generic;
using HermitJam;
using UnityEngine;
using Zenject;

public class BulletReleaser : MonoBehaviour
{
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.transform.CompareTag("Bullet"))
        {
            Bullet bullet = other.transform.GetComponent<Bullet>();
            if (bullet != null)
            {
                bullet.Die(true);
            }
        }
        
    }
}
