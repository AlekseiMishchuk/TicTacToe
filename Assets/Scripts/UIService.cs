using Enums;
using Interfaces;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIService : MonoBehaviour, IManualInitialization
{
    [SerializeField] private TMP_Text _resultText;
    [SerializeField] private GameObject _finalPopup;
    [SerializeField] private Button _startNewGameButton;
    [SerializeField] private Button _popupNewGameButton;

    public BootPriority BootPriority => BootPriority.Dependent;

    private static UIService Instance { get; set; }

    public void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(this);
        }
    }

    public void ManualInit()
    {
        EventService.AddListener<MoveResult>(EventName.GameOver, GameOver);
        _startNewGameButton.onClick.AddListener(StartNewGame);
    }

    private void GameOver(MoveResult whoWins)
    {
        _finalPopup.SetActive(true);
        switch (whoWins)
        {
            
            case MoveResult.CrossesWin:
                _resultText.text = "Crosses won!";
                break;
            case MoveResult.CirclesWin:
                _resultText.text = "Circles won!";
                break;
            case MoveResult.Draw:
            default:
                _resultText.text = "Draw!";
                break;        
        }
        
        _popupNewGameButton.onClick.AddListener(ClosePopupAndRestart);
    }

    private void ClosePopupAndRestart()
    {
        _finalPopup.SetActive(false);
        StartNewGame();
    }
    private static void StartNewGame()
    {
        EventService.Invoke(EventName.StartNewGame);
    }
}