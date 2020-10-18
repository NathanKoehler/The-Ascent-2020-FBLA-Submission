using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoneyCollectable_S : Collectable_S
{
    public float amount;

    public override void TriggerCollect(Player_S player_S)
    {
        SoundManager_S.PlaySound("pin");
        MasterController_S.self.ChangeMoneyAmount(amount);
        Destroy(gameObject);
    }
}
