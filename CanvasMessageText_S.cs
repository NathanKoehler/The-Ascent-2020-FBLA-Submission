using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CanvasMessageText_S : MonoBehaviour
{
    public Animator anim;
    public TextMeshProUGUI canvasMessageText;


    // Start is called before the first frame update
    void Start()
    {
        MasterController_S.canvasMessageText_S = this;
    }

    
    public void SendCanvasMessage(string newText)
    {
        anim.SetTrigger("Activate");
        canvasMessageText.text = newText;
    }
}
