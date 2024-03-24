using Enums;

public class Player
{
    public SymbolType Symbol { get; private set;}

    public void Initialize(SymbolType symbol)
    {
        Symbol = symbol;
    }
}
