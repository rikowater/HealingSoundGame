using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class SceneChange_2to1 : MonoBehaviour
{
    private static string persistentScene = "UIScene"; // 永続シーン
    public string targetScene = "area1"; // 遷移先のシーン名
    public string loadingScene = "LoadingScene"; // ローディングシーン名

    void Start()
    {
        // 永続シーンがロードされていない場合、ロードする
        if (!SceneManager.GetSceneByName(persistentScene).isLoaded)
        {
            SceneManager.LoadScene(persistentScene, LoadSceneMode.Additive);
        }
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            // ローディングシーンをロードし、次のシーン情報を渡す
            LoadingController.NextSceneName = targetScene;
            SceneManager.LoadScene(loadingScene);
        }
    }

    IEnumerator CoLoad()
    {
        // 新しいシーンを非同期でロード
        AsyncOperation loadOperation = SceneManager.LoadSceneAsync(targetScene, LoadSceneMode.Additive);

        // ロードが完了するまで待機
        while (!loadOperation.isDone)
        {
            yield return null;
        }

        // 現在ロードされているシーンを取得し、不要なシーンをアンロード
        int sceneCount = SceneManager.sceneCount; // 現在のシーン数
        for (int i = 0; i < sceneCount; i++)
        {
            Scene scene = SceneManager.GetSceneAt(i);
            // 永続シーンとターゲットシーン以外をアンロード
            if (scene.name != persistentScene && scene.name != targetScene)
            {
                SceneManager.UnloadSceneAsync(scene);
            }
        }

        // アクティブシーンをターゲットシーンに設定
        SceneManager.SetActiveScene(SceneManager.GetSceneByName(targetScene));

        // ロード完了後の処理
        Debug.Log($"Switched to {targetScene}, keeping persistent scene '{persistentScene}'.");
    }
}
