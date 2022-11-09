using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class WordListManager : MonoBehaviour
{
    [SerializeField] private TextAsset _targetWordFile;
    [SerializeField] private TextAsset _acceptableInputWordFile;

    private string[] _targetWordList;
    private HashSet<string> _acceptableInputWordHash;

    private void Awake()
    {
        SetupWordLists();
    }

    private void SetupWordLists()
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
    }
    public Wordle.Word GetRandomWord()
    {
        string targetWord = _targetWordList[Random.Range(0, _targetWordList.Length)];
        return new Wordle.Word(targetWord);
    }

    public bool IsGuessAcceptable(List<char> guess)
    {
        return (guess.Count == 5 &&_acceptableInputWordHash.Contains(new string(guess.ToArray())));
    }
}
