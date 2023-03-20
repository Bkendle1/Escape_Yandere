using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonSceneSwapper : MonoBehaviour
{
    [SerializeField] private string _sceneName = "SceneName";
    
    public void OnButtonPressed()
    {
        SceneSwapper.LoadScene(_sceneName);
    }
}
