using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class InputManager : MonoBehaviour
{
    #region 変数
    /// <summary>
    /// メインカメラ
    /// </summary>
    private Camera mainCamera;

    /// <summary>
    /// タッチの判定
    /// </summary>
    private bool touchFlag;

    /// <summary>
    /// タッチ位置
    /// </summary>
    [SerializeField]
    private Vector2 touchBeginPos, touchingPos, touchLastPos;

    /// <summary>
    /// タッチ状態
    /// </summary>
    private TouchPhase touchPhase;

    /// <summary>
    /// ゲームの状態
    /// </summary>
    [SerializeField]
    private GameController gameController;

    /// <summary>
    /// ポイントデータ
    /// </summary>
    private PointerEventData pointerEventData;
    #endregion

    #region プロパティ
    public bool TouchFlag           => touchFlag;
    public Vector2 TouchBeginPos    => touchBeginPos;
    public Vector2 TouchingPos      => touchingPos;
    public Vector2 TouchingLastPos  => touchLastPos;
    public TouchPhase TouchPhase    => touchPhase;

    public Vector2 DeltaPos => new Vector2(touchingPos.x - (touchBeginPos.x),
                                           touchingPos.y - (touchBeginPos.y));
  
    #endregion

    public static InputManager Instance;

    private void Awake()
    {
        Initialize();
    }

    /// <summary>
    /// 初期化
    /// </summary>
    private void Initialize()
    {
        mainCamera      = Camera.main;
        Instance        = this;
        touchFlag       = false;
        touchBeginPos   = touchingPos
                        = touchLastPos 
                        = Vector2.zero;
        touchPhase      = TouchPhase.Began;
        pointerEventData = new PointerEventData(EventSystem.current);
    }

    private void Update()
    {
        //ゲームプレイ中に実行させる
        //Editor
        if (Application.isEditor)
        {
            //押した瞬間
            if (Input.GetMouseButtonDown(0))
            {
                var hitobjects = GetObjectAll();
                if(hitobjects.Count > 0)
                {
                    foreach (RaycastResult obj in hitobjects)
                    {
                        if (obj.gameObject.CompareTag("Btn_UI"))
                        {
                            return;
                        }
                    }

                }

                touchFlag       = true;
                touchPhase      = TouchPhase.Began;
                touchBeginPos   = mainCamera.ScreenToWorldPoint(Input.mousePosition);
            }

            //離した瞬間
            if (Input.GetMouseButtonUp(0))
            {
                touchFlag       = false;
                touchPhase      = TouchPhase.Ended;
                touchLastPos    = touchingPos;
                touchBeginPos   = touchingPos
                                = Vector2.zero;
            }

            //押しっぱなし
            if (Input.GetMouseButton(0))
            {
                touchPhase    = TouchPhase.Moved;
                touchingPos   = mainCamera.ScreenToWorldPoint(Input.mousePosition);
            }
        }
        //else//端末
        //{
        //    //TODO:追加で必要そう
        //    if(Input.touchCount > 0)
        //    {
        //        Touch touch = Input.GetTouch(0);
        //        touchingPos = touch.position;
        //        touchPhase  = touch.phase;
        //        touchFlag   = true;
        //    }
        //}
    }

    /// <summary>
    /// ヒットしたオブジェクトを全て取得
    /// </summary>
    /// <returns></returns>
    public List<RaycastResult> GetObjectAll()
    {
        //RaycastAllの結果格納用List
        List<RaycastResult> RayResult = new List<RaycastResult>();

        //PointerEventDataにマウスの位置をセット
        pointerEventData.position = Input.mousePosition;

        //RayCast（スクリーン座標）
        EventSystem.current.RaycastAll(pointerEventData, RayResult);

        return RayResult;
    }
}
