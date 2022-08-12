using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// View�̊��N���X
/// </summary>
public abstract class ViewBase : MonoBehaviour
{
    /// <summary>
    /// �Q�[���R���g���[���[
    /// </summary>
    [SerializeField]
    protected GameController gameController;

    protected Animator animator;

    public virtual void OpenEndAnimation() { }

    protected virtual void OnEnable()
    {
        animator = GetComponent<Animator>();
        animator?.SetTrigger("open");
    }

    protected virtual void OnDisable()
    {
        animator?.SetTrigger("close");
    }
}
