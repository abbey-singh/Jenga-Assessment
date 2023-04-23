using Controllers;
using DataAccessors;
using Helpers;
using Managers;
using Signals;
using System;
using UnityEngine;
using Views.Jenga.Factory;
using Zenject;

public class JengaInstaller : MonoInstaller
{
    [SerializeField] private GameObject _blockControllerPrefab;

    public override void InstallBindings()
    {
        Container.Bind<NetworkingOperations>().AsSingle();
        Container.Bind<UrlHolder>().AsSingle();
        Container.BindInterfacesAndSelfTo<StacksDataAccessor>().AsSingle().NonLazy();

        Container.BindInterfacesAndSelfTo<JengaViewFactory>().AsSingle();
        Container.BindInterfacesAndSelfTo<JengaManager>().AsSingle();

        Container.BindInterfacesAndSelfTo<BlockMaterialsHelper>().AsSingle();
        Container.Bind<BlockController>().AsTransient();

        Container.BindFactory<BlockController, BlockController.Factory>().FromComponentInNewPrefab(_blockControllerPrefab);
        
        Container.Bind<InputController>().AsSingle();
        Container.Bind<CameraController>().AsSingle();

        InstallSignals();
    }

    private void InstallSignals()
    {
        SignalBusInstaller.Install(Container);

        Container.DeclareSignal<BlockClickedSignal>();
    }
}