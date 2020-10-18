using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pin_S : MonoBehaviour
{
    [SerializeField]
    float rotation = 50;

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(0, 0, rotation * Time.deltaTime); //rotates "rotation" degrees per second around z axis
    }
}
