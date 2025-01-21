using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class SceneChange_toListening : MonoBehaviour
{
    public string targetScene = "listenig_area"; // 遷移先のシーン名
    public string loadingScene = "LoadingScene"; // ローディングシーン名
    public AudioClip clickSound; // クリック時の効果音
    private AudioSource audioSource; // 効果音再生用

    private void Awake()
    {
        // AudioSourceコンポーネントを取得または追加
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }
    }

    public void OnButtonClick()
    {
        // 効果音を再生
        if (clickSound != null && audioSource != null)
        {
            audioSource.PlayOneShot(clickSound);
        }

        // ローディングシーンをAdditiveでロードし、次のシーン情報を渡す
        LoadingController.NextSceneName = targetScene;

        // ローディングシーンをAdditiveでロード
        SceneManager.LoadScene(loadingScene, LoadSceneMode.Additive);

        // 遷移処理を開始
        StartCoroutine(CoLoad());
    }

    private IEnumerator CoLoad()
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
        for (int i = sceneCount - 1; i >= 0; i--) // 後ろから安全に処理
        {
            Scene scene = SceneManager.GetSceneAt(i);
            // ターゲットシーンとローディングシーン以外をアンロード
            if (scene.name != targetScene && scene.name != loadingScene)
            {
                SceneManager.UnloadSceneAsync(scene);
            }
        }

        // アクティブシーンをターゲットシーンに設定
        SceneManager.SetActiveScene(SceneManager.GetSceneByName(targetScene));

        // ローディングシーンをアンロード
        yield return SceneManager.UnloadSceneAsync(loadingScene);

        // ロード完了後の処理
        Debug.Log($"Switched to {targetScene}.");
    }
}
