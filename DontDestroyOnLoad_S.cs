using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DontDestroyOnLoad_S : MonoBehaviour
{
    [SerializeField]
    private DontDestroyOnLoad_S self;


    void Awake()
    {
        if (self == null)
        {
            self = this;
            DontDestroyOnLoad(gameObject); // Basic method to remain even after scene load
        }
        else Destroy(gameObject);
    }
}
