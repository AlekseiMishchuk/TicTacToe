using Interfaces;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private IBoard _board;
    public static GameManager Instance { get; private set; }
    
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
        gameObject.AddComponent<Player>().Initialization(SymbolType.Cross);
        gameObject.AddComponent<Player>().Initialization(SymbolType.Cross);
        
        if (PlayerPrefs.HasKey(InitializedKey))
        {
            SaveLoadService.LoadBoardState(_board);
        }
        else
        {
            _board.Initialization();
        }
        
        SaveLoadService.SaveBoardState(_board);
    }

    public void MakeMove()
    {
        
    }
}
