using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameDisplay : MonoBehaviour
{
    [SerializeField] float _breakBetweenRounds;

    private ITimer _timer;
    void Awake()
    {
        this._timer = GetComponent<ITimer>();
        this._timer.SetInitialTime(this._breakBetweenRounds);
        this._timer.ResetTimer();
    }

    void Update()
    {
        
    }
}
