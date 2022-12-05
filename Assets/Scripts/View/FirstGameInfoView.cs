using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class FirstGameInfoView : ViewBase
{
    [SerializeField]
    private Toggle toggle;

    public UnityAction CallBack;

    protected override void OnEnable() { }

    public void OnClickCloseBtn()
    {
        //�`�F�b�N�{�b�N�X�f�[�^�̕ۑ�
        var loadData = SaveManager.Instance.Load();
        gameController.IsOpenFirstview =
        loadData.IsFirstViewOpen = toggle.isOn;

        //�Z�[�u����
        SaveManager.Instance.Save(loadData);
        CallBack?.Invoke();

        this.gameObject.SetActive(false);
    }

    protected override void OnDisable()
    {
        base.OnDisable();
    }
}
