using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HermitJam
{
    public class Platform : MonoBehaviour
    {
        [field: SerializeField] public PlatformType PlatformType { get; private set; }
        [field: SerializeField] public PlatformPosition PlatformPosition { get; private set; }

        [SerializeField] private float _speed;
        [SerializeField] private float _currentSpeed = 1f;
        public bool IsHazard => PlatformType != PlatformType.Platform;
        private Rigidbody2D _rigidbody2D;
        
            // Start is called before the first frame update
        void Start()
        {
            if(_rigidbody2D == null) _rigidbody2D = GetComponent<Rigidbody2D>();
            _rigidbody2D.velocity = new Vector2(-1 * _currentSpeed, 0);
        }

        // Update is called once per frame
        void FixedUpdate()
        {
            
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
    }
}

