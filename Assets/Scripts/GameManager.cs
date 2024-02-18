using Enums;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private Board _board;
    
    private static Player _player1;
    private static Player _player2;
    
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
    }
    
    private void Start()
    {
        _player1 = gameObject.AddComponent<Player>();
        _player2 = gameObject.AddComponent<Player>();
        _player1.Initialization(SymbolType.Cross);
        _player2.Initialization(SymbolType.Circle);

        ActivePlayer = _player1;
        
        if (PlayerPrefs.HasKey(HasSavedData))
        {
            SaveLoadService.LoadBoardState(_board);
        }
        else
        {
            _board.Initialization();
        }
        
        EventManager.AddListener(EventName.MoveMade, CheckGameOver);
    }

    private static void CheckGameOver()
    {
        var moveResult = Instance._board.CheckMoveResult(ActivePlayer.Symbol);
        if (moveResult != MoveResult.GameContinues)
        {
            Instance._board.HighlightWinCombination();
            EventManager.Invoke(EventName.GameOver, moveResult);
            PlayerPrefs.DeleteKey(HasSavedData);
        }
        else
        {
            ChangePlayer();
            SaveLoadService.SaveBoardState(Instance._board, ActivePlayer.Symbol);
            PlayerPrefs.SetInt(HasSavedData, 1);
        }
    }
    
    private static void ChangePlayer()
    {
        ActivePlayer = ActivePlayer == _player1 ? _player2 : _player1;
    }
}
