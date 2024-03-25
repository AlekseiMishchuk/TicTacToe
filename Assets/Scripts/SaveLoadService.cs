using System;
using System.Text;
using Enums;
using Interfaces;
using UnityEngine;
using Zenject;

public class SaveLoadService
{
    private readonly DataStorageService _dataStorageService;
    
    private const string BoardStateKey = "boardState";
    private const string LastPlayerKey = "lastPlayerSymbol";

    [Inject]
    public SaveLoadService(DataStorageService dataStorageService)
    {
        _dataStorageService = dataStorageService;
    }
    public void SaveBoardState(IBoard board, SymbolType playerSymbol)
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
        
        _dataStorageService.SetString(BoardStateKey, boardAsString.ToString());
        _dataStorageService.SetInt(LastPlayerKey, (int)playerSymbol);
    }

    public SymbolType LoadBoardState(IBoard board)
    {
        var boardAsString = _dataStorageService.GetString(BoardStateKey);
        
        if (string.IsNullOrEmpty(boardAsString))
        {
            Debug.LogError($"{BoardStateKey} has no value");
            throw new ArgumentNullException($"_dataStorageService {BoardStateKey} has no value");
        }
        
        var boardRows = boardAsString.Split(';');
        for (var i = 0; i < boardRows.Length; i++)
        {
            var rowSymbols = boardRows[i].Split(',');
            for (var j = 0; j < rowSymbols.Length; j++)
            {
                int.TryParse(rowSymbols[j], out var symbolInt);
                if (Enum.IsDefined(typeof(SymbolType), symbolInt))
                {
                    var symbolEnum = (SymbolType)symbolInt;
                    board.Cells[i,j].SetSymbol(symbolEnum);
                }
                else
                {
                    throw new AggregateException("Wrong value in board state string, while loading");
                }
            }
        }
        
        return (SymbolType)_dataStorageService.GetInt(LastPlayerKey);
    }
}
