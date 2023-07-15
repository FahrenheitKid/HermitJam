using UnityEngine;
using Zenject;

public class MainInstaller : MonoInstaller
{
    [SerializeField] private EntitiesDatabase _entitiesDatabase;
    public override void InstallBindings()
    {
        Container.BindInstance(_entitiesDatabase).AsSingle().NonLazy();
    }
}