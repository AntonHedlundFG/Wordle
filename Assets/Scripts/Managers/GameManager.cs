using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GameManager : MonoBehaviour
{
    public UnityEvent<string> KeyPressEvent { get; private set; } = new UnityEvent<string>();

    [SerializeField] private LineIndicators _lineIndicators;
    [SerializeField] private WordListManager _wordListManager;
    [SerializeField] private GameBoard _gameBoard;
    [SerializeField] private KeyboardManager _keyboardManager;

    private Wordle.Word _targetWord;
    private int _currentGuessAmount = -1;
    private List<char> _currentGuess;
    private int _currentGuessLength = -1;

    private void Start()
    {
        SetupStartMenu();
        KeyPressEvent.AddListener(StartNewGame);
    }
    private void SetupStartMenu()
    {
        _gameBoard.ResetBoard();
        _keyboardManager.ResetKeyboard(true);

        string[] text = { "Press", "Any", "Key", "To", "Start" };
        _gameBoard.SetBoardMessage(text);
    }
    private void StartNewGame(string anyKey)
    {
        KeyPressEvent.RemoveListener(StartNewGame);
        _gameBoard.ResetBoard();
        _keyboardManager.ResetKeyboard(false);

        _targetWord = _wordListManager.GetRandomWord();
        SetCurrentGuessAmount(0);
        _currentGuessLength = 0;
        _currentGuess = new List<char>();
        KeyPressEvent.AddListener(OnGameInput);
    }
    private void OnGameInput(string keyPressed)
    {
        switch (keyPressed)
        {
            case "BACK":
                if (_currentGuessLength > 0)
                {
                    _currentGuess.RemoveAt(_currentGuessLength - 1);
                    _currentGuessLength--;
                    _gameBoard.UpdateRow(_currentGuessAmount, _currentGuess);
                }
                break;
            case "ENTER":
                if (_wordListManager.IsGuessAcceptable(_currentGuess))
                {
                    TryGuess(new string(_currentGuess.ToArray()));
                } else
                {
                    _gameBoard.JiggleRow(_currentGuessAmount);
                }
                break;
            default:
                if (_currentGuessLength < 5)
                {
                    _currentGuess.Add(keyPressed.ToCharArray()[0]);
                    _currentGuessLength++;
                    _gameBoard.UpdateRow(_currentGuessAmount, _currentGuess);
                }
                break;
        }
    }
    private void TryGuess(string answer)
    {
        Wordle.Word guessWord = new Wordle.Word(answer);
        Wordle.Result[] result = Wordle.GuessResult(guessWord, _targetWord);

        _gameBoard.UpdateRow(_currentGuessAmount, answer, result);
        _keyboardManager.UpdateKeyboard(answer, result);

        SetCurrentGuessAmount(_currentGuessAmount + 1);

        if (Wordle.IsWin(result)) //Game is won
        {
            KeyPressEvent.RemoveListener(OnGameInput);
            SetupEndMenu(true, _currentGuessAmount);
            return;
        }
        if (_currentGuessAmount >= 6) //Game is lost
        {
            KeyPressEvent.RemoveListener(OnGameInput);
            SetupEndMenu(false);
            return;
        }

        //Game continues
        _currentGuess = new List<char>();
        _currentGuessLength = 0;
    }

    private void SetupEndMenu(bool wonGame, int numberOfGuesses)
    {
        _gameBoard.ResetBoard();
        _keyboardManager.ResetKeyboard(true);

        if(wonGame)
        {
            _gameBoard.SetBoardMessage(new string[] { "", "You", "Won", "In", Wordle.IntToText(numberOfGuesses), "Tries" });
            if (numberOfGuesses == 1)
            {
                _gameBoard.UpdateRow(5, "Try");
            }
            Wordle.Result[] result = Wordle.GuessResult(_targetWord, _targetWord); //Creates an array full of Wordle.Result.Correct
            _gameBoard.UpdateRow(0, _targetWord.ToString(), result);

        } else
        {
            _gameBoard.SetBoardMessage(new string[] { "The", "Word", "Was", _targetWord.ToString(), "Try", "Again"});
        }
        SetCurrentGuessAmount(-1);
        KeyPressEvent.AddListener(StartNewGame);
    }
    private void SetupEndMenu(bool wonGame)
    {
        SetupEndMenu(wonGame, 0);
    }
    private void SetCurrentGuessAmount(int amount)
    {
        _currentGuessAmount = amount;
        _lineIndicators?.SetActiveRow(amount);
    }

}
