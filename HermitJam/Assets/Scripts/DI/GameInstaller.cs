using UnityEngine;
using UnityEngine.InputSystem;
using Zenject;
using HermitJam;

public class GameInstaller : MonoInstaller
{
    [SerializeField] private PlatformPool _floorPool;
    [SerializeField] private PlatformPool _ceilingPool;
    [SerializeField] private PlatformReleaser _PlatformReleaser;
    [SerializeField] private StageManager _stageManager;

    [InjectOptional]
    Difficulty m_difficulty  = Difficulty.Medium;
    
    public override void InstallBindings()
    {
        Container.BindInstance(m_difficulty).WhenInjectedInto<StageManager>();
        if(_stageManager) Container.Bind<StageManager>().FromInstance(_stageManager).AsSingle().NonLazy();
        Container.Bind<Player>().FromComponentInHierarchy().AsSingle().NonLazy();
        Container.Bind<UIManager>().FromComponentInHierarchy().AsSingle().NonLazy();
        if(_PlatformReleaser) Container.Bind<PlatformReleaser>().FromInstance(_PlatformReleaser).AsSingle();
        else Container.Bind<PlatformReleaser>().FromComponentInHierarchy().AsSingle();
        Container.Bind<PlatformPool>().WithId("FloorPool").FromInstance(_floorPool).AsCached().NonLazy();
        Container.Bind<PlatformPool>().WithId("CeilingPool").FromInstance(_ceilingPool).AsCached().NonLazy();
        Container.Bind<ObstaclePool>().FromNewComponentOnNewGameObject().WithGameObjectName("ObstaclePool").AsSingle().NonLazy();
        Container.Bind<TouchAction>().AsSingle().NonLazy();

    }
}