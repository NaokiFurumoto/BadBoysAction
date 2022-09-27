using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDeadAnim : StateMachineBehaviour
{
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
    }

    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
    }

    /// <summary>
    /// 死亡アニメーション終了時
    /// </summary>
    /// <param name="animator"></param>
    /// <param name="stateInfo"></param>
    /// <param name="layerIndex"></param>
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        //死亡終了通知実行
        EnemyStatusController enemyStatus = animator.transform.parent.parent.gameObject.
                                            GetComponent<EnemyStatusController>();
        if (enemyStatus != null)
        {
            enemyStatus.DeadEndCallback();
        }
    }

}
