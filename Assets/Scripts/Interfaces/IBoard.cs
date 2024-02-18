using Enums;

namespace Interfaces
{
    public interface IBoard
    {
        Cell[,] Cells { get; }
        void Initialization();
        MoveResult CheckMoveResult(SymbolType symbol);
    }
}