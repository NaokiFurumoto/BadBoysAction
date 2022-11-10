using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cysharp.Threading.Tasks;
using System.Linq;
using static GlobalValue;
using System.Threading;

///// <summary>
///// �G������N���X
///// </summary>
//public enum GENERATOR_INDEX
//{
//    NONE,
//    FIRST,
//    SECOND,
//    THIRD,
//    FOURTH,
//    FIFTH,
//    SIX,
//    SEVENTH,
//    EIGHT,
//}

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
public class NewEnemyGenerator : MonoBehaviour
{
    #region �G�Ɋւ���
    /// <summary>
    /// ��������G�̃I�u�W�F�N�g���X�g
    /// </summary>
    [SerializeField]
    private List<GameObject> enemyObjects = new List<GameObject>();

    /// <summary>
    /// ��������:���x���ɂ���ĕω�����l
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
    private int enemyCreateCount;

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

    private Vector3 rightTopScreen;
    private Vector3 leftBottomScreen;

    private List<GameObject> enemyAllObjects = new List<GameObject>();
    #endregion


    #region �����@�Ɋւ���
    /// <summary>
    /// ���
    /// </summary>
    [SerializeField]
    private GENERATOR_STATE state;

    /// <summary>
    /// ���g�̈ʒu
    /// </summary>
    private Vector3 rootPosition;

    //�����Ԋu���ԁF���x���ɂ���ĕω�������
    [SerializeField]
    private float createDelayTime;

    //�����x���J�n����
    private int startDelayTaskTime;

    /// <summary>
    /// �v�����ԁFTime.deltaTime�ŉ��Z���Ă���
    /// </summary>
    private float progressTime;
    #endregion

    #region �v���p�e�B
    public GENERATOR_STATE State { get { return state; } set { state = value; } }
    public List<GameObject> EnemyAllObjects => enemyAllObjects;

    //���x���ɂ���Đe����ݒ肳����l
    public float CreateDelayTime { get { return createDelayTime; } set { createDelayTime = value; } }
    public List<int> EnemyEncounts { get { return enemyEncounts; } set { enemyEncounts = value; } }
    public int StartDelayTaskTime { get { return startDelayTaskTime; } set { startDelayTaskTime = value; } }
    #endregion

    /// <summary>
    /// ���g�̏�����
    /// </summary>
    private void Awake() { InitializeThis(); }
    private void InitializeThis()
    {
        //�X�N���[���͈�
        rightTopScreen = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, 0));
        leftBottomScreen = Camera.main.ScreenToWorldPoint(Vector3.zero);

        //�G
        enemyCreateCount = 0;
        rootPosition = transform.position;
        enemyTypeCount = enemyObjects.Count;

        //�����@
        state = GENERATOR_STATE.STOP;
        progressTime = 0.0f;
        startDelayTaskTime = START_CREATE_DIFF;

        //���x���ɂ���ĕω�������
        createDelayTime = 0.01f;

        if (gameController == null)
        {
            gameController = GameObject.FindGameObjectWithTag("GameController")
                                   .GetComponent<GameController>();
        }
    }

    /// <summary>
    /// �O������������
    /// </summary>
    private void Start() { InitializeOther(); }
    private void InitializeOther() { }

    #region UniTask
    /// <summary>
    /// �Q�[���J�n���ɌĂ΂�鏈��
    /// </summary>
    public async UniTask StartCallGenerator()
    {
        await UniTask.Delay(startDelayTaskTime);
        state = GENERATOR_STATE.GENERATE;
    }

    /// <summary>
    /// �X�V
    /// </summary>
    private void Update()
    {
        //�Q�[���J�n���Ă��Ȃ���Γ������Ȃ�
        if (gameController.State != INGAME_STATE.PLAYING)
            return;

        //������
        if (state == GENERATOR_STATE.GENERATE)
        {
            //�o�ߎ���
            progressTime += Time.deltaTime;

            if (progressTime > createDelayTime)
            {
                //�����`�F�b�N
                if (IsCheckOver()) return;

                //�G�̃Z�b�g
                createEnemyObj = SetEnemy();
                CreateEnemy();
                progressTime = 0;
            }
        }
    }
    #endregion

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
        enemyAllObjects.Add(obj);

        enemyCreateCount++;
    }

    /// <summary>
    /// �����ʒu�̐ݒ�
    /// </summary>
    private void SetEnemyPos()
    {
        float x, y;
        bool isCheck;

        do
        {
            x = Random.Range(leftBottomScreen.x - ENEMY_CREATE_DIFF_MAX,
                              rightTopScreen.x + ENEMY_CREATE_DIFF_MAX);
            y = Random.Range(leftBottomScreen.y - ENEMY_CREATE_DIFF_MAX,
                              rightTopScreen.y + ENEMY_CREATE_DIFF_MAX);

            isCheck = (leftBottomScreen.x - ENEMY_CREATE_DIFF_MIN <= x)
                    && (rightTopScreen.x + ENEMY_CREATE_DIFF_MIN >= x)
                    && (leftBottomScreen.y - ENEMY_CREATE_DIFF_MIN <= y)
                    && (rightTopScreen.y + ENEMY_CREATE_DIFF_MIN >= y);
                   

            //isCheck = leftBottomScreen.x - ENEMY_CREATE_DIFF_MIN <= x ? rightTopScreen.x + ENEMY_CREATE_DIFF_MIN >= x
            //        : leftBottomScreen.y - ENEMY_CREATE_DIFF_MIN <= y ? rightTopScreen.y + ENEMY_CREATE_DIFF_MIN >= y
            //        : false;

        } while (isCheck);

        tempPos_x = x;
        tempPos_y = y;

        createEnemyPos = new Vector3((float)tempPos_x, (float)tempPos_y, 0);
    }


    /// <summary>
    /// ��������G�L������ݒ�
    /// </summary>
    private GameObject SetEnemy()
    {
        var totalRatio = TotalRatio();
        var createNum = Random.Range(1, totalRatio);

        var total = 0;
        for (int i = 0; i < enemyTypeCount; i++)
        {
            total += enemyEncounts[i];
            if (createNum <= total)
            {
                return enemyObjects[i];
            }
        }
        return null;
    }

    /// <summary>
    /// ���v�G���J�E���g��
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
    /// ��ʂɕ\��������ő吶�����`�F�b�N
    /// </summary>
    public bool IsCheckOver()
    {
        return enemysParent.childCount >= ENEMY_SCREEN_MAXCOUNT ? true : false;
    }

    /// <summary>
    /// �\������Ă�G�̍폜
    /// </summary>
    public void DeleteEnemys()
    {
        if (enemyAllObjects.Count == 0)
            return;

        foreach (var enemy in enemyAllObjects)
        {
            Destroy(enemy);
        }

        enemyAllObjects.Clear();
        enemyCreateCount = 0;
    }



    //���x���A�b�v���̍X�V����
    public void LevelUpdate(int level)
    {
        //createDelayTaskTime:�����Ԋu


        //�ŏ�����L���ɂ���
        //�Q�[�����x���ɂ���ēG�̐����Ԋu�ƁA�G�̎�ނ���ω�������B�F�����_����������H
        //�Q�[�����x���ɂ���čő吶������ω�������H
        //Unitask�Ő����HWhile�ŁH

    }
    //���g���C�p�̃��t���b�V���������K�v








    //�ŏ�����L���ɂ���
    //�Q�[�����x���ɂ���ēG�̐����Ԋu�ƁA�G�̎�ނ���ω�������B�F�����_����������H
    //�Q�[�����x���ɂ���čő吶������ω�������H
    //Unitask�Ő����HWhile�ŁH

}


