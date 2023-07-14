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
        //Timer.Register(5f, (() => { Time.timeScale += 0.1f; }), null, true, true);
        Time.timeScale = 2f;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
