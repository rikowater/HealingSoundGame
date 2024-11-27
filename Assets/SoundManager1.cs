using UnityEngine;
using UnityEngine.UI;

public class ChangeSoundVolume : MonoBehaviour
{
    private AudioSource audioSource;
    public Slider volumeSlider; // スライダーの参照を設定する

    private bool isMuted = true; // シーン開始時にミュート状態

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
        volumeSlider.value = audioSource.volume;

        // 初期状態でミュートに設定
        MuteAudio(true);
    }

    /// <summary>
    /// スライドバー値の変更イベント
    /// </summary>
    /// <param name="newSliderValue">スライドバーの値(自動的に引数に値が入る)</param>
    public void SoundSliderOnValueChange(float newSliderValue)
    {
        if (audioSource == null) return;
        if (!isMuted) // ミュート状態ではない場合のみ実行
        {
            audioSource.volume = newSliderValue;
        }
    }

    /// <summary>
    /// ミュートの切り替え処理
    /// </summary>
    public void ToggleMute()
    {
        if (audioSource == null || volumeSlider == null) return;

        isMuted = !isMuted;

        MuteAudio(isMuted); // ミュート処理

        // スライダーの操作を制御
        volumeSlider.interactable = !isMuted;
    }

    // ミュート処理を行うヘルパー関数
    private void MuteAudio(bool mute)
    {
        if (mute)
        {
            audioSource.volume = 0; // 音を完全にミュート
        }
        else
        {
            audioSource.volume = volumeSlider.value; // スライダーの値を反映
        }
    }
}
