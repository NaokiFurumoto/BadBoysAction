using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Viewの基底クラス
/// </summary>
public abstract class ViewBase : MonoBehaviour
{
    /// <summary>
    /// ゲームコントローラー
    /// </summary>
    [SerializeField]
    protected GameController gameController;

    protected Animator animator;

    public virtual void OpenEndAnimation() { }

    protected virtual void OnEnable()
    {
        animator = GetComponent<Animator>();
        if(animator != null)
        {
            animator?.SetTrigger("open");
        }
        
    }

    protected virtual void OnDisable()
    {
        if (animator != null)
        {
            animator?.SetTrigger("close");
        }
    }
}
