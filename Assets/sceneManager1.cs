using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class SceneHandler : MonoBehaviour
{
    // 永続的に保持するシーン名
    private static string persistentScene = "test_iOS 1";

    void Start()
    {
        // 永続シーンがロードされていない場合、ロードする
        if (!SceneManager.GetSceneByName(persistentScene).isLoaded)
        {
            SceneManager.LoadScene(persistentScene, LoadSceneMode.Additive);
        }
    }

    // 他のシーンに移動するメソッド
    public void SwitchScene(string newSceneName)
    {
        StartCoroutine(LoadNewScene(newSceneName));
    }

    private IEnumerator LoadNewScene(string newSceneName)
    {
        // 新しいシーンを非同期でロード
        AsyncOperation loadOperation = SceneManager.LoadSceneAsync(newSceneName, LoadSceneMode.Additive);

        // ロードが完了するまで待機
        while (!loadOperation.isDone)
        {
            yield return null;
        }

        // 現在ロードされているシーンを確認し、不要なシーンをアンロード
        int sceneCount = SceneManager.sceneCount; // 現在のシーン数を取得
        for (int i = 0; i < sceneCount; i++)
        {
            Scene scene = SceneManager.GetSceneAt(i);
            // 永続シーンと新しいシーン以外をアンロード
            if (scene.name != persistentScene && scene.name != newSceneName)
            {
                SceneManager.UnloadSceneAsync(scene);
            }
        }

        // アクティブシーンを新しいシーンに設定
        SceneManager.SetActiveScene(SceneManager.GetSceneByName(newSceneName));

        // デバッグログ
        Debug.Log($"Switched to {newSceneName}, keeping {persistentScene}.");
    }
}
