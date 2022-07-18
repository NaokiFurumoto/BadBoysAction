using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// �G�ړ��N���X
/// </summary>
public class EnemyMovement : MonoBehaviour
{
    #region �ϐ�
    /// <summary>
    /// �ړ��X�s�[�h
    /// </summary>
    [SerializeField]
    protected float xSpeed = 1.5f, ySpeed = 1.5f;

    /// <summary>
    /// �ړ���
    /// </summary>
    private Vector2 moveDelta;

    /// <summary>
    /// �^�[�Q�b�g�𔭌����Ă邩�̔���
    /// </summary>
    private bool hasPlayerTarget;

    /// <summary>
    /// �v���C���[���
    /// </summary>
    private Transform player;
    private Vector3 playerLastPos;
        
    /// <summary>
    /// ���g�̏��
    /// </summary>
    private Vector3 startPos, movePos;

    /// <summary>
    /// �ǂ�������X�s�[�h
    /// </summary>
    [SerializeField]
    private float chaseSpeed = 0.8f;

    /// <summary>
    /// ��]�̒x��
    /// </summary>
    [SerializeField]
    private float turningDelay = 1f;

    /// <summary>
    /// ���ɕ����]���\�Ȏ���
    /// </summary>
    [SerializeField]
    private float turningTimeDelay = 1f;

    /// <summary>
    /// �v���C���[�̈ʒu���Ō�ɔc����������
    /// </summary>
    private float lastFollowTime;

    /// <summary>
    /// �����ۑ��p
    /// </summary>
    private Vector3 tempScale;

    /// <summary>
    /// �G�̃A�j���[�^�[
    /// </summary>
    [SerializeField]
    private Animator animator;

    private EnemyStatusController enemyController;
    #endregion

    #region �v���p�e�B
    public Vector2    MoveDelta       => moveDelta;
    public bool       HasPlayerTarget => hasPlayerTarget;
    public GameObject PlayerObject    => player.gameObject;
    #endregion

    private void Start()
    {
        player            = GameObject.FindWithTag("Player").transform;
        playerLastPos     = player.position;
        startPos          = transform.position;
        lastFollowTime    = Time.time;
        turningTimeDelay *= turningDelay;
        enemyController   = GetComponent<EnemyStatusController>();
    }

    /// <summary>
    /// ������
    /// </summary>
    private void Initialize()
    {

    }
    /// <summary>
    /// �G���ړ�������
    /// </summary>
    /// <param name="x"></param>
    /// <param name="y"></param>
    protected virtual void EnemyMove(float x, float y)
    {
        moveDelta = new Vector2(x * xSpeed, y * ySpeed);
        transform.Translate(moveDelta.x * Time.deltaTime,
                            moveDelta.y * Time.deltaTime, 0);
    }
}
