using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Rendering;

public class Canvas_S : MonoBehaviour
{
    public static Canvas_S self;
    public static UIMoney_S _UIMoney_S;
    public static UIHealth_S _UIHealth_S;

    private Renderer rend;


    // Start is called before the first frame update
    void Awake()
    {
        rend = GetComponent<Renderer>();

        _UIMoney_S = GetComponentInChildren<UIMoney_S>();
        _UIHealth_S = GetComponentInChildren<UIHealth_S>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void HideCanvas(bool active)
    {
        if (active)
            rend.enabled = false;
        else
            rend.enabled = true;
    }
}
