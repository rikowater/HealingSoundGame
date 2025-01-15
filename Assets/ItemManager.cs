using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

public class ItemManager : MonoBehaviour
{
    [System.Serializable]
    private class ItemData
    {
        [SerializeField] private string targetSceneName;  // 監視対象のオブジェクトがいるシーン名
        [SerializeField] private string targetObjectName; // 監視対象のオブジェクト名
        [SerializeField] private GameObject itemToShow1;  // 表示する1つ目のアイテム
        [SerializeField] private GameObject itemToShow2;  // 表示する2つ目のアイテム

        public string TargetSceneName => targetSceneName; // 読み取り専用プロパティ
        public string TargetObjectName => targetObjectName;
        public GameObject ItemToShow1 => itemToShow1;
        public GameObject ItemToShow2 => itemToShow2;
    }

    [SerializeField] private List<ItemData> items = new List<ItemData>(); // 複数のアイテムデータリスト
    private Dictionary<ItemData, GameObject> targetObjects = new Dictionary<ItemData, GameObject>();

    void Start()
    {
        foreach (var item in items)
        {
            // 表示するアイテムを非表示にしておく
            if (item.ItemToShow1 != null)
            {
                item.ItemToShow1.SetActive(false);
            }
            if (item.ItemToShow2 != null)
            {
                item.ItemToShow2.SetActive(false);
            }

            // シーンがすでに読み込まれている場合、オブジェクトを探す
            if (SceneManager.GetSceneByName(item.TargetSceneName).isLoaded)
            {
                FindTargetObject(item);
            }
            else
            {
                // シーンを読み込み
                //SceneManager.LoadSceneAsync(item.TargetSceneName, LoadSceneMode.Additive).completed += (operation) =>
                // {
                //     FindTargetObject(item);
                // };
            }
        }
    }

    void Update()
    {
        foreach (var item in items)
        {
            // ターゲットオブジェクトが消えた場合、アイテムを表示
            if (targetObjects.ContainsKey(item) && targetObjects[item] == null)
            {
                if (item.ItemToShow1 != null && !item.ItemToShow1.activeSelf)
                {
                    item.ItemToShow1.SetActive(true);
                    //Debug.Log("アイテム1が表示されました: " + item.ItemToShow1.name);
                }
                if (item.ItemToShow2 != null && !item.ItemToShow2.activeSelf)
                {
                    item.ItemToShow2.SetActive(true);
                    //Debug.Log("アイテム2が表示されました: " + item.ItemToShow2.name);
                }
            }
        }
    }

    private void FindTargetObject(ItemData item)
    {
        Scene targetScene = SceneManager.GetSceneByName(item.TargetSceneName);

        if (targetScene.IsValid())
        {
            // シーン内のすべてのルートオブジェクトを探す
            foreach (GameObject rootObject in targetScene.GetRootGameObjects())
            {
                if (rootObject.name == item.TargetObjectName)
                {
                    targetObjects[item] = rootObject;
                    //Debug.Log("監視対象のオブジェクトを見つけました: " + rootObject.name);
                    return;
                }
            }

            //Debug.LogWarning("指定されたオブジェクトが見つかりませんでした: " + item.TargetObjectName);
        }
        else
        {
            //Debug.LogError("指定されたシーンが無効です: " + item.TargetSceneName);
        }
    }
}
