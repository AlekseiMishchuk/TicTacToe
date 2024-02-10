using System;
using System.Text;
using Interfaces;
using UnityEngine;

public static class SaveLoadService
{
    private const string BoardStateKey = "boardState"; 
    public static void SaveBoardState(IBoard board)
    {
        var cells = board.Cells; 
        var boardAsString = new StringBuilder();
        
        for (var i = 0; i < cells.GetLength(0); i++)
        {
            var boardRows = new StringBuilder();
            for (var j =0; j < cells.GetLength(1); j++)
            {
                var symbol = (int)cells[i, j].Symbol;
                boardRows.Append(symbol);
                if (j < cells.GetLength(1) - 1)
                {
                    boardRows.Append(',');
                }
            }

            boardAsString.Append(boardRows);
            if (i < cells.GetLength(1) - 1)
            {
                boardAsString.Append(';');
            }
        }
        
        PlayerPrefs.SetString(BoardStateKey, boardAsString.ToString());
    }

    public static void LoadBoardState(IBoard board)
    {
        var boardAsString = PlayerPrefs.GetString(BoardStateKey);
        
        if (boardAsString == null)
        {
            Debug.LogError($"{BoardStateKey} has no value");
            return;
        }
        
        var boardRows = boardAsString.Split(';');
        for (var i = 0; i < boardRows.Length; i++)
        {
            var rowSymbols = boardRows[i].Split(',');
            for (var j = 0; j < rowSymbols.Length; j++)
            {
                int.TryParse(rowSymbols[j], out var symbolInt);
                if (Enum.IsDefined(typeof(SymbolType), rowSymbols[j]))
                {
                    var symbolEnum = (SymbolType)symbolInt;
                    board.Cells[i,j].SetSymbol(symbolEnum);
                }
                else
                {
                    Debug.LogError("Wrong value in board state string, while loading");
                    return;
                }
            }
        }
        PlayerPrefs.DeleteAll();
    }
}
