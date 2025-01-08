using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChange_1to2 : MonoBehaviour
{
    // private void OnCollisionEnter(Collision other)
    // {
    //     if (other.gameObject.CompareTag("Player"))
    //     {
    //         SceneManager.LoadScene("area2");
    //     }
    // }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            StartCoroutine(CoLoad());
        }
    }

    IEnumerator CoLoad()
    {
        //SceneAをアンロード
        var op = SceneManager.LoadSceneAsync("area2");
        var qr = SceneManager.UnloadSceneAsync("area1");
        yield return qr;

       //アンロード後の処理を書く

        //必要に応じて不使用アセットをアンロードしてメモリを解放する
        //けっこう重い処理なので、別に管理するのも手
        yield return Resources.UnloadUnusedAssets();
     }
     
}
