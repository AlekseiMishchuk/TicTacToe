using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private Board _board;
    
    private static Player _player1;
    private static Player _player2;
    
    public static Player ActivePlayer { get; private set; }
    private static GameManager Instance { get; set; }
    
    private const string InitializedKey = "initialized";

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }
    
    private void Start()
    {
        _player1 = gameObject.AddComponent<Player>();
        _player2 = gameObject.AddComponent<Player>();
        _player1.Initialization(SymbolType.Cross);
        _player2.Initialization(SymbolType.Circle);

        ActivePlayer = _player1;
        
        if (PlayerPrefs.HasKey(InitializedKey))
        {
            SaveLoadService.LoadBoardState(_board);
        }
        else
        {
            _board.Initialization();
        }
        
        EventManager.AddListener(Event.MoveMade, CheckWinConditions);
    }

    private static void CheckWinConditions()
    {
        if (Instance._board.WinConditions(ActivePlayer.Symbol))
        {
            Debug.Log($"{ActivePlayer.Symbol.GetType()} win");
            Instance._board.HighlightWinCombination();
        }
        else
        {
            ChangePlayer();
            SaveLoadService.SaveBoardState(Instance._board);
        }
    }
    private static void ChangePlayer()
    {
        ActivePlayer = ActivePlayer == _player1 ? _player2 : _player1;
    }
}
