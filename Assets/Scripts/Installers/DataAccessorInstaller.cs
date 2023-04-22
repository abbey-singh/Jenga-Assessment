using DataAccessors;
using UnityEngine;
using Zenject;

public class DataAccessorInstaller : MonoInstaller
{
    public override void InstallBindings()
    {
        Container.Bind<NetworkingOperations>().AsSingle();
        Container.Bind<UrlHolder>().AsSingle();
        Container.BindInterfacesAndSelfTo<StackDataAccessor>().AsSingle().NonLazy();
    }
}