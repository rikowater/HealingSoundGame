using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class sceneManager : MonoBehaviour
{
    private static bool Loaded { get; set; }

    void Awake()
    {
        if(Loaded) return;

        Loaded = true;
        SceneManager.LoadScene("test_iOS 1", LoadSceneMode.Additive);
    }

    public void OnLoadSceneAdditive()
    {
        //SceneBを加算ロード。現在のシーンは残ったままで、シーンBが追加される
        SceneManager.LoadScene("test_Scenes",LoadSceneMode.Additive);
    }

    //void Awake()
    //{
    //    DontDestroyOnLoad(this.gameObject);
    //}
}
