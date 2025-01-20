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
            // 次のシーンでのキャラクター位置を指定
            PlayerPositionManager.TargetPosition = new Vector3(46, 0, 0); // 例: 次のシーンでの初期位置
            PlayerPositionManager.TargetRotation = Quaternion.Euler(0, 180, 0); // 例: 初期回転

            // ローディングシーンをAdditiveでロードし、次のシーン情報を渡す
            LoadingController.NextSceneName = targetScene;

            // `LoadingScene`をロード
            SceneManager.LoadScene(loadingScene, LoadSceneMode.Additive);

            // 現在のシーン（`area1`）をアンロード
            StartCoroutine(UnloadCurrentScene());
        }
    }


    private IEnumerator UnloadCurrentScene()
    {
        // 現在のアクティブシーンを取得
        Scene currentScene = SceneManager.GetActiveScene();

        // 永続シーンでない場合、アンロード
        if (currentScene.name != persistentScene)
        {
            AsyncOperation unloadOperation = SceneManager.UnloadSceneAsync(currentScene);

            // アンロード完了まで待機
            while (!unloadOperation.isDone)
            {
                yield return null;
            }

            Debug.Log($"{currentScene.name} has been unloaded.");
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
