using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Practice : MonoBehaviour
{
    Command button_A;
    Command button_B;
}


class Command
{
    public Command() { }
    public virtual void Execute() { }
}

class JumpCommand : Command
{
    public override void Execute() { Jump(); }

    /// <summary>
    /// ジャンプ処理
    /// </summary>
    private void Jump()
    {

    }
}

class FireCommand : Command
{
    public override void Execute() { Fire(); }

    /// <summary>
    /// ジャンプ処理
    /// </summary>
    private void Fire()
    {

    }
}

