using UnityEngine;
using System.Collections;

public class AudioFadeOut : MonoBehaviour
{
    private AudioSource audioSource;
    public AudioClip clickSound; // クリック効果音
    private bool isFadingOut = false; // フェードアウト中かどうか

    void Start()
    {
        // AudioSourceを取得
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            Debug.LogError("AudioSourceが見つかりません。フェードアウト処理はスキップされます。");
            enabled = false; // スクリプトを無効化
        }
    }

    /// <summary>
    /// フェードアウトを開始するメソッド
    /// </summary>
    /// <param name="duration">フェードアウトにかける時間（秒）</param>
    public void StartFadeOut(float duration)
    {
        if (isFadingOut)
        {
            Debug.LogWarning("既にフェードアウト中です。");
            return;
        }

        if (audioSource != null && audioSource.isPlaying)
        {
            isFadingOut = true;
            StartCoroutine(FadeOutCoroutine(duration));
        }
    }

    private IEnumerator FadeOutCoroutine(float duration)
    {
        float startVolume = audioSource.volume;

        // フェードアウト処理
        while (audioSource.volume > 0)
        {
            audioSource.volume -= startVolume * Time.deltaTime / duration;
            yield return null;
        }

        // 完全に音を停止
        audioSource.volume = 0;
        audioSource.Stop();
        isFadingOut = false; // フェードアウト完了
    }

    /// <summary>
    /// クリック効果音を再生するメソッド
    /// </summary>
    public void PlayClickSound()
    {
        if (clickSound != null && audioSource != null)
        {
            audioSource.PlayOneShot(clickSound);
        }
        else
        {
            Debug.LogWarning("クリック効果音またはAudioSourceが設定されていません。");
        }
    }

    /// <summary>
    /// フェードアウトを中断し、ボリュームをリセット
    /// </summary>
    public void StopFadeOut()
    {
        StopAllCoroutines();
        if (audioSource != null)
        {
            audioSource.volume = 1.0f; // 元のボリュームにリセット
            isFadingOut = false;
        }
    }

    public IEnumerator FadeOutAndWait(float duration)
    {
        if (audioSource != null && audioSource.isPlaying)
        {
            float startVolume = audioSource.volume;

            // フェードアウト処理
            while (audioSource.volume > 0)
            {
                audioSource.volume -= startVolume * Time.deltaTime / duration;
                yield return null;
            }

            // 完全に音を停止
            audioSource.volume = 0;
            audioSource.Stop();
        }
    }

}
