using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[System.Serializable]
public class PlayerData_S
{
    public string level;
    public float health;
    public float money;
    public bool hasLumin;
    public bool hasClimb;

    public PlayerData_S (MasterController_S masterController_S)
    {
        level = SceneManager.GetActiveScene().name;
        health = masterController_S.health;
        money = masterController_S.money;
        hasLumin = masterController_S.hasLumin;
        hasClimb = masterController_S.hasClimb;
    }
}
