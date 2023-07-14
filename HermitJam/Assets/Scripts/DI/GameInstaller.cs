using UnityEngine;
using UnityEngine.InputSystem;
using Zenject;
using HermitJam;

public class GameInstaller : MonoInstaller
{
    [SerializeField] private PlatformPool _floorPool;
    [SerializeField] private PlatformPool _ceilingPool;
    [SerializeField] private PlatformReleaser _PlatformReleaser;
    
    [InjectOptional]
    Difficulty m_difficulty  = Difficulty.Medium;
    
    public override void InstallBindings()
    {
        Container.BindInstance(m_difficulty).WhenInjectedInto<StageManager>();
        Container.Bind<StageManager>().FromNewComponentOnNewGameObject().WithGameObjectName("StageManager").AsSingle().NonLazy();
        if(_PlatformReleaser) Container.Bind<PlatformReleaser>().FromInstance(_PlatformReleaser).AsSingle();
        else Container.Bind<PlatformReleaser>().FromComponentInHierarchy().AsSingle();
        Container.Bind<PlatformPool>().WithId("FloorPool").FromInstance(_floorPool).AsCached().NonLazy();
        Container.Bind<PlatformPool>().WithId("CeilingPool").FromInstance(_ceilingPool).AsCached().NonLazy();
        Container.Bind<TouchAction>().AsSingle().NonLazy();

    }
}