using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Ammo_S : MonoBehaviour
{
    public static bool exists;
    public static float ammoFloat;
    public static string ammoText;

    [SerializeField]
    private Image sliderBackground;
    [SerializeField]
    private Image sliderImage;
    [SerializeField]
    private Slider slider;
    [SerializeField]
    private Text ammo;

    private void Start()
    {
        exists = true;
    }

    private void Update()
    {
        slider.value = ammoFloat;
        ammo.text = ammoText;
        sliderImage.color = Color.Lerp(Color.red, Color.blue, ammoFloat / 15);
        ammo.color = Color.Lerp(Color.red, Color.blue, ammoFloat / 15);
        sliderBackground.color = Color.Lerp(Color.black, Color.grey, ammoFloat / 15);

        if (ammoFloat == 0)
        {
            MasterController_S.self.KillAmmoCanvasObject(this);
        }
    }

    private void OnDestroy()
    {
        exists = false;
    }
}
