using Enums;

namespace Interfaces
{
    public interface ICell
    {
        SymbolType Symbol { get; }

        void SetSymbol(SymbolType symbol);
        void Clear();
        void Highlight();
        void BlockMouseClick();
    }
}