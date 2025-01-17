using System.Linq;
using UnityEngine;
using System.Collections.Generic;

public class InverseColliderArea : MonoBehaviour
{
    [System.Serializable]
    public struct ColliderData
    {
        public PrimitiveType shape;  // コライダーの形状
        public Vector3 size;         // コライダーの大きさ
        public Vector3 offset;       // コライダーの生成位置のオフセット
    }

    [SerializeField]
    private List<ColliderData> colliderSettings = new List<ColliderData>(); // コライダー設定のリスト

    private List<GameObject> colliderObjects = new List<GameObject>(); // コライダーオブジェクトのリスト
    private List<bool> playerInsideColliderStates = new List<bool>(); // 各コライダー内のプレイヤー状態

    private void Start()
    {
        // コライダーを生成
        foreach (var data in colliderSettings)
        {
            CreateInverseCollider(data);
        }
    }

    private void CreateInverseCollider(ColliderData data)
    {
        // 指定された形状のコライダーを生成
        GameObject colliderObject = GameObject.CreatePrimitive(data.shape);
        colliderObject.transform.position = transform.position + data.offset; // オフセットを適用
        colliderObject.transform.SetParent(transform); // 親を設定
        colliderObject.transform.localScale = data.size; // サイズを設定

        // 描画を無効化
        Destroy(colliderObject.GetComponent<MeshRenderer>());

        // 既存のコライダーを削除
        Collider[] colliders = colliderObject.GetComponents<Collider>();
        for (int i = 0; i < colliders.Length; i++) {
            Destroy(colliders[i]);
        }

        // メッシュの面を逆にしてMeshColliderを追加
        var mesh = colliderObject.GetComponent<MeshFilter>().mesh;
        mesh.triangles = mesh.triangles.Reverse().ToArray();
        var meshCollider = colliderObject.AddComponent<MeshCollider>();
        meshCollider.isTrigger = true; // トリガーとして動作

        // イベントを処理するためにスクリプトを追加
        var triggerHandler = colliderObject.AddComponent<ColliderTriggerHandler>();
        triggerHandler.SetParentArea(this);

        // コライダーオブジェクトをリストに追加
        colliderObjects.Add(colliderObject);
        playerInsideColliderStates.Add(false); // 初期状態は「エリア外」とする
    }

    // プレイヤーが指定のコライダー内に入ったときに状態を更新
    public void PlayerEnteredCollider(GameObject colliderObject)
    {
        int index = colliderObjects.IndexOf(colliderObject);
        if (index != -1)
        {
            playerInsideColliderStates[index] = true;
        }
    }

    // プレイヤーが指定のコライダーから退出したときに状態を更新
    public void PlayerExitedCollider(GameObject colliderObject)
    {
        int index = colliderObjects.IndexOf(colliderObject);
        if (index != -1)
        {
            playerInsideColliderStates[index] = false;
        }
    }

    // プレイヤーが1つ以上のコライダー範囲外にいる場合に自由に移動できるかを判定
    public bool IsPlayerFreeToMove()
    {
        // プレイヤーが1つでも範囲外にいる場合、自由に移動できる
        return playerInsideColliderStates.Contains(false);
    }
}

public class ColliderTriggerHandler : MonoBehaviour
{
    private InverseColliderArea parentArea;

    public void SetParentArea(InverseColliderArea area)
    {
        parentArea = area;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // プレイヤーがコライダー内に入った
            parentArea.PlayerEnteredCollider(this.gameObject);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // プレイヤーがコライダーを出た
            parentArea.PlayerExitedCollider(this.gameObject);
        }
    }
}