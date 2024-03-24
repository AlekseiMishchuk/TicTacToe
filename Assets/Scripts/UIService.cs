using Enums;
using Signals;
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

    private SignalBus _signalBus;

    [Inject]
    public void Construct(SignalBus signalBus)
    {
        _signalBus = signalBus;
    }
    
    public void Initialize()
    {
        _signalBus.Subscribe<GameOverSignal>(x => GameOver(x.MoveResult));
        _startNewGameButton.onClick.AddListener(StartNewGame); 
        _popupNewGameButton.onClick.AddListener(ClosePopupAndRestart);
    }
    
    public void GameOver(MoveResult whoWins)
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
        _signalBus.Fire<StartNewGameSignal>();
    }
}