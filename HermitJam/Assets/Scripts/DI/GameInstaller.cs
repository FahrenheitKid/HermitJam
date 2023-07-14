using UnityEngine;
using UnityEngine.InputSystem;
using Zenject;

public class GameInstaller : MonoInstaller
{
    [SerializeField] private PlatformPool _floorPool;
    [SerializeField] private PlatformPool _ceilingPool;
    public override void InstallBindings()
    {
        Container.Bind<StageManager>().FromNewComponentOnNewGameObject().WithGameObjectName("StageManager").AsSingle().NonLazy();
        Container.Bind<PlatformPool>().WithId("FloorPool").FromInstance(_floorPool).AsCached().NonLazy();
        Container.Bind<PlatformPool>().WithId("CeilingPool").FromInstance(_ceilingPool).AsCached().NonLazy();
        Container.Bind<TouchAction>().AsSingle().NonLazy();

    }
}