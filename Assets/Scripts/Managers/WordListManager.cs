using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class WordListManager : MonoBehaviour
{
    [SerializeField] private TextAsset _targetWordFile;
    [SerializeField] private TextAsset _acceptableGuessWordFile;

    private string[] _possibleTargetWordList;
    private HashSet<string> _acceptableGuessWordSet;

    private void Awake()
    {
        SetupWordLists();
    }

    private void SetupWordLists()
    {
        if (_possibleTargetWordList == null)
        {
            string[] targetWords = _targetWordFile.text.Split("\n");
            for (int i = 0; i < targetWords.Length; i++)
            {
                targetWords[i] = targetWords[i].Substring(0, Mathf.Min(5, targetWords[i].Length));
            }
            _possibleTargetWordList = targetWords;
        }
        if (_acceptableGuessWordSet == null)
        {
            string[] _acceptableWords = _acceptableGuessWordFile.text.Split("\n");
            for (int i = 0; i < _acceptableWords.Length; i++)
            {
                _acceptableWords[i] = _acceptableWords[i].Substring(0, Mathf.Min(5, _acceptableWords[i].Length));
            }
            _acceptableGuessWordSet = _acceptableWords.ToHashSet();
        }
    }
    public Wordle.Word GetRandomWord()
    {
        string targetWord = _possibleTargetWordList[Random.Range(0, _possibleTargetWordList.Length)];
        return new Wordle.Word(targetWord);
    }

    public bool IsGuessAcceptable(List<char> guess)
    {
        return (guess.Count == 5 && _acceptableGuessWordSet.Contains(new string(guess.ToArray())));
    }
}
