using Enums;

public class Player
{
    public SymbolType Symbol { get; private set;}

    public void Initialization(SymbolType symbol)
    {
        Symbol = symbol;
    }
}
