using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIHealth_S : MonoBehaviour
{
    public GameObject healthSprite;
    public GameObject healthSprite_half;
    public GameObject healthSprite_broken;

    public List<HealthItem_S> refHealthItems = new List<HealthItem_S>();

    public static bool[] roleCall;
    public List<HealthPrefab_S> healthPrefabs = new List<HealthPrefab_S>();

    private int lastHealthRecalled;

    private void Awake()
    {
        roleCall = new bool[refHealthItems.Count];
    }

    // Start is called before the first frame update
    void Start()
    {
        SetupHealth();
        ActivateHealth(true);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // ** Methods **
    private void SetupHealth()
    {
        GameObject healthObj;

        int health = Mathf.RoundToInt(MasterController_S.self.health);
        lastHealthRecalled = health;

        for (int i = 0; i < health; i++)
        {
            healthObj = Instantiate(healthSprite, transform);
            healthObj.GetComponent<HealthPrefab_S>().AssignHealthItem(refHealthItems[i]);
            healthPrefabs.Add(healthObj.GetComponent<HealthPrefab_S>());
            roleCall[i] = true;
        }
    }


    public void ActivateHealth(bool enable)
    {
        foreach (HealthPrefab_S healthPrefab in healthPrefabs)
        {
            healthPrefab.gameObject.GetComponent<Image>().enabled = enable;
        }
    }


    public void SetHealth()
    {
        int health = Mathf.RoundToInt(MasterController_S.self.health);
        int healthChange = lastHealthRecalled - Mathf.RoundToInt(MasterController_S.self.health);

        if (healthChange > 0)
        {
            for (int i = lastHealthRecalled; i > health; i--)
            {
                int num = Random.Range(1, health);
                HealthPrefab_S healthPrefab = healthPrefabs[num];
                roleCall[healthPrefab.order] = false;
                healthPrefabs.RemoveAt(num);
                healthPrefab.InstantiateGlow(true);
            }
        }
        else
        {
            for (int i = 0; i < roleCall.Length; i++)
            {
                if (healthChange < 0 && !roleCall[i])
                {
                    GameObject healthObj = Instantiate(healthSprite, transform);
                    HealthPrefab_S healthPrefab = healthObj.GetComponent<HealthPrefab_S>();
                    healthPrefab.AssignHealthItem(refHealthItems[i]);
                    healthPrefab.InstantiateGlow(false);
                    healthPrefabs.Add(healthPrefab);
                    MasterController_S.canvasMessageText_S.SendCanvasMessage("Collected New " + refHealthItems[i].itemName);
                    roleCall[i] = true;
                }
            }
        }

        lastHealthRecalled = health;
    }

    public void SetHealth(bool add, int order)
    {
        lastHealthRecalled = Mathf.RoundToInt(MasterController_S.self.health);

        if (add)
        {
            GameObject healthObj = Instantiate(healthSprite, transform);
            healthObj.GetComponent<HealthPrefab_S>().AssignHealthItem(refHealthItems[order]);
            healthPrefabs.Add(healthObj.GetComponent<HealthPrefab_S>());
            MasterController_S.canvasMessageText_S.SendCanvasMessage("Collected New " + refHealthItems[order].itemName);
            roleCall[order] = true;
        }
        else
        {
            HealthPrefab_S healthPrefab = healthPrefabs[order];
            roleCall[healthPrefab.order] = false;
            healthPrefabs.RemoveAt(order);
            MasterController_S.canvasMessageText_S.SendCanvasMessage("Lost " + refHealthItems[order].itemName);
            Destroy(healthPrefab.gameObject);
        }
    }
}
