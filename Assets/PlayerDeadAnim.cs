using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDeadAnim : StateMachineBehaviour
{
    /// <summary>
    /// ���S�A�j���[�V�����I����
    /// </summary>
    /// <param name="animator"></param>
    /// <param name="stateInfo"></param>
    /// <param name="layerIndex"></param>
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        //���S�I���ʒm���s
        var gameController = GameObject.FindGameObjectWithTag("GameController").
                                        GetComponent<GameController>();

        var player = GameObject.FindGameObjectWithTag("Player");
        player?.SetActive(false);

        gameController.GameResult();
    }


}
