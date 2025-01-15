using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class LoadingController : MonoBehaviour
{
    public static string NextSceneName; // 次にロードするシーンの名前

    private void Start()
    {
        // ローディング処理を開始
        StartCoroutine(LoadNextScene());
    }

    private IEnumerator LoadNextScene()
    {
        // 2秒間ローディング画面を表示
        yield return new WaitForSeconds(2);

        // 次のシーンをAdditiveでロード
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(NextSceneName, LoadSceneMode.Additive);

        // ロード完了まで待機
        while (!asyncLoad.isDone)
        {
            yield return null;
        }

        // `LoadingScene`をアンロード
        SceneManager.UnloadSceneAsync("LoadingScene");

        // アクティブシーンをターゲットシーンに設定
        SceneManager.SetActiveScene(SceneManager.GetSceneByName(NextSceneName));
    }
}
