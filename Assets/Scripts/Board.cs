using System;
using System.Collections.Generic;
using Enums;
using Interfaces;
using UnityEngine;
using Zenject;

public class Board : MonoBehaviour, IBoard
{
    private List<Cell> _finalWinCombination;

    public Cell[,] Cells { get; private set; }

    private const int BoardSize = 3;

    [Inject]
    public void Construct(Cell[] cells)
    {
        Cells = new Cell[BoardSize, BoardSize];
        var index = 0;
        for (var i = 0; i < Cells.GetLength(0); i++)
        {
            for (var j = 0; j < Cells.GetLength(1); j++)
            {
                Cells[i, j] = cells[index];
                index++;
            }
        }  
    }
    
    public void Clear()
    {
        foreach (var cell in Cells)
        {
            cell.Clear();
        }
        _finalWinCombination?.Clear();
    }

    public MoveResult CheckMoveResult(SymbolType symbol)
    {
        var haveEmptyCells = false;
        foreach (var cell in Cells)
        {
            if (cell.Symbol == SymbolType.Empty)
            {
                haveEmptyCells = true;
                break;
            }
        }
        
        if (CheckLines(symbol) || CheckDiagonal(symbol))
        {
            return symbol == SymbolType.Cross ? MoveResult.CrossesWin : MoveResult.CirclesWin;
        }
        
        return !haveEmptyCells ? MoveResult.Draw : MoveResult.GameContinues;
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
                isRowMatching = isRowMatching && (Cells[i, j].Symbol == symbol);
                isColumnMatching = isColumnMatching && (Cells[j, i].Symbol == symbol);

                if (isRowMatching)
                {
                    winCombination.Add(Cells[i,j]);
                }
                else if (isColumnMatching)
                {
                    winCombination.Add(Cells[j,i]);
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
            var primaryCell = Cells[i, i];
            var counterIndex = BoardSize - i - 1;
            var counterCell = Cells[i, counterIndex];

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