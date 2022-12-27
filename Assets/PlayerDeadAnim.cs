using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDeadAnim : StateMachineBehaviour
{
    /// <summary>
    /// 死亡アニメーション終了時
    /// </summary>
    /// <param name="animator"></param>
    /// <param name="stateInfo"></param>
    /// <param name="layerIndex"></param>
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        //死亡終了通知実行
        var gameController = GameObject.FindGameObjectWithTag("GameController").
                                        GetComponent<GameController>();

        var player = GameObject.FindGameObjectWithTag("Player");
        var Conteroll = player.GetComponent<PlayerStatusController>();
       
        player?.SetActive(false);
        Conteroll.Sprite.color = new Color(1.0f, 1.0f, 1.0f, 1.0f);
        gameController.GameResult();
    }


}
