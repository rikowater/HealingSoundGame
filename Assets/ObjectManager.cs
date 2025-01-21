using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ObjectManager : MonoBehaviour
{
    public string targetSceneName; // 対象シーン名
    public string targetObjectName; // 対象オブジェクトの名前
    public Vector3 startPosition;
    public Vector3 endPosition;
    public float transitionSpeed = 5.0f;

    private GameObject targetObject;
    private Coroutine currentCoroutine;
    private bool ObjectActive = false; // メニューの状態を追跡

    private bool isSceneLoaded = false;

    private void OnEnable()
    {
        // シーンロードイベントを登録
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        // シーンロードイベントを解除
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    public void ToggleMenu()
    {
        if (targetObject == null)
        {
            Debug.LogWarning("Target object is not loaded yet!");
            return;
        }

        if (currentCoroutine != null)
        {
            StopCoroutine(currentCoroutine);
        }

        if (ObjectActive)
        {
            currentCoroutine = StartCoroutine(MoveToPosition(startPosition));
        }
        else
        {
            currentCoroutine = StartCoroutine(MoveToPosition(endPosition));
        }

        ObjectActive = !ObjectActive; // 状態をトグル
    }

    private IEnumerator MoveToPosition(Vector3 targetPosition)
    {
        while (Vector3.Distance(targetObject.transform.position, targetPosition) > 0.01f)
        {
            targetObject.transform.position = Vector3.Lerp(targetObject.transform.position, targetPosition, Time.deltaTime * transitionSpeed);
            yield return null;
        }
        targetObject.transform.position = targetPosition;
    }

    private void Start()
    {
        // 既にロード済みなら検索
        FindTargetObject();
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == targetSceneName)
        {
            isSceneLoaded = true;
            FindTargetObject();
        }
    }

    private void FindTargetObject()
    {
        // シーンがロードされているか確認
        Scene targetScene = SceneManager.GetSceneByName(targetSceneName);
        if (!targetScene.IsValid() || !targetScene.isLoaded)
        {
            Debug.LogWarning($"Scene '{targetSceneName}' is not loaded yet!");
            return;
        }

        GameObject[] sceneObjects = targetScene.GetRootGameObjects();
        foreach (GameObject obj in sceneObjects)
        {
            if (obj.name == targetObjectName)
            {
                targetObject = obj;
                Debug.Log($"Target object '{targetObjectName}' found in scene '{targetSceneName}'.");
                break;
            }
        }

        if (targetObject == null)
        {
            Debug.LogError($"Target object '{targetObjectName}' not found in scene '{targetSceneName}'!");
        }
    }
}
