using System.Collections;
using System.Collections.Generic;
using HermitJam;
using UnityEngine;

namespace HermitJam
{
    [RequireComponent(typeof(Collider2D))]
    public class Obstacle : MonoBehaviour
    {
        public ObstacleType ObstacleType { get; private set; }
    
        public Collider2D Collider { get; private set; }

        // Start is called before the first frame update
        void Start()
        {
            if (Collider == null) Collider = GetComponent<Collider2D>();
        }

        // Update is called once per frame
        void Update()
        {
        
        }
    }
}

