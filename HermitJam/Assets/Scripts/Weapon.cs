using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using HermitJam;
using UnityEngine;
using Zenject;
using Random = UnityEngine.Random;

public class Weapon : MonoBehaviour
{
    private GameObject _bulletPrefab;
    [SerializeField] private int _maxPoolSize = 10;
    private EntitiesDatabase _entitiesDatabase;
    private DiContainer _diContainer;

    public Pool<Bullet> BulletPool { get; private set; }
    
    [Inject]
    public void Construct (EntitiesDatabase entitiesDatabase, DiContainer diContainer)
    {
        _entitiesDatabase = entitiesDatabase;
        _diContainer = diContainer;
    }
    
    private void Awake()
    {
        _bulletPrefab = _entitiesDatabase.DefaultBullet;
    }

    // Start is called before the first frame update
    void Start()
    {
        BulletPool = new Pool<Bullet>(CreateBullet, OnTakeFromPool, OnReturnedToPool, OnDestroyPoolObject,
            _maxPoolSize);
    }

    //called within the Pool class when no available objects to reuse
    private Bullet CreateBullet()
    {
        Bullet bullet = _diContainer.InstantiatePrefab(_bulletPrefab).GetComponent<Bullet>();

        if (bullet)
        {
            bullet.OnDeath += OnBulletDeath;
            bullet.gameObject.SetActive(false);
        }
        return bullet;
    }

    public void Shoot()
    {
       SpawnBullet();
    }
    
    public Bullet SpawnBullet()
    {
        return BulletPool.Get();
    }

    public void ReleaseBullet(Bullet bullet)
    {
        if(bullet == null) return;

        BulletPool.Release(bullet);
        
    }

    private void OnTakeFromPool(Bullet bullet)
    {
        if (bullet == null) return;
            bullet.ResetAnimatorState();
            bullet.gameObject.SetActive(true);

            bullet.transform.position = transform.position;
    }
    
    private void OnReturnedToPool(Bullet bullet) => bullet?.gameObject.SetActive(false);
    private void OnDestroyPoolObject(Bullet bullet) => GameObject.Destroy(bullet);

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnBulletDeath(Bullet bullet)
    {
        ReleaseBullet(bullet);
    }
}
