using System.Linq;
using UnityEngine;

/// <summary>
/// Abstract class for making reload-proof singletons out of ScriptableObjects
/// Returns the asset created on the editor, or null if there is none
/// Based on https://www.youtube.com/watch?v=VBA1QCoEAX4
/// </summary>
/// <typeparam name="T">Singleton type</typeparam>

public abstract class SingletonScriptableObject<T> : ScriptableObject where T : ScriptableObject
{
    [SerializeField]
    static T _instance = null;

    public static T Instance
    {
        get
        {
            if (!_instance || _instance == null)
                _instance = Resources.FindObjectsOfTypeAll<T>().FirstOrDefault();

                if(_instance == null)
                {
                    //throw new System.Exception($"Not found Singleton Scriptable Object using old method, gonna try to search in resources folder");
                    Debug.LogWarning("Not found Singleton Scriptable Object using old method, gonna try to search in resources folder");
                T[] assets = Resources.LoadAll<T>("");
                if (assets == null || assets.Length < 1)
                {
                    throw new System.Exception($"Not found Singleton Scriptable Object of type: {typeof(T).ToString()}");
                }
                else if (assets.Length > 1)
                {
                    throw new System.Exception($"More than 1 instance of Singleton Scriptable Object of type: {typeof(T).ToString()} found");
                }
                    _instance = assets[0];
                }
            return _instance;
        }
    }

}