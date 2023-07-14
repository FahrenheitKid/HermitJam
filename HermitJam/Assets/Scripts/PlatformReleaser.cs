using System;
using System.Collections;
using System.Collections.Generic;
using HermitJam;
using UnityEngine;
using Zenject;
using Random = System.Random;

public class PlatformReleaser : MonoBehaviour
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

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Platform"))
        {
            Platform platform = collision.gameObject.GetComponent<Platform>();
            if (platform != null)
            {
                if (platform.PlatformPosition == PlatformPosition.Floor)
                {
                    _floorPool.ReleasePlatform(platform);
                    
                    // make smart difficulty later
                    bool hazard = UnityEngine.Random.Range(0, 2) == 0;
                    bool hazardOnFloor = UnityEngine.Random.Range(0, 2)  == 0;
                    bool acidHazard = UnityEngine.Random.Range(0, 2) == 0;
                    bool obstacle = UnityEngine.Random.Range(0, 5) == 0;
                    bool obstacleOnHazard = hazard && acidHazard && UnityEngine.Random.Range(0, 5) == 0;
                    
                        PlatformType hazardType = acidHazard ? PlatformType.Acid : PlatformType.Spike;
                        _floorPool.SpawnPlatform(hazardOnFloor && hazard ? hazardType : PlatformType.Platform);
                        _ceilingPool.SpawnPlatform(!hazardOnFloor && hazard ? hazardType : PlatformType.Platform);
                    
                }
                else
                {
                    _ceilingPool.ReleasePlatform(platform);
                }
            }
        }
    }
}
