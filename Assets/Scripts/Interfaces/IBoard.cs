using Enums;

namespace Interfaces
{
    public interface IBoard
    {
        ICell[,] Cells { get; }

        void Initialize();
        void Clear();
        MoveResult CheckMoveResult(SymbolType symbol);
        void HighlightWinCombination();
    }
}