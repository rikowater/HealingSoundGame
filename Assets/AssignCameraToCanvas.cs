using UnityEngine;

public class AssignCameraToCanvas : MonoBehaviour
{
    public Canvas worldSpaceCanvas; // World Space Canvas
    private Camera currentCamera;

    void Start()
    {
        // 初期カメラ設定
        AssignMainCamera();
    }

    void Update()
    {
        // 毎フレーム MainCamera を確認
        if (currentCamera != Camera.main)
        {
            AssignMainCamera();
        }
    }

    private void AssignMainCamera()
    {
        currentCamera = Camera.main; // Tag でカメラを取得
        if (currentCamera != null)
        {
            worldSpaceCanvas.worldCamera = currentCamera; // Canvas にカメラを割り当て
            Debug.Log("Canvas に新しいカメラを割り当てました: " + currentCamera.name);
        }
        else
        {
            Debug.LogWarning("Main Camera が見つかりません。");
        }
    }
}
