using Interfaces;
using UnityEngine;
using Zenject;

namespace Installers
{
    public class BoardInstaller : MonoInstaller
    {
        [SerializeField] private GameObject _board;
        [SerializeField] private Cell[] _cells;
        public override void InstallBindings()
        {
            Container.Bind<IBoard>().To<Board>().FromComponentOn(_board).AsCached();
            Container.Bind<Cell[]>().FromInstance(_cells).AsCached();
        }
    }
}