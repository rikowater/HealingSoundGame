using UnityEngine;
using UnityEngine.SceneManagement;

public class SyncDestroyWithSceneLoad : MonoBehaviour
{
    [SerializeField] private string targetSceneName;  // 監視対象のオブジェクトがいるシーン名
    [SerializeField] private string targetObjectName; // 監視対象のオブジェクト名

    private GameObject targetObject;

    private void Start()
    {
        // シーンが既にロード済みの場合に対象を検出
        if (IsSceneLoaded(targetSceneName))
        {
            FindTargetObjectInScene(targetSceneName);
        }
        else
        {
            // シーンのロード完了を待つ
            SceneManager.sceneLoaded += OnSceneLoaded;
        }
    }

    private void Update()
    {
        // 対象オブジェクトが破壊されていれば、自身も破壊
        if (targetObject == null && IsSceneLoaded(targetSceneName))
        {
            Destroy(gameObject);
        }
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // 監視対象のシーンがロードされたときに対象を検出
        if (scene.name == targetSceneName)
        {
            FindTargetObjectInScene(scene.name);
        }
    }

    private void FindTargetObjectInScene(string sceneName)
    {
        foreach (var scene in SceneManager.GetAllScenes())
        {
            if (scene.name == sceneName && scene.isLoaded)
            {
                targetObject = FindObjectInScene(scene, targetObjectName);
                break;
            }
        }
    }

    private GameObject FindObjectInScene(Scene scene, string objectName)
    {
        // シーン内のルートオブジェクトを取得して再帰的に検索
        GameObject[] rootObjects = scene.GetRootGameObjects();

        foreach (GameObject rootObject in rootObjects)
        {
            GameObject foundObject = SearchInHierarchy(rootObject, objectName);
            if (foundObject != null)
            {
                return foundObject;
            }
        }

        Debug.LogWarning($"監視対象 '{objectName}' がシーン '{scene.name}' に見つかりませんでした。");
        return null;
    }

    private GameObject SearchInHierarchy(GameObject parent, string objectName)
    {
        if (parent.name == objectName)
        {
            return parent;
        }

        foreach (Transform child in parent.transform)
        {
            GameObject foundObject = SearchInHierarchy(child.gameObject, objectName);
            if (foundObject != null)
            {
                return foundObject;
            }
        }

        return null;
    }

    private bool IsSceneLoaded(string sceneName)
    {
        foreach (var scene in SceneManager.GetAllScenes())
        {
            if (scene.name == sceneName && scene.isLoaded)
            {
                return true;
            }
        }

        return false;
    }

    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
}
