using System;
using Enums;
using Interfaces;
using UnityEngine;
using Zenject;

public class GameCoordinator : IInitializable
{
    private readonly EventService _eventService;
    private readonly SaveLoadService _saveLoadService;
    private readonly SceneService _sceneService;
    private readonly IBoard _board;
    private readonly Player _player1;
    private readonly Player _player2;

    private Player _activePlayer; 
    
    private const string HasSavedData = "saved";

    [Inject]
    public GameCoordinator(
        Player player1, 
        Player player2, 
        IBoard board, 
        EventService eventService, 
        SaveLoadService saveLoadService,
        SceneService sceneService)
    {
        _board = board;
        _player1 = player1;
        _player2 = player2;
        _eventService = eventService;
        _saveLoadService = saveLoadService;
        _sceneService = sceneService;
    }
    
    public void Initialize()
    {
        _player1?.Initialization(SymbolType.Cross);
        _player2?.Initialization(SymbolType.Circle);
        
        if (PlayerPrefs.HasKey(HasSavedData))
        {
            try
            {
                var lastPlayerSymbol = _saveLoadService.LoadBoardState(_board);
                _activePlayer = lastPlayerSymbol == _player1?.Symbol ? _player1 : _player2;
            }
            catch (ArgumentNullException e)
            {
                Debug.LogError(e.Message);
            }
            catch (AggregateException e)
            {
                Debug.LogError(e.Message);
            }
        }
        else
        {
            _activePlayer = _player1;
        }
        
        _eventService.AddListener<Cell>(EventName.MoveMade, SetCellSymbol);
        _eventService.AddListener(EventName.StartNewGame, StartNewGame);
    }

    private void SetCellSymbol(Cell cell)
    {
        cell.SetSymbol(_activePlayer.Symbol);
        CheckGameOver();
    }
 
    private void CheckGameOver()
    {
        var moveResult = _board.CheckMoveResult(_activePlayer.Symbol);
        if (moveResult != MoveResult.GameContinues)
        {
            if(moveResult != MoveResult.Draw)
            {
                _board.HighlightWinCombination();
            }
            _eventService.Invoke(EventName.GameOver);
            _eventService.Invoke(EventName.GameOver, moveResult);
            PlayerPrefs.DeleteKey(HasSavedData);
        }
        else
        {
            ChangePlayer();
            _saveLoadService.SaveBoardState(_board, _activePlayer.Symbol);
            PlayerPrefs.SetInt(HasSavedData, 1);
        }
    }
    
    private void ChangePlayer()
    {
        _activePlayer = _activePlayer == _player1 ? _player2 : _player1;
    }

    private void StartNewGame()
    {
        PlayerPrefs.DeleteAll();
        _sceneService.ReloadScene();
    }

}
