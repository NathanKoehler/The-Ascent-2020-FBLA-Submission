using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

public class Portal_S : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Light2D emission = GetComponent<Light2D>();
        Light2D emissionOther = GetComponentInChildren<Light2D>();

        StartCoroutine(Illuminate(emission, emissionOther));
    }

    IEnumerator Illuminate(Light2D newEmmission, Light2D newEmissionOther)
    {
        float emissionMax = newEmmission.intensity;
        float emissionOtherMax = newEmissionOther.intensity;
        float num = 0;

        newEmmission.intensity = 0;
        newEmissionOther.intensity = 0;

        while (newEmmission.intensity < emissionMax && newEmissionOther.intensity < emissionOtherMax)
        {
            newEmmission.intensity = Mathf.Lerp(0, emissionMax, num);
            newEmissionOther.intensity = Mathf.Lerp(0, emissionOtherMax, num);
            num += Time.deltaTime * 0.5f;
            yield return null;
        }

        LightFlicker_S newComponent = GetComponentInChildren<LightFlicker_S>();

        newComponent.enabled = true;
        emissionOtherMax = newComponent.maxIntensity;
        newComponent.maxIntensity = newComponent.minIntensity;
        num = 0;

        while (newComponent.maxIntensity < emissionOtherMax)
        {
            newComponent.maxIntensity = Mathf.Lerp(0, emissionOtherMax, num);
            num += Time.deltaTime * 0.5f;
            yield return null;
        }
    }
}
