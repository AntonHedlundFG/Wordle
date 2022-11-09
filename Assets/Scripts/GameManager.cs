using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

public class GameManager : MonoBehaviour
{
    public UnityEvent<string> KeyPressEvent { get; private set; } = new UnityEvent<string>();
    [SerializeField] private GameBoard _gameBoard;
    private KeyboardManager _keyboardManager;

    [SerializeField] private TextAsset _targetWordFile;
    [SerializeField] private TextAsset _acceptableInputWordFile;
    private string[] _targetWordList;
    private HashSet<string> _acceptableInputWordHash;

    private Wordle.Word _targetWord;
    private int _currentGuessAmount = -1;
    private List<char> _currentGuess;
    private int _currentGuessLength = -1;

    private void Start()
    {
        SetupStartMenu();
    }
 
    public void SetKeyboardManager(KeyboardManager kbm)
    {
        if (kbm != null)
        {
            _keyboardManager = kbm;
        }
    }
    private void SetupStartMenu()
    {
        _gameBoard.ResetBoard();
        _keyboardManager.ResetKeyboard(true);
        string[] text = { "Press", "Any", "Key", "To", "Start" };
        for (int i = 0; i < text.Length; i++)
        {
            _gameBoard.UpdateRow(i, text[i]);
        }
        KeyPressEvent.AddListener(StartNewGame);
    }

    private void StartNewGame(string strNoUse)
    {
        KeyPressEvent.RemoveListener(StartNewGame);
        _gameBoard.ResetBoard();
        _keyboardManager.ResetKeyboard(false);

        _targetWord = GetRandomWord();
        _currentGuessAmount = 0;
        _currentGuessLength = 0;
        _currentGuess = new List<char>();
        KeyPressEvent.AddListener(OnGameInput);
    }

    private void OnGameInput(string keyPressed)
    {
        if (keyPressed.Length == 1 && _currentGuessLength < 5)
        {
            _currentGuess.Add(keyPressed.ToCharArray()[0]);
            _currentGuessLength++;
            _gameBoard.UpdateRow(_currentGuessAmount, new string(_currentGuess.ToArray()));
        }
        if (keyPressed == "BACK" && _currentGuessLength > 0)
        {
            _currentGuess.RemoveAt(_currentGuessLength - 1);
            _currentGuessLength--;
            _gameBoard.UpdateRow(_currentGuessAmount, new string(_currentGuess.ToArray()));
        }
        if (keyPressed == "ENTER" && _currentGuessLength == 5)
        {
            if (IsGuessAcceptable(_currentGuess))
            {
                TestAnswer(new string(_currentGuess.ToArray()));
            }
        }
    }
    private void TestAnswer(string answer)
    {
        Wordle.Word guessWord = new Wordle.Word(answer);
        Wordle.Result[] result = Wordle.GuessResult(guessWord, _targetWord);
        _gameBoard.UpdateRow(_currentGuessAmount, answer, result);
        _keyboardManager.UpdateKeyboard(answer, result);
        _currentGuessAmount++;

        if (Wordle.IsWin(result))
        {
            KeyPressEvent.RemoveListener(OnGameInput);
            SetupEndMenu(true, _currentGuessAmount);
            return;
        }
        if (_currentGuessAmount >= 6)
        {
            KeyPressEvent.RemoveListener(OnGameInput);
            SetupEndMenu(false);
            return;
        }
        _currentGuess = new List<char>();
        _currentGuessLength = 0;
    }

    private void SetupEndMenu(bool wonGame, int numberOfGuesses)
    {
        _gameBoard.ResetBoard();
        _keyboardManager.ResetKeyboard(true);

        if(wonGame)
        {
            SetBoardMessage(new string[] { "", "You", "Won", "", IntToText(numberOfGuesses), "Tries" });
            if (numberOfGuesses == 1)
            {
                _gameBoard.UpdateRow(5, "Try");
            }
            Wordle.Result[] result = Wordle.GuessResult(_targetWord, _targetWord);
            _gameBoard.UpdateRow(0, _targetWord.ToString(), result);

        } else
        {
            SetBoardMessage(new string[] { "The", "Word", "Was", _targetWord.ToString(), "Try", "Again"});
        }

        KeyPressEvent.AddListener(StartNewGame);
    }
    private void SetupEndMenu(bool wonGame)
    {
        SetupEndMenu(wonGame, 0);
    }


    private string IntToText(int number)
    {
        switch(number)
        {
            case 1:
                return "One";
            case 2:
                return "Two";
            case 3:
                return "Three";
            case 4:
                return "Four";
            case 5:
                return "Five";
            case 6:
                return "Six";
            default:
                return "";
        }
    }

    private void SetBoardMessage(string[] text)
    {
        for (int i = 0; i < text.Length; i++)
        {
            _gameBoard.UpdateRow(i, text[i]);
        }
    }

    private Wordle.Word GetRandomWord()
    {
        if (_targetWordList == null)
        {
            string[] targetWords = _targetWordFile.text.Split("\n");
            for (int i = 0; i < targetWords.Length; i++)
            {
                targetWords[i] = targetWords[i].Substring(0, Mathf.Min(5, targetWords[i].Length));
            }
            _targetWordList = targetWords;
        }
        if (_acceptableInputWordHash == null)
        {
            string[] _acceptableWords = _acceptableInputWordFile.text.Split("\n");
            for (int i = 0; i < _acceptableWords.Length; i++)
            {
                _acceptableWords[i] = _acceptableWords[i].Substring(0, Mathf.Min(5, _acceptableWords[i].Length));
            }
            _acceptableInputWordHash = _acceptableWords.ToHashSet();
        }

        string targetWord = _targetWordList[Random.Range(0, _targetWordList.Length)];
        return new Wordle.Word(targetWord);
    }

    private bool IsGuessAcceptable(List<char> guess)
    {

        return _acceptableInputWordHash.Contains(new string(guess.ToArray()));
        
    }
}
