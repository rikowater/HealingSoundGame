using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class soundManager5 : MonoBehaviour
{
    private AudioSource audioSource;
    public Slider volumeSlider; // スライダーの参照を設定する

    public string targetSceneName; // 対象シーン名
    public string targetObjectName; // 対象オブジェクト名

    private GameObject targetObject; // 別シーンのオブジェクト参照
    private bool isMuted = true; // シーン開始時にミュート状態
    private float lastVolume;   // ミュート解除時に戻す音量

    private void Start()
    {
        // "AudioSource"コンポーネントを取得
        audioSource = gameObject.GetComponent<AudioSource>();
        if (audioSource == null)
        {
            Debug.LogError("AudioSource コンポーネントがこのオブジェクトに見つかりません！");
            return;
        }

        // スライダーの初期値を現在の音量に設定
        if (volumeSlider == null)
        {
            Debug.LogError("Volume Slider が設定されていません！");
            return;
        }

        lastVolume = audioSource.volume; // 初期音量を保存
        volumeSlider.value = lastVolume;

        // 初期状態でミュートに設定
        isMuted = true;
        audioSource.volume = 0; // 音を完全にミュート

        // 対象オブジェクトを探すコルーチンを開始
        if (!string.IsNullOrEmpty(targetSceneName) && !string.IsNullOrEmpty(targetObjectName))
        {
            StartCoroutine(FindTargetObjectInScene());
        }
    }

    /// <summary>
    /// スライドバー値の変更イベント
    /// </summary>
    public void SoundSliderOnValueChange(float newSliderValue)
    {
        if (audioSource == null) return;

        // スライダーの値を保存しておく（ミュート解除時に反映）
        lastVolume = newSliderValue;

        // ミュート状態でなければ即座に音量を反映
        if (!isMuted)
        {
            audioSource.volume = newSliderValue;
        }
    }

    /// <summary>
    /// ミュートの切り替え処理
    /// </summary>
    public void ToggleMute()
    {
        if (audioSource == null) return;

        isMuted = !isMuted;

        if (isMuted)
        {
            audioSource.volume = 0; // 音を完全にミュート
            if (targetObject != null)
            {
                targetObject.SetActive(false); // オブジェクトを非表示
            }
        }
        else
        {
            audioSource.volume = lastVolume; // ミュート解除時に保存していた音量を復元
            if (targetObject != null)
            {
                targetObject.SetActive(true); // オブジェクトを表示
            }
        }
    }

    /// <summary>
    /// 指定されたシーンにあるオブジェクトを探すコルーチン
    /// </summary>
    private IEnumerator FindTargetObjectInScene()
    {
        // 対象シーンがロードされるまで待機
        while (!SceneManager.GetSceneByName(targetSceneName).isLoaded)
        {
            yield return null; // 次のフレームまで待機
        }

        // 対象シーンをアクティブにする（必須ではないが推奨）
        Scene targetScene = SceneManager.GetSceneByName(targetSceneName);
        SceneManager.SetActiveScene(targetScene);

        // シーン内の対象オブジェクトを探す
        GameObject foundObject = GameObject.Find(targetObjectName);
        if (foundObject != null)
        {
            targetObject = foundObject;
            // ミュート状態に応じた初期表示設定
            targetObject.SetActive(!isMuted);
        }
        else
        {
            Debug.LogError($"指定されたオブジェクト '{targetObjectName}' がシーン '{targetSceneName}' 内に見つかりません！");
        }
    }
}
