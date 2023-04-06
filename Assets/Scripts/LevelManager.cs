using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    public GameObject w;
    public static GameObject win;

    private void Start()
    {
        win = w;
    }
    public static void LoadLevelWithName(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

    public static void Won()
    {
        Timer.StopTimer();
        //var a = GameObject.FindGameObjectsWithTag("Finish");
        win.SetActive(true);
    }

    public void Quit()
    {
        Application.Quit();
    }

    public static void LoadNextLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
}
