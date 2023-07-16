using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using HermitJam;
using UnityEngine;
using Zenject;
using Random = UnityEngine.Random;

public class ObstaclePool : MonoBehaviour
{
    private GameObject _slidePrefab;
    private List<GameObject> _zombiePrefabs;
    [SerializeField] private int _maxSlidePoolSize = 10;
    [SerializeField] private int _maxZombiePoolSize = 15;
    private EntitiesDatabase _entitiesDatabase;
    private DiContainer _diContainer;

    public Pool<Obstacle> ZombiePool { get; private set; }
    public Pool<Obstacle> SlidePool { get; private set; }

    [Inject]
    public void Construct (EntitiesDatabase entitiesDatabase, DiContainer diContainer)
    {
        _entitiesDatabase = entitiesDatabase;
        _diContainer = diContainer;
    }
    
    /*
    public ObstaclePool( EntitiesDatabase entitiesDatabase)
    {
        _entitiesDatabase = entitiesDatabase;
        Awake();
        Start();
    }*/

    private void Awake()
    {
        _slidePrefab = _entitiesDatabase.GetObstacleData(ObstacleType.Slide).prefab;
        _zombiePrefabs = _entitiesDatabase.GetAllObstacleData(ObstacleType.Zombie).Select(zombie => zombie.prefab).ToList();
    }

    // Start is called before the first frame update
    void Start()
    {
        ZombiePool = new Pool<Obstacle>(CreateZombie, OnTakeFromPool, OnReturnedToPool, OnDestroyPoolObject,
            _maxZombiePoolSize);
        SlidePool = new Pool<Obstacle>(CreateSlide, OnTakeFromPool, OnReturnedToPool, OnDestroyPoolObject,
            _maxSlidePoolSize);
    }
    
    private Obstacle CreateZombie()
    {
        return CreateObstacle(ObstacleType.Zombie);
    }
    private Obstacle CreateZombie(bool male)
    {
        int maleIndex = male ? 0 : 1;
        return CreateObstacle(ObstacleType.Zombie, maleIndex);
    }
    
    private Obstacle CreateSlide()
    {
        return CreateObstacle(ObstacleType.Slide);
    }
    
     //called within the Pool class when no available objects to reuse
    private Obstacle CreateObstacle(ObstacleType obstacleType, int prefabIndex = -1)
    {
        Obstacle obstacle;
        switch (obstacleType)
        {
                
            case ObstacleType.Slide:
                obstacle = _diContainer.InstantiatePrefab(_slidePrefab).GetComponent<Obstacle>();
                break;
            case ObstacleType.Zombie:
                obstacle = _diContainer.InstantiatePrefab(prefabIndex <= -1 ? _zombiePrefabs.ElementAtOrDefault(Random.Range(0, _zombiePrefabs.Count)): _zombiePrefabs.ElementAtOrDefault(prefabIndex)).GetComponent<Obstacle>();
                break;
            default:
                obstacle = _diContainer.InstantiatePrefab(prefabIndex <= -1 ? _zombiePrefabs.ElementAtOrDefault(Random.Range(0, _zombiePrefabs.Count)) : _zombiePrefabs.ElementAtOrDefault(prefabIndex)).GetComponent<Obstacle>();
                break;
        }

        if (obstacle)
        {
            obstacle.gameObject.SetActive(false);
        }
        return obstacle;
    }
    
    public Obstacle SpawnObstacle(ObstacleType obstacleType)
    {
        Obstacle obstacle;
        switch (obstacleType)
        {
            case ObstacleType.Slide:
                obstacle = SlidePool.Get();
                break;
            case ObstacleType.Zombie:
                obstacle = ZombiePool.Get();
                break;
            case ObstacleType.None:
                return null;
                break;
            default:
                obstacle = ZombiePool.Get();
                break;
        }

        return obstacle;

    }

    public void ReleaseObstacle(Obstacle obstacle)
    {
        if(obstacle == null) return;

        switch(obstacle.ObstacleType)
        {
            case ObstacleType.Zombie:
                ZombiePool?.Release(obstacle);
                break;
            case ObstacleType.Slide:
                SlidePool?.Release(obstacle);
                break;
        }
        
    }

    private void OnTakeFromPool(Obstacle obstacle)
    {
        if (obstacle == null) return;
            obstacle.gameObject.SetActive(true);
    }
    
    private void OnReturnedToPool(Obstacle obstacle) => obstacle?.gameObject.SetActive(false);
    private void OnDestroyPoolObject(Obstacle obstacle) => GameObject.Destroy(obstacle);

    // Update is called once per frame
    void Update()
    {
        
    }
}
