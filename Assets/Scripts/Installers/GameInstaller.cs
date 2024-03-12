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
            Container.BindInterfacesAndSelfTo<GameManager>().AsSingle().NonLazy();
            Container.BindInterfacesAndSelfTo<UIService>().FromInstance(_uiService).AsSingle();
            Container.Bind<SceneService>().AsSingle();
            Container.BindInterfacesAndSelfTo<SaveLoadService>().AsSingle();
            Container.Bind<Player>().AsTransient();
        }
    }
}