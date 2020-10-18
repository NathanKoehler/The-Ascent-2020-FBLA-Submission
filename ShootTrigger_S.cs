using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootTrigger_S : MonoBehaviour
{
    public void InvokeShoot()
    {
        MasterController_S.player_S.TestIfCanAttack();
    }
}
