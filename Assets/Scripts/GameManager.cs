using Enums;
using Interfaces;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private static IBoard _board;
    private static Player _player1;
    private static Player _player2;
    private bool _isInitialized;
    
    public static Player ActivePlayer { get; private set; }
    private static GameManager Instance { get; set; }
    
    private const string HasSavedData = "saved";

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(this);
        }
        
        _board = GameObject.FindGameObjectWithTag("Board")?.GetComponent<IBoard>();
        if (_board == null)
        {
            Debug.LogError("Cannot find appropriate board");
        }
        
        if (!_isInitialized)
        {
            if (_player1 == null)
            {
                _player1 = gameObject.AddComponent<Player>();
                _player1.Initialization(SymbolType.Cross);
            }
            if (_player2 == null)
            {
                _player2 = gameObject.AddComponent<Player>();
                _player2.Initialization(SymbolType.Cross);
            }
           
            _isInitialized = true;
        }
            
    }
    
    private void Start()
    {
        if (PlayerPrefs.HasKey(HasSavedData))
        {
            SaveLoadService.LoadBoardState(_board);
        }
        else
        {
            _board.Clear();
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
            _board.HighlightWinCombination();
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

    public static void StartNewGame()
    {
        SceneService.ReloadScene();
        PlayerPrefs.DeleteAll();
        EventService.ClearEvents();
    }
}
