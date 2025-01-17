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
    private int playerInsideCount = 0; // プレイヤーがエリア内にいるかのカウント

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
        meshCollider.convex = true; // 凸形に設定
        meshCollider.isTrigger = true; // トリガーとして動作

        // イベントを処理するためにスクリプトを追加
        var triggerHandler = colliderObject.AddComponent<ColliderTriggerHandler>();
        triggerHandler.SetParentArea(this);

        // コライダーオブジェクトをリストに追加
        colliderObjects.Add(colliderObject);
    }

    // エリア内にプレイヤーが侵入したときにカウントを増加
    public void PlayerEnteredArea()
    {
        playerInsideCount++;
    }

    // エリアからプレイヤーが退出したときにカウントを減少
    public void PlayerExitedArea()
    {
        playerInsideCount = Mathf.Max(0, playerInsideCount - 1);
    }

    // プレイヤーがエリア内にいるか確認
    public bool IsPlayerInsideArea()
    {
        return playerInsideCount > 0;
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
            parentArea.PlayerEnteredArea();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            parentArea.PlayerExitedArea();
        }
    }
}
