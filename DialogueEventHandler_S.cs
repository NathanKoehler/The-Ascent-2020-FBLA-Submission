using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueEventHandler_S : MonoBehaviour
{
    public GameObject healthPickup;
    public GameObject pampletPickup;
    public GameObject moneyExplosion;

    public float moneyAmount = 50f;

    public Vector3 checkpointOffset = new Vector2(0f, 2f);


    public void SetPlayerCheckpoint()
    {
        MasterController_S.player_S.AddCheckpoint(transform.position + checkpointOffset);
    }


    public void RewardPlayer(string type, Transform location, int count)
    {
        if (type.Equals("HEALTH"))
        {
            for (int i = 0; i < count; i++)
            {
                GameObject newHealthPickup = Instantiate(healthPickup, new Vector2(location.position.x, location.position.y + 1f), location.rotation);
            }
        }
        else if (type.Equals("MONEY"))
        {
            for (int i = 0; i < count; i++)
            {
                SoundManager_S.PlaySound("pin");
                MasterController_S.self.ChangeMoneyAmount(moneyAmount);
                Instantiate(moneyExplosion, new Vector2(location.position.x, location.position.y + 1f), location.rotation);
            }
        }
        else if (type.Equals("MONEY 2"))
        {
            for (int i = 0; i < count; i++)
            {
                SoundManager_S.PlaySound("pin");
                MasterController_S.self.ChangeMoneyAmount(moneyAmount * 2f);
                Instantiate(moneyExplosion, new Vector2(location.position.x, location.position.y + 1f), location.rotation);
            }
        }
        else if (type.Equals("MONEY 4"))
        {
            for (int i = 0; i < count; i++)
            {
                SoundManager_S.PlaySound("pin");
                MasterController_S.self.ChangeMoneyAmount(moneyAmount * 4f);
                Instantiate(moneyExplosion, new Vector2(location.position.x, location.position.y + 1f), location.rotation);
            }
        }
        else if (type.Equals("AMMO"))
        {
            for (int i = 0; i < count; i++)
            {
                GameObject newPampletPickup = Instantiate(pampletPickup, new Vector2(location.position.x, location.position.y + 1f), location.rotation);
            }
        }
    }
}
