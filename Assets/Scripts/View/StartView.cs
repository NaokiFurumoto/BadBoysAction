using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartView : ViewBase
{
    public override void OpenEndAnimation()
    {
        gameController?.DisableStartView();
    }
}
