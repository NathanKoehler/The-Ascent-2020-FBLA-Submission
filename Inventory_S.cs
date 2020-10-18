using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Inventory_S : MonoBehaviour
{
    public bool[] isFull;
    public GameObject[] slots;

    void Awake()
    {
        MasterController_S.inventory_S = this;
    }


    private void Start()
    {
        MasterController_S.LoadInventory();

        StartCoroutine(InformPlayerOfSave());
        
    }


    private void OnDestroy()
    {
        
    }


    IEnumerator InformPlayerOfSave()
    {
        yield return new WaitForSeconds(0.1f);
        MasterController_S.canvasMessageText_S.SendCanvasMessage("Progress Saved...");
    }
}
