using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static GlobalValue;
/// <summary>
/// プレーヤーから一定時間逃げる
/// </summary>
public enum ESCAPE_STATE
{
    WALK,//歩く
    ESCAPE,//逃げる
    IDLE,//待機
}
public class EnemyEscapeMove : EnemyMovement
{
    /// <summary>
    /// 状態
    /// </summary>
    [SerializeField]
    private ESCAPE_STATE state;

    /// <summary>
    /// 待機開始時間
    /// </summary>
    private float idleStartTime;

    /// <summary>
    /// 追いかけ開始時間
    /// </summary>
    private float escapeStartTime;

    private Rigidbody2D rigid;

    /// <summary>
    /// //////////////////////////////////////////////////////////////////////////
    /// </summary>
    Vector2 A, C, AB, AC;
    Vector2 move = new Vector2(1, 0); // 進む方向
    float speed = 5f; // 動くスピード
    float arot = 0; // 自分の角度

    float Maxkaku = 0.05f; // 曲がる最大角度
    public float rotation; // 曲がる角度

    private GameObject target; // 離れる対象


    protected override void Initialize()
    {
        base.Initialize();
        target = playerCenter.gameObject;
        rigid = this.gameObject.GetComponent<Rigidbody2D>();    
        state = ESCAPE_STATE.WALK;
    }



    /// <summary>
    /// 移動処理
    /// </summary>
    protected override void TypeMove()
    {
        //自分の位置
        A = new Vector2(enemyTrans.position.x, enemyTrans.position.y); 
        //プレイヤーの位置
        C = new Vector2(target.transform.position.x, target.transform.position.y); 

        //移動方向
        AB = new Vector2(move.x,move.y);
        //ターゲットのベクトル
        AC = C - A;

        //なす角を求める
        //内積
        float dot = AB.x * AC.x + AB.y * AC.y;

        // アークコサインを使って内積とベクトルの長さから角度を求める
        float r = Acosf(dot / ((float)length(AB) * (float)length(AC)));


        // 曲がる方向を決める
        if (AB.x * AC.y - AB.y * AC.x < 0)
        {
            r = -r;
        }

        r = r * 180 / Mathf.PI; // ラジアンから角度に

        // 回転角度制御
        if (r > Maxkaku)
        {
            r = Maxkaku;
        }
        if (r < -Maxkaku)
        {
            r = -Maxkaku;
        }


        rotation = r; // 曲がる角度を入れる


        Move();


        //if (enemyMoveType != ENEMY_MOVETYPE.ESCAPE)
        //    return;

        //if (Time.time - lastFollowTime > turningTimeDelay)
        //{
        //    playerLastPos = playerCenter.position;
        //    lastFollowTime = Time.time;
        //}

        //if (state == ESCAPE_STATE.WALK)
        //{
        //    movePos = (playerLastPos - enemyTrans.position).normalized * chaseSpeed;
        //    if (Vector3.Distance(enemyTrans.position, playerLastPos) < 3.0f)
        //    {
        //        state = ESCAPE_STATE.ESCAPE;
        //        movePos = Vector3.zero;
        //        escapeStartTime = Time.time;
        //    }
        //}

        //if (state == ESCAPE_STATE.ESCAPE)
        //{
        //   // chaseSpeed *= -2.0f;
        //}





        //BaseMoving(movePos.x, movePos.y);
    }

    /// <summary>
    /// 移動処理
    /// </summary>
    protected  void TypeMoveer()
    {

       
        if (Time.time - lastFollowTime > turningTimeDelay)
        {
            playerLastPos = playerCenter.position;
            lastFollowTime = Time.time;
        }

        if (state == ESCAPE_STATE.WALK)
        {
            movePos = (playerLastPos - enemyTrans.position).normalized * chaseSpeed;
            if (Vector3.Distance(enemyTrans.position, playerLastPos) < 3.0f)
            {
                state = ESCAPE_STATE.ESCAPE;
                movePos = Vector3.zero;
                escapeStartTime = Time.time;
            }
        }

        if (state == ESCAPE_STATE.ESCAPE)
        {
            // chaseSpeed *= -2.0f;
        }





        BaseMoving(movePos.x, movePos.y);
    }

    //-------------------------------------------------
    // オブジェクトの移動処理
    //-------------------------------------------------
    void Move()
    {
        float rot = rotation; // 曲がる角度

        float tx = move.x, ty = move.y;

        move.x = tx * Mathf.Cos(rot) - ty * Mathf.Sin(rot);
        move.y = tx * Mathf.Sin(rot) + ty * Mathf.Cos(rot);

        arot = Mathf.Atan2(move.x, move.y); // 移動量から角度を求める
        float kaku = arot * 180.0f / Mathf.PI *-1 + 90; // ラジアンから角度に

        rigid.velocity = new Vector2(move.x, move.y) * speed; // 移動(最後のー1をかけている所を消すとプレイヤーを追いかけます)
        //transform.rotation = Quaternion.Euler(0, 0, kaku); // 回転

    }

    /// <summary>
    /// ベクトルの長さを求める
    /// </summary>
    /// <param name="vec">2点間のベクトル</param>
    /// <returns>ベクトルの長さを返す</returns>
    public float length(Vector2 vec)
    {
        return Mathf.Sqrt(vec.x * vec.x + vec.y * vec.y);
    }

    /// <summary>
    /// 長さが+-1を越えたとき1に戻す処理
    /// </summary>
    /// <param name="a">内積 / ベクトルの長さの答</param>
    /// <returns></returns>
    public float Acosf(float a)
    {
        if (a < -1) a = -1;
        if (a > 1) a = 1;

        return (float)Mathf.Acos(a);
    }
}
