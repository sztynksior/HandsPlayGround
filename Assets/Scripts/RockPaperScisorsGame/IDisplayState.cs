using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDisplayState
{
    public void Enter(GameDisplay gameDisplay);

    public void Update(GameDisplay gameDisplay);

    public void Exit(GameDisplay gameDisplay);

}
