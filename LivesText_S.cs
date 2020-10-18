using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class LivesText_S : MonoBehaviour
{
    public TextMeshProUGUI goGUI;


    // Start is called before the first frame update
    void Start()
    {
        goGUI.text = ("LIVES LEFT: " + MasterController_S.self.lives);
    }
}
