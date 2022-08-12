using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static GlobalValue;
using System.Linq;
/// <summary>
/// �G�����N���X
/// </summary>
public enum GENERATOR_RANK
{
    NONE,
    FIRST,
    SECOND,
    THIRD,
    FOURTH,
    FIFTH,
    SIX,
    SEVENTH,
    EIGHT,
}

/// <summary>
/// ������ԏ��
/// </summary>
public enum GENERATOR_STATE
{
    NONE,
    GENERATE,//������
    STOP,//��~��
    RESUME,//�ĊJ
}

public class EnemyGenerator : MonoBehaviour
{

    #region �G�Ɋւ���
    /// <summary>
    /// �����I�u�W�F�N�g���X�g
    /// </summary>
    [SerializeField]
    private List<GameObject> enemyObjects = new List<GameObject>();

    /// <summary>
    /// ��������
    /// </summary>
    [SerializeField]
    private List<int> enemyEncounts = new List<int>();

    /// <summary>
    /// �G�����e
    /// </summary>
    [SerializeField]
    private Transform enemysParent;

    [SerializeField]
    private GameController gameController;

    /// <summary>
    /// �G������
    /// </summary>
    private static int enemyCreateCount;

    /// <summary>
    /// �G��ސ�
    /// </summary>
    private int enemyTypeCount;

    /// <summary>
    /// �����ʒu
    /// </summary>
    private Vector3 createEnemyPos;

    /// <summary>
    /// �����ʒu�ɐ������Ȃ��Ή�
    /// </summary>
    private float tempPos_x, tempPos_y = 0.0f;

    /// <summary>
    /// ��������G
    /// </summary>
    private GameObject createEnemyObj;
    #endregion

    #region �����@�Ɋւ���
    /// <summary>
    /// ���������N
    /// </summary>
    [SerializeField]
    private GENERATOR_RANK rank;

    /// <summary>
    /// ���
    /// </summary>
    [SerializeField]
    private GENERATOR_STATE state;

    /// <summary>
    /// ���g�̈ʒu
    /// </summary>
    private Vector3 rootPosition;

    //�����Ԋu����
    [SerializeField]
    private float createDelayTime;

    /// <summary>
    /// �v������
    /// </summary>
    private float progressTime;
    #endregion

    #region �v���p�e�B
    public GENERATOR_STATE State { get { return state; } set { state = value; } }
    #endregion

    /// <summary>
    /// �L�����ɌĂ΂��
    /// </summary>
    private void OnEnable()
    {
        Initialize();

        //�����Ԋu�ݒ�
        SetCreateDelay();
    }

    /// <summary>
    /// �������ɌĂ΂��
    /// </summary>

    private void OnDisable()
    {
        
    }

    private void Initialize()
    {
        //�G
        enemyCreateCount = 0;
        rootPosition = transform.position;
        enemyTypeCount = enemyObjects.Count;

        //�����@
        state = GENERATOR_STATE.STOP;
        createDelayTime = 0;
        progressTime = 0;
        if (gameController == null)
        {
            gameController = GameObject.FindGameObjectWithTag("GameController")
                                   .GetComponent<GameController>();
        }
    }

    /// <summary>
    /// �X�V
    /// </summary>
    private void Update()
    {
        //�Q�[���J�n���Ă��Ȃ���Γ������Ȃ�
        //if (gameController.State != INGAME_STATE.PLAYING)
        //    return;

        if (state == GENERATOR_STATE.STOP)
            return;

        //������
        if (state == GENERATOR_STATE.GENERATE)
        {
            //�o�ߎ���
            progressTime += Time.deltaTime;

            if(progressTime > createDelayTime)
            {
                //�G�̃Z�b�g
                createEnemyObj = SetEnemy();
                CreateEnemy();
                progressTime = 0;
            }
        }
    }

    /// <summary>
    /// �G����
    /// �w��̈ʒu�Ɏw��Obj�𐶐�
    /// </summary>
    private void CreateEnemy()
    {
        if (createEnemyObj == null)
            return;

        SetEnemyPos();

        var obj = Instantiate(createEnemyObj, createEnemyPos, Quaternion.identity);
        obj.transform.SetParent(enemysParent);

        //�G��HP��ݒ肷�鏈���K�v�H
        enemyCreateCount++;
        //�����`�F�b�N
        CheckOverChangeState();
    }

    /// <summary>
    /// �����ʒu�̐ݒ�
    /// </summary>
    private void SetEnemyPos()
    {
        float x, y;

        do
        {
            x = Random.Range(rootPosition.x - EN_CREATEPOS_RADIUS,
                         rootPosition.x + EN_CREATEPOS_RADIUS);
            y = Random.Range(rootPosition.y - EN_CREATEPOS_RADIUS,
                                   rootPosition.y + EN_CREATEPOS_RADIUS);

        } while (Mathf.Abs(x - tempPos_x) <= CREATE_DIFFX || Mathf.Abs(y - tempPos_y) <= CREATE_DIFFY);

        tempPos_x = x;
        tempPos_y = y;

        createEnemyPos = new Vector3((float)tempPos_x, (float)tempPos_y, 0);
    }

    //�����Ԋu�̐ݒ菈��
    private void SetCreateDelay()
    {
        createDelayTime = 1.0f;
    }


    /// <summary>
    /// ��������G�L������ݒ�
    /// </summary>
    private GameObject SetEnemy()
    {
        var totalRatio = TotalRatio();
        var createNum = Random.Range(1,totalRatio);
       
        var total = 0;
        for (int i = 0; i < enemyTypeCount; i++)
        {
            total += enemyEncounts[i];
            if(createNum <= total)
            {
                return enemyObjects[i];
            }
        }
        return null;
    }

    /// <summary>
    /// ���v�G���J�E���g��
    /// ���̏����̑O�ɃG���J�E���g����ύX����
    /// </summary>
    /// <returns></returns>
    private int TotalRatio()
    {
        return enemyEncounts.Sum();
    }

    /// <summary>
    /// �����Ԋu�̕ύX
    /// </summary>
    private void ChangeCreateDelay()
    {
        //���j���ɂ���ĊԊu��Z���F�ŏ��ƍő��݂���
        //���j����100�𒴂���x�ɐ����Ԋu��-0.1�b�ÂZ������H
        //�ŏ��l�ƍő�l�̊�
    }

    /// <summary>
    /// �ő吶�����ɂ���ăX�e�[�g�̕ύX
    /// �ǂ����炩�ĂԕK�v������
    /// </summary>
    public void CheckOverChangeState()
    {
        if (enemysParent == null)
            return;
        
        if(enemysParent.childCount >= MAX_GE_CREATECOUNT)
        {
            state = GENERATOR_STATE.STOP;
        }
        else
        {
            state = GENERATOR_STATE.GENERATE;
        }
    }

    //�G��HP��ݒ�F�G�L�����̃R�[�h���Ăяo��
    
}
