using UnityEngine;
using UnityEngine.SceneManagement;

public class ShowObjectInSpecificScene : MonoBehaviour
{
    // 表示させたいシーン名
    [SerializeField] private string targetSceneName;

    void Start()
    {
        // 現在のシーン名を取得
        string currentSceneName = SceneManager.GetActiveScene().name;

        // シーン名が一致していれば表示、そうでなければ非表示
        gameObject.SetActive(currentSceneName == targetSceneName);
    }

    // シーンが変更されたときに呼ばれる
    void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // シーンがロードされた際にオブジェクトを切り替える
        gameObject.SetActive(scene.name == targetSceneName);
    }
}
