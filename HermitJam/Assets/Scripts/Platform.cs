using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Zenject;

namespace HermitJam
{
    public class Platform : MonoBehaviour
    {
        [field: SerializeField] public PlatformType PlatformType { get; private set; }
        [field: SerializeField] public PlatformPosition PlatformPosition { get; private set; }
        [field: SerializeField] public List<Transform> ObstacleSpawnPositions { get; private set; }

        [SerializeField] private float _speed;
        [SerializeField] private float _currentSpeed = 1f;
        [field: SerializeField] public List<Obstacle> Obstacles { get; private set; }
        
        private ObstaclePool _obstaclePool;
        
        
        public bool IsHazard => PlatformType != PlatformType.Platform;
        private Rigidbody2D _rigidbody2D;
        
        [Inject]
        void Construct(ObstaclePool obstaclePool)
        {
            _obstaclePool = obstaclePool;
        }
        
            // Start is called before the first frame update
        void Start()
        {
            if(_rigidbody2D == null) _rigidbody2D = GetComponent<Rigidbody2D>();
            _rigidbody2D.velocity = new Vector2(-1 * _currentSpeed, 0);
        }

        public void SetPlatformPosition(PlatformPosition position)
        {
            PlatformPosition = position;
        }

        private void OnEnable()
        {
            if(_rigidbody2D == null) _rigidbody2D = GetComponent<Rigidbody2D>();
            _rigidbody2D.velocity = new Vector2(-1 * _currentSpeed, 0);
        }

        public void AddObstacle(ObstacleType obstacleType)
        {
            //cannot have two slides in the same platform
            if (obstacleType == ObstacleType.Slide && HasObstacle(ObstacleType.Slide))
            {
                return;
            }
            
            Obstacle obstacle = _obstaclePool.SpawnObstacle(obstacleType);
            Obstacles.Add(obstacle);
            obstacle.SetPlatform(this);
            
        }

        public bool HasObstacle(ObstacleType obstacleType)
        {
            return Obstacles.Any(x => x.ObstacleType == obstacleType);
        }

        public void ReleaseObstacles()
        {
            if (Obstacles?.Any() == true)
            {
                Obstacles.ForEach(obstacle => {_obstaclePool.ReleaseObstacle(obstacle);});
            }
            Obstacles?.Clear();
        }
    }
}

