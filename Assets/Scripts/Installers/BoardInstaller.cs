using Interfaces;
using UnityEngine;
using Zenject;

namespace Installers
{
    public class BoardInstaller : MonoInstaller
    {
        [SerializeField] private GameObject _board;
        public override void InstallBindings()
        {
            Container.Bind<IBoard>().To<Board>().FromComponentOn(_board).AsSingle();
            Container.BindInterfacesAndSelfTo<Cell>().FromComponentInHierarchy().AsTransient();
        }
    }
}