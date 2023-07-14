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

    [SerializeField] private float _timeScaleTickRate = 5f;
    [SerializeField] private float _timeScaleIncrease = 0.025f; 
    
    // Start is called before the first frame update
    void Start()
    {
        Timer.Register(_timeScaleTickRate, (() => { Time.timeScale += _timeScaleIncrease; Debug.Log("Current TimeScale: " + Time.timeScale); }), null, true, true);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
