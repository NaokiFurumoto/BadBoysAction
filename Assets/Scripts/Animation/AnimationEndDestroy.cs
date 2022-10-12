using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// アニメーション終了後に削除する
/// </summary>

public class AnimationEndDestroy : StateMachineBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    /// <summary>
    /// アニメーション終了時
    /// </summary>
    /// <param name="animator"></param>
    /// <param name="stateInfo"></param>
    /// <param name="layerIndex"></param>
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        Destroy(animator.gameObject);
    }
}
