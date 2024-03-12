using System;
using Enums;
using Interfaces;
using UnityEngine;
using Zenject;

public class GameManager : IInitializable
{
    private readonly EventService _eventService;
    private readonly SaveLoadService _saveLoadService;
    private readonly SceneService _sceneService;
    private readonly IBoard _board;
    private readonly Player _player1;
    private readonly Player _player2;

    public Player ActivePlayer { get; private set; }
    
    private const string HasSavedData = "saved";

    [Inject]
    public GameManager(
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
                ActivePlayer = lastPlayerSymbol == _player1?.Symbol ? _player1 : _player2;
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
            ActivePlayer = _player1;
        }
        
        _eventService.AddListener(EventName.MoveMade, CheckGameOver);
        _eventService.AddListener(EventName.StartNewGame, StartNewGame);
    }
 
    private void CheckGameOver()
    {
        var moveResult = _board.CheckMoveResult(ActivePlayer.Symbol);
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
            _saveLoadService.SaveBoardState(_board, ActivePlayer.Symbol);
            PlayerPrefs.SetInt(HasSavedData, 1);
        }
    }
    
    private void ChangePlayer()
    {
        ActivePlayer = ActivePlayer == _player1 ? _player2 : _player1;
    }

    private void StartNewGame()
    {
        PlayerPrefs.DeleteAll();
        _sceneService.ReloadScene();
    }

}
