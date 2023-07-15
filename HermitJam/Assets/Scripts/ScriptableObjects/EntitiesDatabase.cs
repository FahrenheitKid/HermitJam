using System;
using System.Collections;
using System.Collections.Generic;
using HermitJam;
using UnityEngine;
using UnityEngine.Serialization;

[Serializable, CreateAssetMenu(fileName = "EntitiesDatabase", menuName = "ScriptableObject/Database/Entities")]
public class EntitiesDatabase : ScriptableObject
{
    
    [Serializable]
    public struct PlatformData
    {
#if UNITY_EDITOR_WIN
        [HideInInspector] public string name;
#endif
        public PlatformType platformType;
        public GameObject prefab;

#if UNITY_EDITOR_WIN
        public void SetName(string p_name)
        {
            name = p_name;
        }
#endif
    }
    
    [Serializable]
    public struct ObstacleData
    {
#if UNITY_EDITOR_WIN
        [HideInInspector] public string name;
#endif
        public ObstacleType obstacleType;
        public GameObject prefab;

#if UNITY_EDITOR_WIN
        public void SetName(string p_name)
        {
            name = p_name;
        }
#endif
    }
    
    [FormerlySerializedAs("_entityData")] [SerializeField] private PlatformData[] _platformData;
    [SerializeField] private ObstacleData[] _obstacleData;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    public PlatformData GetPlatformData(PlatformType platformType)
    {
        return Array.Find(_platformData, d => d.platformType == platformType);
    }
    
    public ObstacleData GetObstacleData(ObstacleType obstacleType)
    {
        return Array.Find(_obstacleData, d => d.obstacleType == obstacleType);
    }
    
    public ObstacleData[] GetAllObstacleData(ObstacleType obstacleType)
    {
        return Array.FindAll(_obstacleData, d => d.obstacleType == obstacleType);
    }
}
