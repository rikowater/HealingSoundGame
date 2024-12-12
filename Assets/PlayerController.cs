using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    Animator animator;

    float speed = 3.0f;

    void Start()
    {
        // Animatorコンポーネントの取得
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        bool isWalking = false;

        // Wキー（前方移動）
        if (Input.GetKey(KeyCode.W))
        {
            transform.position += speed * transform.forward * Time.deltaTime;
            isWalking = true;
        }

        // Sキー（後方移動）
        if (Input.GetKey(KeyCode.S))
        {
            transform.position -= speed * transform.forward * Time.deltaTime;
            isWalking = true;
        }

        // Dキー（右移動）
        if (Input.GetKey(KeyCode.D))
        {
            transform.position += speed * transform.right * Time.deltaTime;
            isWalking = true;
        }

        // Aキー（左移動）
        if (Input.GetKey(KeyCode.A))
        {
            transform.position -= speed * transform.right * Time.deltaTime;
            isWalking = true;
        }

        // アニメーションの切り替え
        if (isWalking)
        {
            animator.SetTrigger("WalkTrigger");
        }
        else
        {
            animator.SetTrigger("IdleTrigger");
        }
    }
}
