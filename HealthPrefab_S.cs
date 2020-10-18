using UnityEngine.Experimental.Rendering.Universal;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthPrefab_S : MonoBehaviour
{
    [HideInInspector]
    public HealthItem_S healthItem;
    [HideInInspector]
    public int order;
    public Light2D light2D;
    public float minimum = 0;
    public float maximum = 1f;

    private float t = 0;
    private float constant = 0.5f;

    IEnumerator Glow(bool destroy, Light2D glowLight)
    {

        t = 0.0001f;
        while (t > 0f)
        {
            glowLight.intensity = Mathf.Lerp(minimum, maximum, t);
            t += constant * Time.deltaTime;
            if (t > 1f)
            {
                constant = -constant;
            }
            yield return new WaitForEndOfFrame();
        }
        t = 0f;
        constant = -constant;
        if (destroy)
            Destroy(gameObject);
    }























    public void AssignHealthItem(HealthItem_S newHealthItem)
    {
        healthItem = newHealthItem;

        order = healthItem.order;
        GetComponent<Image>().sprite = healthItem.sprite;
    }

    public void InstantiateGlow(bool destroy)
    {
        if (destroy)
            Destroy(gameObject);

        //StartCoroutine(Glow(destroy, light2D));
    }


}
