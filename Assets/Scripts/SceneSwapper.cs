using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public static class SceneSwapper
{
    public static void LoadScene(string sceneName)
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(sceneName);
    }

    public static void LoadScene(ref string sceneName)
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(sceneName);
    }

    public static void Initialize()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    public static void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        
    }
}
