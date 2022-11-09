using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;

public class GameBoard : MonoBehaviour
{
    [SerializeField] TileRow[] _tileRows;

    public void ResetBoard()
    {
        for (int i = 0; i < _tileRows.Length; i++)
        {
            _tileRows[i].ResetWord();
        }
    }

    public void UpdateRow(int row, string word, Wordle.Result[] results)
    {
        if (row < 0 || row >= _tileRows.Length)
        {
            return;
        }
        if (word.Length == 0)
        {
            _tileRows[row].ResetRow();
            return;
        }

        if (!Regex.IsMatch(word, @"^[a-zA-Z]+$"))
        {
            return;
        }
        _tileRows[row].ResetRow();

        _tileRows[row].WriteWord(word);
        _tileRows[row].SetColors(results);

    }
    public void UpdateRow(int row, string word)
    {

        Wordle.Result[] results = new Wordle.Result[word.Length];
        for (int i = 0; i < results.Length; i++)
        {
            results[i] = Wordle.Result.Wrong;
        }
        UpdateRow(row, word, results);
    }
    public void UpdateRow(int row, List<char> word)
    {
        string wordString = new string(word.ToArray());
        UpdateRow(row, wordString);
    }
    public void SetBoardMessage(string[] text)
    {
        for (int i = 0; i < text.Length && i < _tileRows.Length; i++)
        {
            UpdateRow(i, text[i]);
        }
    }

}
