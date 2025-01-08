using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class SceneChange_1to2 : MonoBehaviour
{
    private static string persistentScene = "test_iOS 1"; // 永続シーン

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
            StartCoroutine(CoLoad());
        }
    }

    IEnumerator CoLoad()
    {
        // 新しいシーンarea2を非同期でロード
        AsyncOperation loadOperation = SceneManager.LoadSceneAsync("area2", LoadSceneMode.Additive);

        // area2のロードが完了するまで待機
        while (!loadOperation.isDone)
        {
            yield return null;
        }

        // 現在ロードされているシーンを確認し、area1はアンロード
        foreach (var scene in SceneManager.GetAllScenes())
        {
            // 永続シーンと新しくロードしたarea2はアンロードしない
            if (scene.name != persistentScene && scene.name != "area2")
            {
                SceneManager.UnloadSceneAsync(scene);
            }
        }

        // アクティブシーンをarea2に設定
        SceneManager.SetActiveScene(SceneManager.GetSceneByName("area2"));

        // ロード完了後の処理
        Debug.Log("Switched to area2, keeping persistent scene 'test_iOS 1'.");
    }
}
