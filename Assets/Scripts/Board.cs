using System;
using System.Collections.Generic;
using Interfaces;
using UnityEngine;

public class Board : MonoBehaviour, IBoard
{
    [SerializeField] private List<Cell> _cellList;
    
    private Cell[,] _cells = new Cell[BoardSize, BoardSize];
    private List<Cell> _finalWinCombination;
    
    private const int BoardSize = 3;
    
    public Cell[,] Cells => _cells;

    private void Awake()
    {
        var count = 0;
        for (var i = 0; i < _cells.GetLength(0); i++)
        {
            for (var j = 0; j < _cells.GetLength(1); j++)
            {
                _cells[i, j] = _cellList[count];
                count++;
            }
        }
    }

    public void Initialization()
    {
        foreach (var cell in _cells)
        {
            cell.Clear();
        }
        _finalWinCombination?.Clear();
    }

    public bool WinConditions(SymbolType symbol)
    {
        return CheckLines(symbol) || CheckDiagonal(symbol);
    }

    private bool CheckLines(SymbolType symbol)
    {
        var isRowSame = true;
        var isColumnSame = true;
        var rowWinCombination = new List<Cell>();
        var columnWinCombination = new List<Cell>();
        
        for (var i = 0; i < BoardSize; i++)
        {
            for (var j = 0; j < BoardSize; j++)
            {
                isRowSame = isRowSame && (_cells[i, j].Symbol == symbol);
                isColumnSame = isColumnSame && (_cells[j, i].Symbol == symbol);
            }
        }
        
        if (isRowSame)
        {
            _finalWinCombination = rowWinCombination;
            return true;
        }
        else if (isColumnSame)
        {
            _finalWinCombination = columnWinCombination;
            return true;
        }
        else
        {
            return false;
        }
    }

    private bool CheckDiagonal(SymbolType symbol)
    {
        var isPrimaryDiagonalSame = true;
        var isCounterDiagonalSame = true;
        var primaryWinCombination = new List<Cell>();
        var counterWinCombination = new List<Cell>();
        
        for (var i = 0; i < BoardSize; i++)
        {
            var nextSymbol = _cells[i, i].Symbol;
            if (isPrimaryDiagonalSame)
            {
                isPrimaryDiagonalSame = isPrimaryDiagonalSame && (nextSymbol == symbol);
                primaryWinCombination.Add(_cells[i,i]);
            }
            
            var counterIndex = BoardSize - i - 1;
            nextSymbol = _cells[counterIndex, counterIndex].Symbol;
            if (isCounterDiagonalSame)
            {
                isCounterDiagonalSame = isCounterDiagonalSame && (nextSymbol == symbol);
                counterWinCombination.Add(_cells[counterIndex, counterIndex]);    
            }
        }

        if (isPrimaryDiagonalSame)
        {
            _finalWinCombination = primaryWinCombination;
            return true;
        }
        else if (isCounterDiagonalSame)
        {
            _finalWinCombination = counterWinCombination;
            return true;
        }
        else
        {
            return false;
        }
    }

    
    public void HighlightWinCombination()
    {
        foreach (var cell in _finalWinCombination)
        {
            cell.Highlight();
        }
    }
}