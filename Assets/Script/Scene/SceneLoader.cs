using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public static class SceneLoader
{
    private static string sceneToLoad;
    public static string SceneToLoad {get => sceneToLoad;}

    const string PROGRESS_SCENE = "Loading";

    public static void ProgressLoad(string sceneName)
    {
        sceneToLoad = sceneName;
        SceneManager.LoadScene(PROGRESS_SCENE);
    }

    public static void Load(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

    public static void Reload()
    {
        var currentScene = SceneManager.GetActiveScene().name;
        ProgressLoad(currentScene);
    }
}
