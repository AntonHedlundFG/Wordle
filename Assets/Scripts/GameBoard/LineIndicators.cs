using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineIndicators : MonoBehaviour
{
    [SerializeField] private GameObject[] _indicators;

    public void SetActiveRow(int row)
    {
        for (int i = 0; i < _indicators.Length; i++)
        {
            _indicators[i].SetActive(i == row ? true : false);
        }
        
    }
    
}
