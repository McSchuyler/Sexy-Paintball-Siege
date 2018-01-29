using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour {

    public string mainSceneName;

	public void StartGame()
    {
        SceneManager.LoadScene(mainSceneName,LoadSceneMode.Single);
    }

    public void ExitGame()
    {
        Application.Quit();
    }
}
