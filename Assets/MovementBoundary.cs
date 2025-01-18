using System.Collections.Generic;
using UnityEngine;

public class MovementBoundary : MonoBehaviour
{
    private HashSet<Collider> activeColliders = new HashSet<Collider>();
    private Rigidbody playerRigidbody;
    private Transform playerTransform;

    void Start()
    {
        // プレイヤーオブジェクトを取得
        GameObject player = GameObject.FindWithTag("Player");
        playerTransform = player.transform;
        playerRigidbody = player.GetComponent<Rigidbody>();

        if (playerRigidbody == null)
        {
            Debug.LogError("Player object must have a Rigidbody component.");
        }
    }

    void OnTriggerEnter(Collider other)
    {
        // プレイヤーがコライダー内に入った場合、コライダーを登録
        if (other.CompareTag("Player"))
        {
            activeColliders.Add(this.GetComponent<Collider>());
        }
    }

    void OnTriggerExit(Collider other)
    {
        // プレイヤーがコライダーから出た場合、コライダーを解除
        if (other.CompareTag("Player"))
        {
            activeColliders.Remove(this.GetComponent<Collider>());
        }
    }

    void FixedUpdate()
    {
        if (activeColliders.Count == 0)
        {
            // プレイヤーがコライダー外にいる場合、位置を補正
            Collider closestCollider = GetClosestCollider(playerTransform.position);
            if (closestCollider != null)
            {
                Vector3 closestPoint = closestCollider.ClosestPoint(playerTransform.position);

                // プレイヤーの位置を制限内に戻す
                playerRigidbody.MovePosition(closestPoint);
            }
        }
    }

    private Collider GetClosestCollider(Vector3 position)
    {
        Collider closestCollider = null;
        float closestDistance = float.MaxValue;

        foreach (var col in activeColliders)
        {
            float distance = Vector3.Distance(position, col.ClosestPoint(position));
            if (distance < closestDistance)
            {
                closestDistance = distance;
                closestCollider = col;
            }
        }

        return closestCollider;
    }
}
