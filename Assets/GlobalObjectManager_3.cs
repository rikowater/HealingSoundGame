using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GlobalObjectManager_3 : MonoBehaviour
{
    // シーン間で共有するオブジェクトの参照
    public GameObject sharedObject;  // ← これが定義されていることを確認

    [SerializeField] private List<string> activeScenes;

    private void Awake()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
        CheckSceneActive();
    }

    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        CheckSceneActive();
    }

    private void CheckSceneActive()
    {
        string currentScene = SceneManager.GetActiveScene().name;
        if (sharedObject != null)
        {
            sharedObject.SetActive(activeScenes.Contains(currentScene));
        }
    }

}
