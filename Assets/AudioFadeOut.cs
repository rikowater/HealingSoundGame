using UnityEngine;
using System.Collections;

public class AudioFadeOut : MonoBehaviour
{
    private AudioSource audioSource;
    public AudioClip clickSound; // クリック効果音

    void Start()
    {
        // AudioSourceを取得
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            Debug.LogError("AudioSourceが見つかりません。");
        }
    }

    /// <summary>
    /// フェードアウトを開始するメソッド
    /// </summary>
    /// <param name="duration">フェードアウトにかける時間（秒）</param>
    public void StartFadeOut(float duration)
    {
        if (audioSource != null && audioSource.isPlaying)
        {
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
    }
}
