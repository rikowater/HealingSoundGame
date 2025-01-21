using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class SceneChange_to1 : MonoBehaviour
{
    private static string persistentScene = "UIScene"; // 永続シーン
    public string targetScene = "area1"; // 遷移先のシーン名
    public string loadingScene = "LoadingScene"; // ローディングシーン名
    public AudioClip clickSound; // クリック時の効果音
    private AudioSource audioSource; // 効果音再生用

    void Start()
    {
        // 永続シーンがロードされていない場合、ロードする
        if (!SceneManager.GetSceneByName(persistentScene).isLoaded)
        {
            SceneManager.LoadScene(persistentScene, LoadSceneMode.Additive);
        }
    }

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

        // `LoadingScene`をロード
        SceneManager.LoadScene(loadingScene, LoadSceneMode.Additive);

        // 現在のシーン（`area1`）をアンロード
        StartCoroutine(UnloadCurrentScene());
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
        //yield return SceneManager.UnloadSceneAsync(loadingScene);

        // ロード完了後の処理
        Debug.Log($"Switched to {targetScene}.");
    }
}
