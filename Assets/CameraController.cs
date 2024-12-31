using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Transform target; // キャラクターのTransform
    public Vector3 offset = new Vector3(0, 5, -10); // カメラのオフセット
    public float smoothSpeed = 0.125f; // カメラ追尾のスムーズさ
    public Vector3 fixedRotation = new Vector3(25, 0, 0); // 固定するカメラの回転 (euler角)

    void LateUpdate()
    {
        // 目標位置を計算
        Vector3 desiredPosition = target.position + offset;

        // スムーズにカメラを移動
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);
        transform.position = smoothedPosition;

        // カメラの回転を固定
        transform.rotation = Quaternion.Euler(fixedRotation);
    }
}
