using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class UIManager : MonoBehaviour
{
    [System.Serializable]
    public class SceneUI
    {
        public string sceneName; // シーン名
        public GameObject canvas; // 対応するCanvas
    }

    public SceneUI[] sceneUIs;
    public string persistentScene = "UIScene"; // 永続シーン名

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        Debug.Log($"Scene loaded: {scene.name}"); // デバッグ用
        bool foundMatchingCanvas = false;

        foreach (var sceneUI in sceneUIs)
        {
            if (sceneUI.canvas != null)
            {
                bool shouldBeActive = sceneUI.sceneName == scene.name;
                sceneUI.canvas.SetActive(shouldBeActive);

                if (shouldBeActive) foundMatchingCanvas = true;
            }
        }

        if (!foundMatchingCanvas)
        {
            Debug.LogWarning($"No matching Canvas found for scene: {scene.name}");
        }
    }

    void Start()
    {
        StartCoroutine(EnsureUISceneLoaded());

        // 現在のシーンに対応するCanvasを初期化
        var currentScene = SceneManager.GetActiveScene();
        OnSceneLoaded(currentScene, LoadSceneMode.Single);
    }

    private IEnumerator EnsureUISceneLoaded()
    {
        if (!SceneManager.GetSceneByName(persistentScene).isLoaded)
        {
            Debug.Log($"Loading persistent scene: {persistentScene}");
            AsyncOperation loadOperation = SceneManager.LoadSceneAsync(persistentScene, LoadSceneMode.Additive);

            while (!loadOperation.isDone)
            {
                yield return null;
            }
        }
        else
        {
            Debug.Log($"{persistentScene} is already loaded.");
        }
    }
}
