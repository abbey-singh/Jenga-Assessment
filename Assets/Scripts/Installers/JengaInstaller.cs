using DataAccessors;
using Managers;
using UnityEngine;
using Views.Jenga.Factory;
using Zenject;

public class JengaInstaller : MonoInstaller
{
    public override void InstallBindings()
    {
        Container.Bind<NetworkingOperations>().AsSingle();
        Container.Bind<UrlHolder>().AsSingle();
        Container.BindInterfacesAndSelfTo<StacksDataAccessor>().AsSingle().NonLazy();

        Container.BindInterfacesAndSelfTo<JengaViewFactory>().AsSingle();
        Container.BindInterfacesAndSelfTo<JengaManager>().AsSingle();
    }
}