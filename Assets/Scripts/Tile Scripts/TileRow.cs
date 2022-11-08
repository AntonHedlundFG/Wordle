using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;

public class TileRow : MonoBehaviour
{
    [SerializeField] private SingleTile[] _tiles;

    public void WriteWord(string writeString)
    {
        if(!Regex.IsMatch(writeString, @"^[a-zA-Z]+$"))
        {
            return;
        }

        for (int i = 0; i < _tiles.Length; i++)
        {
            _tiles[i].ResetLetter();
            if (i < writeString.Length)
            {
                _tiles[i].SetLetter(writeString[i]);
            }
        }
    }

    public void ResetWord()
    {
        for (int i = 0; i < _tiles.Length; i++)
        {
            _tiles[i].ResetLetter();
        }
    }

    public void ResetRow()
    {
        ResetWord();
        ResetColors();
    }

    private void ResetColors()
    {
        for (int i = 0; i < _tiles.Length; i++)
        {
            _tiles[i].SetColor(Wordle.Result.Wrong);
        }
    }

    public void SetColors(Wordle.Result[] results)
    {
        for (int i = 0; i < _tiles.Length && i < results.Length; i++)
        {
            _tiles[i].SetColor(results[i]);
        }
    }

}
