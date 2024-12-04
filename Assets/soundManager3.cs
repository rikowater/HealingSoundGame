using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class soundManager3 : MonoBehaviour
{
	private AudioSource audioSource;
    public Slider volumeSlider; // スライダーの参照を設定する

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
    }

    /// <summary>
    /// スライドバー値の変更イベント
    /// </summary>
    /// <param name="newSliderValue">スライドバーの値(自動的に引数に値が入る)</param>
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
        }
        else
        {
            audioSource.volume = lastVolume; // ミュート解除時に保存していた音量を復元
        }
    }
}
