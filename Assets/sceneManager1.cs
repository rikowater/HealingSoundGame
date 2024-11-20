using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class sceneManager1 : MonoBehaviour
{
    private static bool Loaded { get; set; }

    void Awake()
    {
        if(Loaded) return;

        Loaded = true;
        SceneManager.LoadScene("test_iOS 1", LoadSceneMode.Additive);
    }
}
