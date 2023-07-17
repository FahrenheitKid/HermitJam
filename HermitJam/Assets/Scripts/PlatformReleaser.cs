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
    
    public struct PlatformInfo
    {
        public PlatformType platformType;
        public bool hasObstacle;

        public PlatformInfo(PlatformType platformType, bool hasObstacle)
        {
            this.platformType = platformType;
            this.hasObstacle = hasObstacle;
        }
    }
    [Inject(Id = "FloorPool")]
    [SerializeField] private PlatformPool _floorPool;
    [Inject(Id = "CeilingPool")]
    [SerializeField] private PlatformPool _ceilingPool;

    private PlatformInfo lastFloorPlatform, lastCeilingPlatform;
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
        lastFloorPlatform = lastCeilingPlatform = new PlatformInfo(PlatformType.Platform, false);
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
                    if (obstacleOnFloor == hazardOnFloor && obstacle && acidHazard == false) acidHazard = true;
                    bool obstacleOnHazard = hazard && acidHazard && (hazardOnFloor == obstacleOnFloor);
                    
                    
                        PlatformType hazardType = acidHazard ? PlatformType.Acid : PlatformType.Spike;
                        ObstacleType obstacleType = obstacle ? (ObstacleType) Random.Range(1,Enum.GetNames(typeof(ObstacleType)).Length) : ObstacleType.None;
                        
                        //here we avoid having two obstacles together (one on floor/ceiling and the next in the opposite floor)
                        if (obstacleType != ObstacleType.None && obstacle)
                        {
                            if ((obstacleOnFloor && lastCeilingPlatform.hasObstacle) ||
                                (!obstacleOnFloor && lastFloorPlatform.hasObstacle) || 
                                (obstacleOnFloor && lastFloorPlatform.hasObstacle) ||
                                (!obstacleOnFloor && lastCeilingPlatform.hasObstacle))
                            {
                                obstacle = false;
                            }
                        }
                        
                        _floorPool.SpawnPlatform(hazardOnFloor && hazard ? hazardType : PlatformType.Platform, 
                            obstacleOnFloor && obstacle ? obstacleType : ObstacleType.None);
                        _ceilingPool.SpawnPlatform(!hazardOnFloor && hazard ? hazardType : PlatformType.Platform, 
                            !obstacleOnFloor && obstacle ? obstacleType : ObstacleType.None);

                        lastFloorPlatform.hasObstacle = obstacleOnFloor && obstacle;
                        lastCeilingPlatform.hasObstacle = !obstacleOnFloor && obstacle;

                }
                else
                {
                    _ceilingPool.ReleasePlatform(platform);
                }
            }
        }
    }
}
