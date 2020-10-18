using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class WinScreen_S : MonoBehaviour
{
    public TextMeshProUGUI qaText;
    public TextMeshProUGUI qcText;
    public TextMeshProUGUI qiText;
    public TextMeshProUGUI piText;


    // Start is called before the first frame update
    void Start()
    {
        MasterController_S.self.WinScreenActivate(this);
    }
}
