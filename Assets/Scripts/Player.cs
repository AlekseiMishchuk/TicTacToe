using UnityEngine;

public class Player : MonoBehaviour
{
    public SymbolType Symbol { get; private set;}

    public void Initialization(SymbolType symbol)
    {
        Symbol = symbol;
    }
}
