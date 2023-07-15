using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using HermitJam;
using UnityEngine;
using Zenject;

public class PlatformPool : MonoBehaviour
{
    [SerializeField] private StageManager _stageManager;
    private EntitiesDatabase _entitiesDatabase;
    private GameObject _platformPrefab;
    private GameObject _acidPrefab;
    private GameObject _spikesPrefab;

    [SerializeField] private PlatformPosition _platformPosition;
    [SerializeField] private float _spawnXPosition = 10f;
    [SerializeField] private bool collectionChecks = true;
    [SerializeField] private int _maxPoolSize;
    public Pool<Platform> PlatformsPool { get; private set; }
    public Pool<Platform> AcidsPool { get; private set; }
    public Pool<Platform> SpikesPool { get; private set; }
    
    public Pool<Obstacle> ObstaclePool { get; private set; }

    [Inject]
    void Construct(StageManager stageManager, EntitiesDatabase entitiesDatabase)
    {
        _stageManager = stageManager;
        _entitiesDatabase = entitiesDatabase;
    }

    private void Awake()
    {
        _platformPrefab = _entitiesDatabase.GetPlatformData(PlatformType.Platform).prefab;
        _acidPrefab = _entitiesDatabase.GetPlatformData(PlatformType.Acid).prefab;
        _spikesPrefab = _entitiesDatabase.GetPlatformData(PlatformType.Spike).prefab;
    }

    // Start is called before the first frame update
    void Start()
    {
        PlatformsPool = new Pool<Platform>(CreateDefaultPlatform, OnTakeFromPool, OnReturnedToPool, OnDestoryPoolObject,
            _maxPoolSize);
        AcidsPool = new Pool<Platform>(CreateAcidPlatform, OnTakeFromPool, OnReturnedToPool, OnDestoryPoolObject,
            _maxPoolSize);
        SpikesPool = new Pool<Platform>(CreateSpikePlatform, OnTakeFromPool, OnReturnedToPool, OnDestoryPoolObject,
            _maxPoolSize);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private Platform CreateDefaultPlatform()
    {
        return CreatePlatform(_platformPosition);
    }
    
    private Platform CreateAcidPlatform()
    {
        return CreatePlatform(_platformPosition, PlatformType.Acid);
    }
    
    private Platform CreateSpikePlatform()
    {
        return CreatePlatform(_platformPosition, PlatformType.Spike);
    }
    
    //called within the Pool class when no available objects to reuse
    private Platform CreatePlatform(PlatformPosition platformPosition, PlatformType platformType = PlatformType.Platform)
    {
        Platform platform;
        switch (platformType)
        {
            case PlatformType.Acid:
                platform = Instantiate(_acidPrefab, transform).GetComponent<Platform>();
                break;
            case PlatformType.Spike:
                platform = Instantiate(_spikesPrefab, transform).GetComponent<Platform>();
                break;
            case PlatformType.Platform:
                platform = Instantiate(_platformPrefab, transform).GetComponent<Platform>();
                break;
            default:
                platform = Instantiate(_platformPrefab, transform).GetComponent<Platform>();
                break;
        }

        if (platform)
        {
            platform.SetPlatformPosition(platformPosition);
            platform.gameObject.SetActive(false);
            if (platformPosition == PlatformPosition.Ceiling)
            {
                foreach (var renderer in platform.GetComponentsInChildren<SpriteRenderer>())
                {
                    if (renderer != null)
                    {
                        if(platformType != PlatformType.Acid)
                            renderer.flipY = true;
                        else
                        {
                            if (renderer.transform.parent.name == "Tiles")
                            {
                                renderer.transform.eulerAngles = new Vector3(0, 0, 180f);
                                renderer.transform.localPosition = new Vector3(renderer.transform.localPosition.x, renderer.transform.localPosition.y * -1f, renderer.transform.localPosition.z);
                                
                            }
                        }
                    }
                }
            }
        }
        return platform;
    }

    private void OnTakeFromPool(Platform platform)
    {
        if (platform == null) return;
        platform.transform.position = new Vector2(_spawnXPosition, platform.transform.position.y);
        platform.gameObject.SetActive(true);
    }

    public void ReleasePlatform(Platform platform)
    {
        if (platform == null) return;
        
        switch (platform.PlatformType)
        {
            case PlatformType.Acid:
                AcidsPool?.Release(platform);
                break;
            case PlatformType.Spike:
                SpikesPool?.Release(platform);
                break;
            case PlatformType.Platform:
                PlatformsPool.Release(platform);
                break;
            default:
                PlatformsPool.Release(platform);
                break;
        }
    }
    
    public void SpawnPlatform(PlatformType platformType = PlatformType.Platform,ObstacleType obstacleType = ObstacleType.None)
    {
        Platform platform;
        switch (platformType)
        {
            case PlatformType.Acid:
                platform = AcidsPool.Get();
                break;
            case PlatformType.Spike:
                platform = SpikesPool.Get();
                break;
            case PlatformType.Platform:
                platform = PlatformsPool.Get();
                break;
            default:
                platform = PlatformsPool.Get();
                break;
        }
        
        //setup obstacle later
        switch (obstacleType)
        {
            case ObstacleType.None:
                
                break;
            case ObstacleType.Zombie:
                //platform.addObstacle(ZombieType);
                break;
            case ObstacleType.Slide:
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(obstacleType), obstacleType, null);
        }
    }

    private void OnReturnedToPool(Platform platform) => platform?.gameObject.SetActive(false);
    private void OnDestoryPoolObject(Platform platform) => Destroy(platform);
}
