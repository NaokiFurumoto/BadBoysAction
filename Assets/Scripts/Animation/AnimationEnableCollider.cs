using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// カプセルコライダーを有効にする
/// </summary>
public class AnimationEnableCollider : StateMachineBehaviour
{
    /// <summary>
    /// アニメーション終了時
    /// </summary>
    /// <param name="animator"></param>
    /// <param name="stateInfo"></param>
    /// <param name="layerIndex"></param>
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if(animator != null)
        {
            var collider = animator.gameObject.GetComponent<CapsuleCollider2D>();
            collider.enabled = true;
        }
    }
}
