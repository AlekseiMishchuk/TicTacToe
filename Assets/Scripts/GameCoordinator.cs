using System;
using Enums;
using Interfaces;
using Signals;
using UnityEngine;
using Zenject;

public class GameCoordinator : IInitializable
{
    private readonly SignalBus _signalBus;
    private readonly DataStorageService _dataStorageService;
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
        DataStorageService dataStorageService,
        SaveLoadService saveLoadService,
        SceneService sceneService,
        SignalBus signalBus)
    {
        _player1 = player1;
        _player2 = player2;
        _board = board;
        _dataStorageService = dataStorageService;
        _saveLoadService = saveLoadService;
        _sceneService = sceneService;
        _signalBus = signalBus;
    }
    
    public void Initialize()
    {
        _player1?.Initialize(SymbolType.Cross);
        _player2?.Initialize(SymbolType.Circle);
        _board.Initialize();
        
        if (_dataStorageService.HasKey(HasSavedData))
        {
            LoadSavedData();
        }
        else
        {
            _activePlayer = _player1;
        }
        
        _signalBus.Subscribe<MoveMadeSignal>(x => SetCellSymbol(x.Cell));
        _signalBus.Subscribe<StartNewGameSignal>(StartNewGame);
    }

    private void LoadSavedData()
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
        finally
        {
            _dataStorageService.DeleteAll();
        }
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
            GameOver(moveResult);
        }
        else
        {
            GameContinues();
        }
    }

    private void GameContinues()
    {
        ChangePlayer();
        _saveLoadService.SaveBoardState(_board, _activePlayer.Symbol);
        _dataStorageService.SetInt(HasSavedData, 1);
    }

    private void GameOver(MoveResult moveResult)
    {
        _signalBus.Fire(new GameOverSignal() {MoveResult = moveResult});
        _dataStorageService.DeleteKey(HasSavedData);
    }

    private void ChangePlayer()
    {
        _activePlayer = _activePlayer == _player1 ? _player2 : _player1;
    }

    private void StartNewGame()
    {
        _dataStorageService.DeleteAll();
        _sceneService.ReloadScene();
    }

}
