using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalObjectManager : MonoBehaviour
{
    // シングルトンのインスタンス
    public static GlobalObjectManager Instance;

    // シーン間で共有するオブジェクトの参照
    public GameObject sharedObject;

    private void Awake()
    {
        // シングルトンのセットアップ
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(this); // シーンを切り替えても破棄されないようにする
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
