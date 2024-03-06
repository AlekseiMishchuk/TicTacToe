using System;
using Enums;
using Interfaces;
using UnityEngine;

public class GameManager : MonoBehaviour, IBootstrappable
{
    private static GameManager _instance;
    private static IBoard _board;
    private static Player _player1;
    private static Player _player2;

    public BootPriority BootPriority => BootPriority.Dependent;
    public static Player ActivePlayer { get; private set; }
    
    private const string HasSavedData = "saved";

    private void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
        }
        else
        {
            Destroy(this);
        }
    }

    public void ManualStart()
    {
        if (_player1 == null)
        {
            _player1 = _instance.gameObject.AddComponent<Player>();
            _player1.Initialization(SymbolType.Cross);
        }
        if (_player2 == null)
        {
            _player2 = _instance.gameObject.AddComponent<Player>(); 
            _player2.Initialization(SymbolType.Circle);
        }
        
        _board = GameObject.FindGameObjectWithTag("Board")?.GetComponent<IBoard>();
        if (_board == null)
        {
            Debug.LogError("Cannot find appropriate board");
        }
        
        if (PlayerPrefs.HasKey(HasSavedData))
        {
            try
            {
                var lastPlayerSymbol = SaveLoadService.LoadBoardState(_board);
                ActivePlayer = lastPlayerSymbol == _player1.Symbol ? _player1 : _player2;
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
        
        EventService.AddListener(EventName.MoveMade, CheckGameOver);
        EventService.AddListener(EventName.StartNewGame, StartNewGame);
    }

    private static void CheckGameOver()
    {
        var moveResult = _board.CheckMoveResult(ActivePlayer.Symbol);
        if (moveResult != MoveResult.GameContinues)
        {
            if(moveResult != MoveResult.Draw)
            {
                _board.HighlightWinCombination();
            }
            EventService.Invoke(EventName.GameOver);
            EventService.Invoke(EventName.GameOver, moveResult);
            PlayerPrefs.DeleteKey(HasSavedData);
        }
        else
        {
            ChangePlayer();
            SaveLoadService.SaveBoardState(_board, ActivePlayer.Symbol);
            PlayerPrefs.SetInt(HasSavedData, 1);
        }
    }
    
    private static void ChangePlayer()
    {
        ActivePlayer = ActivePlayer == _player1 ? _player2 : _player1;
    }

    private static void StartNewGame()
    {
        SceneService.ReloadScene();
        PlayerPrefs.DeleteAll();
    }

}
