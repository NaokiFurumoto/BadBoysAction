using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using static GlobalValue;

public class CameraAction : MonoBehaviour
{
    /// <summary>
    /// 敵にダメージを与えたときに行う演出
    /// </summary>
    public static void EnemyDamage()
    {
        Camera.main.gameObject.transform.DOShakePosition(
            0.1f,
            0.1f
            ).OnComplete(() =>
            {
                Camera.main.transform.position = CAMERA_INITPOS;
            });
    }

    /// <summary>
    /// プレイヤーがダメージを受けたときに行う演出
    /// </summary>
    public static void PlayerDamage()
    {
        DOTween.To
            (
                () => Camera.main.orthographicSize,
                (n) => Camera.main.orthographicSize = n,
                7.5f,
                0.1f
            ).OnComplete(() =>
            {
                Camera.main.orthographicSize = CAMERA_INITSIZE;
            });
    }
}
