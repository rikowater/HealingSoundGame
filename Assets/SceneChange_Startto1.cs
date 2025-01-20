using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class SceneChange_Startto1 : MonoBehaviour
{
    public string targetScene = "area1"; // 遷移先のシーン名
    public string loadingScene = "LoadingScene"; // ローディングシーン名
    public AudioClip clickSound; // クリック時の効果音

    private AudioSource audioSource;

    void Start()
    {
        // AudioSourceを追加または取得
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }
    }

    public void OnScreenClick()
    {
        // 効果音を再生
        if (clickSound != null)
        {
            audioSource.PlayOneShot(clickSound);
        }

        // フェードアウト処理を呼び出し
        AudioFadeOut fadeOut = GetComponent<AudioFadeOut>();
        if (fadeOut != null)
        {
            fadeOut.StartFadeOut(2f); // フェードアウト時間を指定（例: 2秒）
        }

        // 次のシーンでのキャラクター位置を指定
        PlayerPositionManager.TargetPosition = new Vector3(6, 0, 0); // 例: 次のシーンでの初期位置
        PlayerPositionManager.TargetRotation = Quaternion.Euler(0, 180, 0); // 例: 初期回転

        // ローディングシーンをAdditiveでロードし、次のシーン情報を渡す
        LoadingController.NextSceneName = targetScene;

        // ローディングシーンをAdditiveでロード
        SceneManager.LoadScene(loadingScene, LoadSceneMode.Additive);

        // 遷移処理を開始
        StartCoroutine(CoLoad());
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
            // ターゲットシーンとローディングシーン以外をアンロード
            if (scene.name != targetScene && scene.name != loadingScene)
            {
                SceneManager.UnloadSceneAsync(scene);
            }
        }

        // アクティブシーンをターゲットシーンに設定
        SceneManager.SetActiveScene(SceneManager.GetSceneByName(targetScene));

        // ローディングシーンをアンロード
        SceneManager.UnloadSceneAsync(loadingScene);

        // ロード完了後の処理
        Debug.Log($"Switched to {targetScene}.");
    }
}
