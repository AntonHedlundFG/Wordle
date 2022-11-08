using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class KeyboardManager : MonoBehaviour
{
    public UnityEvent<string, Wordle.Result, bool> KeyColorEvent { get; private set; }
    [SerializeField] private GameManager _gameManager;


    private void Update()
    {
        
    }

    private void Awake()
    {
        if (KeyColorEvent == null)
        {
            KeyColorEvent = new UnityEvent<string, Wordle.Result, bool>();
        }
        _gameManager?.SetKeyboardManager(this);
    }
    public void KeyPress(string key)
    {
        _gameManager?.KeyPressEvent?.Invoke(key);
    }

    public void UpdateKeyboard(string word, Wordle.Result[] result)
    {
        for (int i = 0; i < word.Length; i++)
        {
            KeyColorEvent.Invoke(word[i].ToString(), result[i], true);
        }
    }
    public void ResetKeyboard(bool writePlay)
    {
        KeyColorEvent.Invoke("ALL", Wordle.Result.Default, false);
        if (writePlay)
        {
            WritePlay();
        }
        
    }

    private void WritePlay()
    {
        KeyColorEvent.Invoke("P", Wordle.Result.Correct, false);
        KeyColorEvent.Invoke("L", Wordle.Result.Correct, false);
        KeyColorEvent.Invoke("A", Wordle.Result.Correct, false);
        KeyColorEvent.Invoke("Y", Wordle.Result.Correct, false);
    }

}
