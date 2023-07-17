using System.Collections;
using System.Collections.Generic;
using HermitJam;
using UnityEngine;

public partial class Player : MonoBehaviour
{
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
                if (obstacle.ObstacleType == ObstacleType.Zombie || (obstacle.ObstacleType == ObstacleType.Slide && !Sliding))
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
}
