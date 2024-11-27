using UnityEngine;
using UnityEngine.SceneManagement;

public class ItemManager : MonoBehaviour
{
    public string targetSceneName;  // 監視対象のオブジェクトがいるシーン名
    public string targetObjectName; // 監視対象のオブジェクト名
    public GameObject itemToShow;   // 表示するアイテム

    private GameObject targetObject;

    void Start()
    {
        // 表示するアイテムを非表示にしておく
        if (itemToShow != null)
        {
            itemToShow.SetActive(false);
        }

        // シーンがすでに読み込まれている場合、オブジェクトを探す
        if (SceneManager.GetSceneByName(targetSceneName).isLoaded)
        {
            FindTargetObject();
        }
        else
        {
            // シーンを読み込み
            SceneManager.LoadSceneAsync(targetSceneName, LoadSceneMode.Additive).completed += (operation) =>
            {
                FindTargetObject();
            };
        }
    }

    void Update()
    {
        // ターゲットオブジェクトが消えた場合、アイテムを表示
        if (targetObject == null && itemToShow != null && !itemToShow.activeSelf)
        {
            itemToShow.SetActive(true);
            Debug.Log("アイテムが表示されました: " + itemToShow.name);
        }
    }

    private void FindTargetObject()
    {
        Scene targetScene = SceneManager.GetSceneByName(targetSceneName);

        if (targetScene.IsValid())
        {
            // シーン内のすべてのルートオブジェクトを探す
            foreach (GameObject rootObject in targetScene.GetRootGameObjects())
            {
                if (rootObject.name == targetObjectName)
                {
                    targetObject = rootObject;
                    Debug.Log("監視対象のオブジェクトを見つけました: " + targetObject.name);
                    break;
                }
            }

            if (targetObject == null)
            {
                Debug.LogWarning("指定されたオブジェクトが見つかりませんでした: " + targetObjectName);
            }
        }
        else
        {
            Debug.LogError("指定されたシーンが無効です: " + targetSceneName);
        }
    }
}
