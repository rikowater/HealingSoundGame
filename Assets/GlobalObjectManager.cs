using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalObjectManager : MonoBehaviour
{
    // シングルトンのインスタンス
    public static GlobalObjectManager Instance { get; private set; }

    // シーン間で共有するオブジェクトの参照
    public GameObject sharedObject;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            //DontDestroyOnLoad(gameObject); // シーンをまたいでもオブジェクトを維持
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
