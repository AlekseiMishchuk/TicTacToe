using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class Cell : MonoBehaviour
{
    [SerializeField] private Sprite _crossSprite;
    [SerializeField] private Sprite _circleSprite;
    [SerializeField] private SpriteRenderer _highlight;
    [SerializeField] private SpriteRenderer _background;
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
        Debug.Log("Cell highlighted");
        var color = _highlight.color;
        color.a = 1;
        _highlight.color = color;
    }

    private void OnMouseDown()
    {
        if (Symbol is SymbolType.Circle or SymbolType.Cross)
        {
            return;
        }
        
        SetSymbol(GameManager.ActivePlayer.Symbol);
        _background.color = Color.gray;
        EventManager.Invoke(Event.MoveMade);
    }

    private IEnumerable ClickAnimation()
    {
        yield return null;
    }
}

public enum SymbolType
{
    Empty,
    Cross,
    Circle
}