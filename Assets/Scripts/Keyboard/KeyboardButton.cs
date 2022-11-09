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

    private KeyboardManager _keyboardManager;
    private Wordle.Result _currentColor;

    private KeyCode _keyCode;

    [SerializeField][Range(0f, 0.5f)] private float _pressEffectTime = 0.1f;
    [SerializeField][Range(0f, 1f)] private float _pressEffectModifier = 0.9f;
    private Coroutine _pressedRoutine;

    private void Awake()
    {
        _keyboardManager = GetComponentInParent<KeyboardManager>();
        SetColor(Wordle.Result.Default);
        switch (_key)
        {
            case "ENTER":
                _keyCode = KeyCode.Return;
                break;
            case "BACK":
                _keyCode = KeyCode.Backspace;
                break;
            default:
                _keyCode = (KeyCode)System.Enum.Parse(typeof(KeyCode), _key);
                break;
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(_keyCode))
        {
            InvokeKeyDown();
        }
    }

    private void Start()
    {
        _keyboardManager.KeyColorEvent?.AddListener(OnKeyColorChange);
    }

    private void OnDestroy()
    {
        _keyboardManager.KeyColorEvent?.RemoveListener(OnKeyColorChange);
    }
    private void OnMouseDown()
    {
        InvokeKeyDown();
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

    private void InvokeKeyDown()
    {
        _keyboardManager?.KeyPress(_key);
        if (_pressedRoutine == null)
        {
            _pressedRoutine = StartCoroutine(OnPressEffect());
        }
    }

    private IEnumerator OnPressEffect()
    {
        Vector3 startScale = transform.localScale;
        float time = 0f;
        float halfEffectTime = _pressEffectTime / 2;

        while (time < halfEffectTime)
        {
            yield return new WaitForEndOfFrame();
            time += Time.deltaTime;
            transform.localScale = Vector3.Lerp(startScale, startScale * _pressEffectModifier, time / halfEffectTime);
        }

        while (time > 0)
        {
            yield return new WaitForEndOfFrame();
            time -= Time.deltaTime;
            transform.localScale = Vector3.Lerp(startScale, startScale * _pressEffectModifier, time / halfEffectTime);
        }
        transform.localScale = startScale;

        _pressedRoutine = null;
        yield return null;
    }
}
