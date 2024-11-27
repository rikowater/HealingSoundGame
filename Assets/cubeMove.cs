using UnityEngine;

public class CubeMove : MonoBehaviour
{
    public float moveSpeed = 5f; // 移動速度

    void Update()
    {
        // 入力値を取得 (水平: X軸, 垂直: Y軸)
        float horizontal = Input.GetAxis("Horizontal"); // 左右キーまたはA/Dキー
        float vertical = Input.GetAxis("Vertical");     // 上下キーまたはW/Sキー

        // 移動量を計算
        Vector3 movement = new Vector3(horizontal, vertical, 0) * moveSpeed * Time.deltaTime;

        // オブジェクトを移動
        transform.Translate(movement);
    }

    // アイテムに触れた時の処理
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Item")) // "Item"タグを持つオブジェクトのみ削除
        {
            Debug.Log("アイテムに触れました: " + other.gameObject.name);
            Destroy(other.gameObject); // 触れたアイテムを削除
        }
    }
}
