using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;
using UnityEngine.SceneManagement;

public class LoadingPanel : NetworkSceneManagerBase
{
    [SerializeField] private GameObject loadingPanel;
    private void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
    }

    protected override IEnumerator SwitchScene(SceneRef prevScene, SceneRef newScene, FinishedLoadingDelegate finished)
    {   
        if(SceneManager.GetActiveScene().buildIndex != (int)MapIndex.GameMap)
        {
            Debug.Log($"Switching Scene from {prevScene} to {newScene}");
            loadingPanel.SetActive(true);
            List<NetworkObject> sceneObjects = new List<NetworkObject>();
            string path;
            switch ((MapIndex)(int)newScene)
            {
                case MapIndex.RoomList: path = "1.RoomList"; break;
                case MapIndex.Lobby: path = "2.Lobby"; break;
                default: path = "Main"; break;
            }
            yield return SceneManager.LoadSceneAsync(path, LoadSceneMode.Single);
            var loadedScene = SceneManager.GetSceneByName(path);
            Debug.Log($"Loaded scene {path}: {loadedScene}");
            sceneObjects = FindNetworkObjects(loadedScene, disable: false);

            yield return null;
            finished(sceneObjects);

            Debug.Log($"Switched Scene from {prevScene} to {newScene} - loaded{sceneObjects.Count} scene objects");

            loadingPanel.SetActive(false);
        }

    }

}
