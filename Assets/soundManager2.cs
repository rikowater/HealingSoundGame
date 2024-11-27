using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class soundManager2 : MonoBehaviour
{
	private AudioSource audioSource;

	private void Start()
	{
		// "AudioSource"コンポーネントを取得
		audioSource = gameObject.GetComponent<AudioSource>();
	}

	/// <summary>
	/// スライドバー値の変更イベント
	/// </summary>
	/// <param name="newSliderValue">スライドバーの値(自動的に引数に値が入る)</param>
	public void SoundSliderOnValueChange(float newSliderValue)
	{
		// 音楽の音量をスライドバーの値に変更
		audioSource.volume = newSliderValue;
	}


	private bool isMuted = false;

    public void ToggleMute()
    {
        isMuted = !isMuted;

        if (isMuted)
        {
            audioSource.volume = 0; // 音を完全にミュート
        }
        else
        {
            audioSource.volume = 1; // 音を通常のボリュームに戻す
        }
    }
}
