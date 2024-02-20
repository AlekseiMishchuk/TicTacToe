using Enums;

namespace Interfaces
{
    public interface IBoard
    {
        Cell[,] Cells { get; }
        void Clear();
        MoveResult CheckMoveResult(SymbolType symbol);
        void HighlightWinCombination();
    }
}