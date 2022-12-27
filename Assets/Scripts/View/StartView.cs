using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class StartView : ViewBase
{
    public override void OpenEndAnimation()
    {
        gameController?.DisableView(this);
       // StartCoroutine(gameController.PlayGame());
    }
}
