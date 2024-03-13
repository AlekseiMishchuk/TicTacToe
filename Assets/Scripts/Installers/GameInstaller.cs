using UnityEngine;
using Zenject;

namespace Installers
{
    public class GameInstaller : MonoInstaller
    {
        [SerializeField] private UIService _uiService;
        public override void InstallBindings()
        {
            Container.BindInterfacesAndSelfTo<EventService>().AsSingle().NonLazy();
            Container.BindInterfacesAndSelfTo<GameCoordinator>().AsSingle().NonLazy();
            Container.BindInterfacesAndSelfTo<UIService>().FromInstance(_uiService).AsSingle();
            Container.BindInterfacesAndSelfTo<SaveLoadService>().AsSingle();
            Container.Bind<SceneService>().AsSingle();
            
            Container.Bind<Player>().AsTransient();
        }
    }
}