using System.Collections;
using System.Collections.Generic;
using System.Linq;
using HermitJam;
using UnityEngine;

namespace HermitJam
{
    [RequireComponent(typeof(Collider2D))]
    public class Obstacle : MonoBehaviour
    {
        public ObstacleType ObstacleType { get; private set; }
        public PlatformPosition PlatformPosition { get; private set; }
    
        public Collider2D Collider { get; private set; }

        

        // Start is called before the first frame update
        void Start()
        {
            if (Collider == null) Collider = GetComponent<Collider2D>();
        }

        public void SetPlatform(Platform platform, int spawnPositionIndex = 0) // -1 for random spawnPosition, 0 for default middle
        {
            transform.SetParent(platform.transform);
            transform.position = spawnPositionIndex == -1
                ? platform.ObstacleSpawnPositions.GetRandom().position
                : platform.ObstacleSpawnPositions.ElementAtOrDefault(spawnPositionIndex).position;
            
            SetPlatformPosition(platform.PlatformPosition);
            
        }
        
        public void SetPlatformPosition(PlatformPosition platformPosition)
        {
            PlatformPosition = platformPosition;
            if (platformPosition == PlatformPosition.Ceiling)
            {
                foreach (var renderer in GetComponentsInChildren<SpriteRenderer>())
                {
                    if (renderer != null)
                    {
                        if(ObstacleType != ObstacleType.Slide && ObstacleType != ObstacleType.None)
                            renderer.flipY = true;
                        
                    }
                }
                //todo fix the offset position of obstacles
                transform.localPosition = new Vector3(transform.localPosition.x, transform.localPosition.y * -3f, transform.localPosition.z);
            }
        }

        // Update is called once per frame
        void Update()
        {
        
        }
    }
}

