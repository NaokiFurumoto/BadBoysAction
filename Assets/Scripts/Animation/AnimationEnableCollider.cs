using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// �J�v�Z���R���C�_�[��L���ɂ���
/// </summary>
public class AnimationEnableCollider : StateMachineBehaviour
{
    /// <summary>
    /// �A�j���[�V�����I����
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
