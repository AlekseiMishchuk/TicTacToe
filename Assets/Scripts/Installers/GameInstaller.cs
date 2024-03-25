using Interfaces;
using Signals;
using UnityEngine;
using Zenject;

namespace Installers
{
    public class GameInstaller : MonoInstaller
    {
        [SerializeField] private UIService _uiService;
        public override void InstallBindings()
        {
            InstallServices();
            InstallSignals();
            Container.Bind<Player>().AsTransient();
        }

        private void InstallServices()
        {
            Container.BindInterfacesAndSelfTo<GameCoordinator>().AsSingle().NonLazy();
            Container.BindInterfacesAndSelfTo<UIService>().FromInstance(_uiService).AsSingle();
            
            Container.Bind<IDataStorage>().To<PlayerPrefsWrapper>().AsSingle();
            Container.BindInterfacesAndSelfTo<DataStorageService>().AsSingle();
            
            Container.BindInterfacesAndSelfTo<SaveLoadService>().AsSingle();
            Container.Bind<SceneService>().AsSingle();    
        }

        private void InstallSignals()
        {
            SignalBusInstaller.Install(Container);
            Container.DeclareSignal<MoveMadeSignal>();
            Container.DeclareSignal<StartNewGameSignal>();
            Container.DeclareSignal<GameOverSignal>();
        }
    }
}