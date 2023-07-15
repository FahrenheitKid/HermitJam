using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using HermitJam;
using UnityEngine;
using Zenject;
using Random = UnityEngine.Random;

public class PlatformReleaser : MonoBehaviour
{
    [Inject(Id = "FloorPool")]
    [SerializeField] private PlatformPool _floorPool;
    [Inject(Id = "CeilingPool")]
    [SerializeField] private PlatformPool _ceilingPool;
    public event Action<Platform> OnPlatformRelease;

    private StageManager _stageManager;
    
    [Inject]
    public void Construct(StageManager stageManager)
    {
        _stageManager = stageManager;
    }
    
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
                    //we only call the floor one because both floor and ceiling triggers at the same time, so we avoid an useless additional trigger
                    OnPlatformRelease?.Invoke(platform);
                    _floorPool.ReleasePlatform(platform);
                    
                    // make smart difficulty later
                    bool hazard = Random.Range(0, 2) == 0;
                    bool hazardOnFloor = Random.Range(0, 2)  == 0;
                    bool acidHazard = Random.Range(0, 2) == 0;
                    bool obstacle = Random.Range(0, 2) == 0;
                    bool obstacleOnFloor = Random.Range(0, 2)  == 0;
                    bool obstacleOnHazard = hazard && acidHazard && Random.Range(0, 5) == 0;
                    
                    
                        PlatformType hazardType = acidHazard ? PlatformType.Acid : PlatformType.Spike;
                        ObstacleType obstacleType = obstacle ? (ObstacleType) Random.Range(1,Enum.GetNames(typeof(ObstacleType)).Length) : ObstacleType.None;
                        
                        _floorPool.SpawnPlatform(hazardOnFloor && hazard ? hazardType : PlatformType.Platform, 
                            obstacleOnFloor && obstacle? obstacleType : ObstacleType.None);
                        _ceilingPool.SpawnPlatform(!hazardOnFloor && hazard ? hazardType : PlatformType.Platform, 
                            !obstacleOnFloor && obstacle? obstacleType : ObstacleType.None);
                    
                }
                else
                {
                    _ceilingPool.ReleasePlatform(platform);
                }
            }
        }
    }
}
