using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KillFloor_S : Collectable_S
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public override void TriggerCollect(Player_S player_S)
    {
        Debug.Log("Player Injured!");
        MasterController_S.self.ChangeHealth(-1, true);
    }
}
