using UnityEngine;

public class cubeMove : MonoBehaviour
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

    //アイテムに触れた時の処理
    void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.tag == "Item")
        {
            Debug.Log("アイテムを拾う（B）");

            if (Input.GetKeyDown(KeyCode.B)) //Bキーを押すとアイテムを拾う
            {
                Destroy(collision.gameObject); //拾ったらそのアイテムを消す
            }
        }
    }

    //アイテムから離れるとメッセージを空の文字列に戻す
    void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.tag == "Item")
        {
            Debug.Log("アイテムから離れました");
        }
    }
}
