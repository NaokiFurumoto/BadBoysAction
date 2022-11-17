using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
/// <summary>
/// �G�t�F�N�g�\���Ǘ��N���X
/// </summary>

public class PlayerEffectManager : MonoBehaviour
{
    /// <summary>
    /// �C���X�^���X
    /// </summary>
    public static PlayerEffectManager Instance;

    [SerializeField]
    private Transform under_root;

    [SerializeField]
    private Transform center_root;

    [SerializeField]
    private Transform top_root;

    [SerializeField]
    private List<EffectStatus> effectObjects = new List<EffectStatus>();

    /// <summary>
    /// �C���X�^���X���ꂽ�G�t�F�N�g
    /// </summary>
    private List<GameObject> effectPlacedObjects = new List<GameObject>();

    private Transform parent;

    private void Awake() { InitializeThis(); }
    void InitializeThis()
    {
        Instance ??= this;
        effectPlacedObjects.Clear();
    }

    /// <summary>
    /// �Đ��F���Ƃ�SE���������ɓn���悤��
    /// </summary>
    /// <param name="effect"></param>
    public GameObject EffectPlay(EFFECT_TYPE type)
    {
        var effect = effectObjects.Single(list => list.EffectType == type);
        if (effect == null)
            return null;
        
        switch (effect.RootType)
        {
            case PEFFECT_ROOT_TYPE.UNDER:
                parent = under_root;
                break;
            case PEFFECT_ROOT_TYPE.CENTER:
                parent = center_root;
                break;
            case PEFFECT_ROOT_TYPE.TOP:
                parent = top_root;
                break;
        }

        var obj = Instantiate(effect.gameObject,parent);
        obj.transform.localPosition = new Vector2(effect.Pos.x, effect.Pos.y);

        effectPlacedObjects.Add(obj);
        return obj;
    }

    /// <summary>
    /// �G�t�F�N�g�폜
    /// </summary>
    /// <param name="effect"></param>
    public void DeleteEffect(GameObject effect)
    {
        Destroy(effect);
    }

    /// <summary>
    /// �S�ẴG�t�F�N�g�̍폜
    /// </summary>
    public void DeleteAllEffects()
    {
        effectPlacedObjects.Clear();
    }

    /// <summary>
    /// �w�肵���G�t�F�N�g�폜
    /// </summary>
    public void DeleteSelectEffects(EFFECT_TYPE type)
    {
        effectPlacedObjects.RemoveAll(list => list == null);
        if (effectPlacedObjects.Count == 0)
            return;

        foreach(var list in effectPlacedObjects)
        {
            if(list.GetComponent<EffectStatus>().EffectType == type)
            {
                Destroy(list.gameObject);
            }
        }
    }

    /// <summary>
    /// ���g���C����
    /// </summary>
    public void Retry()
    {
        DeleteAllEffects();
    }

}
