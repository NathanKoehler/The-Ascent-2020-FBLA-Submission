using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Parallax_S : MonoBehaviour
{
    public GameObject cam;

    public float parallaxEffect;

    private float length;
    private float startPosX;
    

    // Start is called before the first frame update
    void Start()
    {
        startPosX = transform.position.x;
        length = GetComponent<SpriteRenderer>().bounds.size.x;
    }

    private void FixedUpdate()
    {
        float temp = (cam.transform.position.x * (1 - parallaxEffect));
        float dist = (cam.transform.position.x * parallaxEffect);
        transform.position = new Vector3(startPosX + dist, transform.position.y, transform.position.z);
        if (temp > startPosX + length)
        {
            startPosX += length;
        }
        else if (temp < startPosX - length)
        {
            startPosX -= length;
        }
    }
}
