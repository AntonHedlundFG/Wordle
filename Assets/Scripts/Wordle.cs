using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;


public static class Wordle
{
    public struct Word
    {
        public bool IsValid { get; private set; }
        private char[] _word;
        public Word(string word)
        {
            if (word.Length != 5 || !Regex.IsMatch(word, @"^[a-zA-Z]+$"))
            {
                _word = null;
                IsValid = false;
                return;
            }
               
            _word = new char[5];
            for (int i = 0; i < 5; i++)
            {
                _word[i] = word.ToUpper()[i];
            }
            IsValid = true;
        }

        public char[] GetWord()
        {
            return _word;
        }

        public override string ToString()
        {
            return new string(_word);
        }
    }

    public enum Result
    {
        Wrong = 0, 
        WrongSpot = 1,
        Correct = 2,
        Default = -1
    }

    public static Result[] GuessResult(Word guess, Word target)
    {
        if (!guess.IsValid || !target.IsValid)
        {
            return null;
        }

        char[] guessWord = guess.GetWord();
        char[] targetWord = target.GetWord();

        Result[] returnArray = new Result[5];

        for (int i = 0; i < 5; i++)
        {
            if (targetWord[i] == guessWord[i])
            {
                returnArray[i] = Result.Correct;
                continue;
            }

            returnArray[i] = Result.Wrong;

            for (int j = 0; j < 5; j++)
            {
                if (targetWord[j] == guessWord[i])
                {
                    returnArray[i] = Result.WrongSpot;
                    continue;
                }
            }
        }

        return returnArray;
    }

    public static bool IsWin(Result[] results)
    {
        for (int i = 0; i < results.Length; i++)
        {
            if (results[i] != Result.Correct)
            {
                return false;
            }
        }
        return true;
    }

    /*public static Word GetRandomWord()
    {
        int randomInt = Random.Range(0, 5);
        string randomString;
        switch (randomInt)
        {
            case 0:
                randomString = "HELLO";
                break;
            case 1:
                randomString = "BLOOP";
                break;
            case 2:
                randomString = "PEACH";
                break;
            case 3:
                randomString = "MOUNT";
                break;
            default:
                randomString = "PLEAD";
                break;
        }

        return new Word(randomString);
    }*/
}
