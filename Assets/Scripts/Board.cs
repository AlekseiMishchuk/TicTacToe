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
        var index = 0;
        for (var i = 0; i < _cells.GetLength(0); i++)
        {
            for (var j = 0; j < _cells.GetLength(1); j++)
            {
                _cells[i, j] = _cellList[index];
                index++;
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
        for (var i = 0; i < BoardSize; i++)
        {
            var isRowMatching = true;
            var isColumnMatching = true;
            var winCombination = new List<Cell>();
            
            for (var j = 0; j < BoardSize; j++)
            {
                isRowMatching = isRowMatching && (_cells[i, j].Symbol == symbol);
                isColumnMatching = isColumnMatching && (_cells[j, i].Symbol == symbol);

                if (isRowMatching)
                {
                    winCombination.Add(_cells[i,j]);
                }
                else if (isColumnMatching)
                {
                    winCombination.Add(_cells[j,i]);
                }
            }
            
            if (isRowMatching)
            {
                _finalWinCombination = winCombination;
                return true;
            }
            if (isColumnMatching)
            {
                _finalWinCombination = winCombination;
                return true;
            }
        }
        return false;
    }

    private bool CheckDiagonal(SymbolType symbol)
    {
        var isPrimaryMatching = true;
        var isCounterMatching = true;
        var primaryCombination = new List<Cell>();
        var counterCombination = new List<Cell>();

        for (var i = 0; i < BoardSize; i++)
        {
            var primaryCell = _cells[i, i];
            var counterIndex = BoardSize - i - 1;
            var counterCell = _cells[i, counterIndex];

            isPrimaryMatching = primaryCell.Symbol == symbol && isPrimaryMatching;
            isCounterMatching = counterCell.Symbol == symbol && isCounterMatching;
            
            if (isPrimaryMatching)
            {
                primaryCombination.Add(primaryCell);
            }
            if (isCounterMatching)
            {
                counterCombination.Add(counterCell);
            }

            if (!isPrimaryMatching && !isCounterMatching)
            {
                return false;
            }
        }

        _finalWinCombination = isPrimaryMatching ? primaryCombination : counterCombination;
        return true;
    }

    
    public void HighlightWinCombination()
    {
        foreach (var cell in _finalWinCombination)
        {
            cell.Highlight();
        }
    }
}