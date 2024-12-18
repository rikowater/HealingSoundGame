using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Transform target; // キャラクターのTransform
    public Vector3 offset = new Vector3(0, 5, -10); // カメラのオフセット
    public float smoothSpeed = 0.125f; // カメラ追尾のスムーズさ

    void LateUpdate()
    {
        // 目標位置を計算
        Vector3 desiredPosition = target.position + offset;

        // スムーズにカメラを移動
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);
        transform.position = smoothedPosition;

        // ターゲットを常に注視
        transform.LookAt(target);
    }
}
