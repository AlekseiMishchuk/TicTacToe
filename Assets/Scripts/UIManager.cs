using System;
using Enums;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField] private TMP_Text _resultText;
    [SerializeField] private Image _backgroundShader;

    private static UIManager Instance { get; set; }

    public void Awake()
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

    public void Start()
    {
        EventManager.AddListener<MoveResult>(EventName.GameOver, GameOver);
    }

    private void GameOver(MoveResult whoWins)
    {
        _backgroundShader.gameObject.SetActive(true);
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
}