using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class KeyboardButton : MonoBehaviour
{
    [SerializeField] private Color _wrongColor;
    [SerializeField] private Color _wrongTileColor;
    [SerializeField] private Color _correctColor;
    [SerializeField] private Color _defaultColor;

    [SerializeField] private SpriteRenderer _tileRenderer;

    [SerializeField] private string _key;

    private KeyboardManager _keyboardEvents;
    private Wordle.Result _currentColor;

    private void Awake()
    {
        _keyboardEvents = GetComponentInParent<KeyboardManager>();
        SetColor(Wordle.Result.Default);
    }

    private void Start()
    {
        _keyboardEvents.KeyColorEvent?.AddListener(OnKeyColorChange);
    }

    private void OnDestroy()
    {
        _keyboardEvents.KeyColorEvent?.RemoveListener(OnKeyColorChange);
    }
    private void OnMouseDown()
    {
        _keyboardEvents?.KeyPress(_key);
    }

    private void OnKeyColorChange(string key, Wordle.Result result, bool onlyUpgrade)
    {
        if (key != _key && key != "ALL")
        {
            return;
        }
        if (onlyUpgrade && (int)result < (int) _currentColor)
        {
            return;
        }
        SetColor(result);
    }

    public void SetColor(Wordle.Result result)
    {
        switch (result)
        {
            case Wordle.Result.Wrong:
                _tileRenderer.color = _wrongColor;
                break;
            case Wordle.Result.WrongSpot:
                _tileRenderer.color = _wrongTileColor;
                break;
            case Wordle.Result.Correct:
                _tileRenderer.color = _correctColor;
                break;
            case Wordle.Result.Default:
                _tileRenderer.color = _defaultColor;
                break;
            default:
                return;
        }
        _currentColor = result;
    }
}
