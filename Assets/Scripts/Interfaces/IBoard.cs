namespace Interfaces
{
    public interface IBoard
    {
        Cell[,] Cells { get; }
        void Initialization();
        bool WinConditions(SymbolType symbol);
    }
}