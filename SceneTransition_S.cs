using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneTransition_S : MonoBehaviour
{
    public bool canPass;
    public bool resetLevel;
    public Animator transitionAnim;
    public string overrideLevel;


    public void Update()
    {
        
    }

    

    public void SetCanPass(bool newCanPass)
    {
        canPass = newCanPass;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if (resetLevel)
            {
                StartCoroutine(LoadScene());
                MasterController_S.self.ResetScene();
            }
            else if (canPass)
            {
                StartCoroutine(LoadScene());
                if (overrideLevel.Equals(""))
                    MasterController_S.SceneLoader_S.LoadNextScreen();
                else
                MasterController_S.SceneLoader_S.LoadNextScreen(overrideLevel);
            }
            else
            {
                MasterController_S.player_S.RestartGame();
            }
        }

    }

    private void StartCoroutine(IEnumerable enumerable)
    {
        //throw new NotImplementedException();
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if (resetLevel)
            {
                MasterController_S.self.ResetScene();
            }
            else if (canPass)
                MasterController_S.SceneLoader_S.LoadNextScreen();
            else
            {
                MasterController_S.player_S.RestartGame();
            }
        }
    }

    IEnumerable LoadScene()
    {
        transitionAnim.SetTrigger("end");
        yield return new WaitForSeconds(1.5f);
    }
}
