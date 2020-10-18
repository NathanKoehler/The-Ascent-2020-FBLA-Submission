using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MainMenu_S : MonoBehaviour
{
    public GameObject playGameButton;
    public GameObject continueButton;
    public GameObject controlsButton;


    // Start is called before the first frame update
    void Start()
    {
        if (PlayerPrefs.GetFloat("HasSave", 0f) > 0.0f)
        {
            //Debug.Log("True");
            RectTransform controlsRect = controlsButton.GetComponent<RectTransform>();
            controlsRect.localPosition = new Vector2(20.45f, -180f);
            continueButton.SetActive(true);
        }
        else
        {
            GameObject.FindGameObjectWithTag("Save").SetActive(false); // Sets the fire
        }
    }


    public void LoadFromSave()
    {
        MasterController_S.self.LoadSaved();
    }

    
    public void DeleteGameSave()
    {
        PlayerPrefs.SetFloat("HasSave", 0f); // Prevents the player from accessing save
        continueButton.SetActive(false);
        RectTransform controlsRect = controlsButton.GetComponent<RectTransform>();
        controlsRect.localPosition = new Vector2(20.45f, -101f);
    }
}
