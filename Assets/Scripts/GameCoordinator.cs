using System;
using Enums;
using Interfaces;
using Signals;
using UnityEngine;
using Zenject;

public class GameCoordinator : IInitializable
{
    private readonly SignalBus _signalBus;
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
        SaveLoadService saveLoadService,
        SceneService sceneService,
        SignalBus signalBus)
    {
        _board = board;
        _player1 = player1;
        _player2 = player2;
        _saveLoadService = saveLoadService;
        _sceneService = sceneService;
        _signalBus = signalBus;
    }
    
    public void Initialize()
    {
        _player1?.Initialize(SymbolType.Cross);
        _player2?.Initialize(SymbolType.Circle);
        _board.Initialize();
        
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
        
        _signalBus.Subscribe<MoveMadeSignal>(x => SetCellSymbol(x.Cell));
        _signalBus.Subscribe<StartNewGameSignal>(StartNewGame);
    }

    private void SetCellSymbol(ICell cell)
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
            _signalBus.Fire(new GameOverSignal() {MoveResult = moveResult});
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
        SceneService.ReloadScene();
    }

}
