using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneManager1 : MonoBehaviour
{
    private static SceneManager1 instance;

    private void Awake()
    {
        // シングルトンパターンの実装
        if (instance != null)
        {
            Destroy(gameObject); // 既に存在する場合、重複を削除
            return;
        }

        instance = this;
        DontDestroyOnLoad(gameObject); // シーンをまたいで保持

        // シーンの読み込みを確認
        if (!IsSceneLoaded("test_iOS 1"))
        {
            SceneManager.LoadScene("test_iOS 1", LoadSceneMode.Additive);
        }
    }

    // シーンが既に読み込まれているか確認
    private bool IsSceneLoaded(string sceneName)
    {
        for (int i = 0; i < SceneManager.sceneCount; i++)
        {
            Scene scene = SceneManager.GetSceneAt(i);
            if (scene.name == sceneName)
            {
                return true;
            }
        }
        return false;
    }
}
