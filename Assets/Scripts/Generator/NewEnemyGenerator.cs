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

    [SerializeField]
    private UiController uiController;

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

    /// <summary>
    /// �X�N���[���T�C�Y
    /// </summary>
    private Vector3 rightTopScreen;
    private Vector3 leftBottomScreen;

    /// <summary>
    /// �������̑S�ẴI�u�W�F�N�g
    /// </summary>
    private List<GameObject> enemyAllObjects = new List<GameObject>();
    #endregion


    #region �����@�Ɋւ���
    /// <summary>
    /// �������
    /// </summary>
    [SerializeField]
    private GENERATOR_STATE state;

    /// <summary>
    /// �����Ԋu���ԁF���x���ɂ���ĕω�������
    /// </summary>
    [SerializeField]
    private float createDelayTime;

    /// <summary>
    /// �G��ʕ\�����F���x���ɂ���ĕω�������
    /// </summary>
    [SerializeField]
    private int enemyScreenDisplayIndex;

    /// <summary>
    /// �����x���J�n����
    /// </summary>
    private int startDelayTaskTime;

    /// <summary>
    /// �G�G���J�E���g�������l
    /// </summary>
    private List<int> EncountClearList = new List<int>();

    /// <summary>
    /// �v�����ԁFTime.deltaTime�ŉ��Z���Ă���
    /// </summary>
    private float progressTime;
    #endregion

    #region �v���p�e�B
    public GENERATOR_STATE State { get { return state; } set { state = value; } }
    public List<GameObject> EnemyAllObjects => enemyAllObjects;
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
        enemyTypeCount = enemyObjects.Count;

        //�����@
        state = GENERATOR_STATE.STOP;
        progressTime = 0.0f;
        startDelayTaskTime = START_CREATE_DIFF;
        EncountClearList = enemyEncounts;
    }

    /// <summary>
    /// �J�n���̕ω�������l
    /// </summary>
    public void InitializeData()
    {
        //���x���ɂ���ĕω�������
        createDelayTime = FIRST_CREATETIME;
        enemyScreenDisplayIndex = ENEMY_SCREEN_MAXCOUNT;
    }

    /// <summary>
    /// ���[�h���̕ω�������l
    /// </summary>
    public void InitializeLoadedData()
    {
        //���x���ɂ���ĕω�������
        createDelayTime = GetCreateDelayTime();
        enemyScreenDisplayIndex = GetEnemyScreenDisplayIndex();
    }

    /// <summary>
    /// �O������������
    /// </summary>
    private void Start() { InitializeOther(); }
    private void InitializeOther() 
    {
        if (gameController == null)
        {
            gameController = GameObject.FindGameObjectWithTag("GameController")
                                   .GetComponent<GameController>();
        }

        if (uiController == null)
        {
            uiController = GameObject.FindGameObjectWithTag("UI")
                                   .GetComponent<UiController>();
        }
    }

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
    private void AddEnemyEncount()
    {
        var list = enemyEncounts.Select(x => x + 1).ToList();
        enemyEncounts = list;
    }

    /// <summary>
    /// ��ʂɕ\��������ő吶�����`�F�b�N
    /// </summary>
    public bool IsCheckOver()
    {
        return enemysParent.childCount >= enemyScreenDisplayIndex ? true : false;
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
    public void LevelUpdate()
    {
        //�����Ԋu�X�V
        createDelayTime -= CREATE_TIMEDIFF;

        if(createDelayTime <= 0.01f)
        {
            createDelayTime = LAST_CREATETIME;
        }

        enemyScreenDisplayIndex += ENEMY_SCREEN_ADDCOUNT;
        AddEnemyEncount();
    }

    /// <summary>
    /// ���g���C��̍X�V����
    /// </summary>
    public void RetryInitialize()
    {
        progressTime = 0.0f;
        enemyEncounts = EncountClearList;
        createDelayTime = FIRST_CREATETIME;
        enemyScreenDisplayIndex = ENEMY_SCREEN_MAXCOUNT;
    }

    #region get/Set
    /// <summary>
    /// ���x���A�b�v�p�J�E���g��
    /// </summary>
    /// <returns></returns>
    public float GetCreateDelayTime()
    {
        return createDelayTime;
    }

    /// <summary>
    /// �K�v�o���l
    /// </summary>
    /// <returns></returns>
    public int GetEnemyScreenDisplayIndex()
    {
        return enemyScreenDisplayIndex;
    }

    /// <summary>
    /// ���x���A�b�v�p�J�E���g��
    /// </summary>
    /// <returns></returns>
    public void SetCreateDelayTime(float count)
    {
        createDelayTime = count;
    }

    /// <summary>
    /// �K�v�o���l
    /// </summary>
    /// <returns></returns>
    public void SetEnemyScreenDisplayIndex(int count)
    {
        enemyScreenDisplayIndex = count;
    }
    #endregion

}


