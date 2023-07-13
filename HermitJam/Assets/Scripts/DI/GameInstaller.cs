using UnityEngine;
using Zenject;

public class GameInstaller : MonoInstaller
{
    [SerializeField] private PlatformPool _floorPool;
    [SerializeField] private PlatformPool _ceilingPool;
    public override void InstallBindings()
    {
        Container.Bind<StageManager>().FromNewComponentOnNewGameObject().WithGameObjectName("StageManager").AsSingle();
        Container.Bind<PlatformPool>().WithId("FloorPool").FromInstance(_floorPool);
        Container.Bind<PlatformPool>().WithId("CeilingPool").FromInstance(_ceilingPool);

    }
}