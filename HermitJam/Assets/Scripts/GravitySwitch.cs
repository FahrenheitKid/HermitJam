using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GravitySwitch : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            print("colidi player");
            Player player = collision.GetComponent<Player>() ?? collision.GetComponentInParent<Player>();
            if (player != null)
            {
                player.SwitchGravity();
                
            }
        }

    }
}
