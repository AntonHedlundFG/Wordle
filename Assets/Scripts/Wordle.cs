using System.Text.RegularExpressions;


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
        bool[] marked = new bool[5]; //Used to make sure we don't get both a green and yellow marker for a letter that shows up once, such as guessing THERE if answer is GLARE. (First E is NOT yellow)

        Result[] returnArray = new Result[5];

        for (int i = 0; i < 5; i++)
        {
            if (targetWord[i] == guessWord[i])
            {
                returnArray[i] = Result.Correct;
                marked[i] = true;
                continue;
            }
            returnArray[i] = Result.Wrong;
        }
        for (int i = 0; i < 5; i++)
        {
            for (int j = 0; j < 5; j++)
            {
                if (targetWord[j] == guessWord[i] && !marked[j])
                {
                    returnArray[i] = Result.WrongSpot;
                    marked[j] = true;
                    continue;
                }
            }
        }

        return returnArray;
    }

    public static bool IsWin(Result[] results)
    {
        if (results == null || results.Length != 5)
        {
            return false;
        }
        for (int i = 0; i < results.Length; i++)
        {
            if (results[i] != Result.Correct)
            {
                return false;
            }
        }
        return true;
    }

    public static string IntToText(int number)
    {
        switch (number)
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
}
