using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    [SerializeField] private string startScene;

    public void LoadNextScene()
    {
        SceneManager.LoadScene(DDestroyOnLoad.getInstance().NextLevelName);
    }
    public void LoadPriviousScene()
    {
        SceneManager.LoadScene(DDestroyOnLoad.getInstance().ThisLevelName);
    }

    public void StartGame()
    {
        SceneManager.LoadScene(startScene);
    }
    public void Quit()
    {
        Application.Quit();
    }
}
