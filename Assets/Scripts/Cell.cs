using System.Collections;
using Enums;
using UnityEngine;

public class Cell : MonoBehaviour
{
    [SerializeField] private Sprite _crossSprite;
    [SerializeField] private Sprite _circleSprite;
    [SerializeField] private SpriteRenderer _background;
    [SerializeField] private Color _highlightColor;
    public SymbolType Symbol { get; private set; }


    public void SetSymbol(SymbolType symbol)
    {
        Symbol = symbol;
        switch (symbol)
        {
            case SymbolType.Circle:
                gameObject.GetComponent<SpriteRenderer>().sprite = _circleSprite;
                break;
            case SymbolType.Cross:
                gameObject.GetComponent<SpriteRenderer>().sprite = _crossSprite;
                break;
            case SymbolType.Empty:
            default:
                gameObject.GetComponent<SpriteRenderer>().sprite = null;
                break;
        }
    }

    public void Clear()
    {
        Symbol = SymbolType.Empty;
        SetSymbol(SymbolType.Empty);
    }

    public void Highlight()
    {
        _background.color = _highlightColor;
    }

    private void OnMouseDown()
    {
        if (Symbol is SymbolType.Circle or SymbolType.Cross)
        {
            return;
        }
        
        SetSymbol(GameManager.ActivePlayer.Symbol);
        EventService.Invoke(EventName.MoveMade);
    }

    private IEnumerable ClickAnimation()
    {
        yield return null;
    }
}