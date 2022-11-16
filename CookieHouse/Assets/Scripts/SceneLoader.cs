using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{

    [SerializeField]
    private string LoadScene;

    public void loadScene(string scene) {
        if(NetworkManager.FindInstance() != null)
        {
            NetworkManager manager = NetworkManager.FindInstance();
            manager.Disconnect();
        }
        SceneManager.LoadScene(scene);
    }

    public void exitGame()
    {
        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
        #else
            Application.Quit();
        #endif
    }
}
