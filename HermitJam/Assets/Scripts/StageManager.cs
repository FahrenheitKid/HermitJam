using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class StageManager : MonoBehaviour
{
    [Inject(Id = "FloorPool")]
    [SerializeField] private PlatformPool _floorPool;
    [Inject(Id = "CeilingPool")]
    [SerializeField] private PlatformPool _ceilingPool;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
