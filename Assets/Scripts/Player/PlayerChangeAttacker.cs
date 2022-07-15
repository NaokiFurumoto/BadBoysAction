using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// UŒ‚‚ğØ‚è‘Ö‚¦‚é
/// </summary>
public partial class PlayerMovement : MonoBehaviour
{
    /// <summary>
    /// UŒ‚ŠÇ—ƒNƒ‰ƒX
    /// </summary>
    [SerializeField]
    private AttackerManager attackerManager;

    /// <summary>
    /// Œü‚«‚Ìˆê‘Ş”ğ
    /// </summary>
    private (float, float) tempDirection = (0,0);

    /// <summary>
    /// UŒ‚•ûŒü‚ğØ‚è‘Ö‚¦‚é
    /// </summary>
    /// <param name="x"></param>
    /// <param name="y"></param>
    private void ChangeAttacker(float x, float y)
    {
        // 0,-1: Front
        // 0,1:  Up
        // 1,0 : Side
        // 1,-1: SideDown
        // 1,1 : SideUp  
        if (tempDirection.Item1 == x && tempDirection.Item2 == y)
        {
            return;
        }
        
        tempDirection = (x, y);
        if (x == 0 && y == -1)
        {
            attackerManager.ActivateAttacker(ATTACK_DIRECTION.FRONT);
            return;
        }

        if (x == 0 && y == 1)
        {
            attackerManager.ActivateAttacker(ATTACK_DIRECTION.UP);
            return;
        }

        if (x == 1 && y == 0)
        {
            attackerManager.ActivateAttacker(ATTACK_DIRECTION.SIDE);
            return;
        }

        if (x == 1 && y == -1)
        {
            attackerManager.ActivateAttacker(ATTACK_DIRECTION.SIDEDOWN);
            return;
        }

        if (x == 1 && y == 1)
        {
            attackerManager.ActivateAttacker(ATTACK_DIRECTION.SIDEUP);
            return;
        }

            attackerManager.ActivateAttacker(ATTACK_DIRECTION.NONE);
    }

}
