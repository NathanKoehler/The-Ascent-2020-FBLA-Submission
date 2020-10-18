using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader_S : MonoBehaviour
{

    private void Start()
    {
        MasterController_S.SceneLoader_S = this;
    }

    public void LoadMainMenu()
    {
        MasterController_S.self.ResetVariables();
        SceneManager.LoadScene("Main Menu");
    }

    public void LoadNextScreen()
    {
        int currSceneIndex = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(currSceneIndex + 1, LoadSceneMode.Single);
    }


    public void LoadNextScreen(string sceneName)
    {
        SceneManager.LoadScene(sceneName, LoadSceneMode.Single);
    }


    public void QuitGame()
    {
        Application.Quit();
    }

    public void EndLifeScreen()
    {
        SceneManager.LoadScene("Life Over", LoadSceneMode.Single);
    }

    public void EndGameScreen()
    {
        SceneManager.LoadScene("Game Over", LoadSceneMode.Single);
    }

}
