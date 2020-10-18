using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIMoney_S : MonoBehaviour
{
    [SerializeField]
    private Text _UIText;
    [SerializeField]
    private float money = 0;

    // Start is called before the first frame update
    void Start()
    {
        _UIText.text = "Pins Collected: " + money;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ChangeMoneyUI(float amount)
    {
        money += amount;
        if (money < 0)
        {
            money = MasterController_S.self.money;
        }
        _UIText.text = "Pins Collected: " + money;
    }
}
