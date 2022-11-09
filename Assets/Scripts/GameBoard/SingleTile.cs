using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Text.RegularExpressions;

public class SingleTile : MonoBehaviour
{
    [SerializeField] private Color _wrongColor;
    [SerializeField] private Color _wrongTileColor;
    [SerializeField] private Color _correctColor;

    [SerializeField] private SpriteRenderer _tileRenderer;
    [SerializeField] private TMP_Text _text;

    [SerializeField] private Vector3 _jiggleDistance;
    private Coroutine _jiggleRoutine;
    
    

    private void Awake()
    {
        SetColor(Wordle.Result.Wrong);
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
        }
    }

    public void SetLetter(char letter)
    {
        if (!char.IsLetter(letter))
        {
            return;
        }
        _text.text = char.ToUpper(letter).ToString();
    }

    public void ResetLetter()
    {
        _text.text = "";
        SetColor(Wordle.Result.Wrong);
    }

    public void JiggleTile(float jiggleTime)
    {
        if (_jiggleRoutine == null)
        {
            _jiggleRoutine = StartCoroutine(JiggleAnim(jiggleTime));
        }
    }

    private IEnumerator JiggleAnim(float jiggleTime)
    {
        Vector3 startPosition = transform.position;
        float time = 0f;

        while (time < jiggleTime / 2)
        {
            yield return new WaitForEndOfFrame();
            time += Time.deltaTime;
            transform.position = Vector3.Lerp(startPosition, startPosition + _jiggleDistance, time / jiggleTime);
        }

        while (time > 0)
        {
            yield return new WaitForEndOfFrame();
            time -= Time.deltaTime;
            transform.position = Vector3.Lerp(startPosition, startPosition + _jiggleDistance, time / jiggleTime);
        }
        transform.position = startPosition;
        _jiggleRoutine = null;
    }
}
