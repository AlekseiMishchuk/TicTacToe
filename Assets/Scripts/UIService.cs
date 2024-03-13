using Enums;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class UIService : MonoBehaviour, IInitializable
{
    [SerializeField] private TMP_Text _resultText;
    [SerializeField] private GameObject _finalPopup;
    [SerializeField] private Button _startNewGameButton;
    [SerializeField] private Button _popupNewGameButton;

    private EventService _eventService;

    [Inject]
    public void Construct(EventService eventService)
    {
        _eventService = eventService;
    }
    
    public void Initialize()
    {
        _eventService.AddListener<MoveResult>(EventName.GameOver, GameOver);
        _startNewGameButton.onClick.AddListener(StartNewGame); 
        _popupNewGameButton.onClick.AddListener(ClosePopupAndRestart);
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
    }

    private void ClosePopupAndRestart()
    {
        _finalPopup.SetActive(false);
        StartNewGame();
    }
    
    private void StartNewGame()
    {
        _eventService.Invoke(EventName.StartNewGame);
    }
}