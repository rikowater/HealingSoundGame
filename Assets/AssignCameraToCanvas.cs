using UnityEngine;

public class AssignCameraToCanvas : MonoBehaviour
{
    public Canvas worldSpaceCanvas; // World Space Canvas
    private Camera sceneCamera;

    void Start()
    {
        // 別シーンのカメラを取得
        sceneCamera = Camera.main; // 例: Tag でカメラを取得
        if (sceneCamera != null)
        {
            worldSpaceCanvas.worldCamera = sceneCamera; // Canvas にカメラを割り当て
        }
        else
        {
            Debug.LogWarning("Main Camera が見つかりません。");
        }
    }
}
