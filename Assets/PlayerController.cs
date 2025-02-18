using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour
{
    private Rigidbody _rigidbody;
    private Transform _transform;
    private Animator _animator;
    private float _horizontal;
    private float _vertical;
    private Vector3 _velocity;
    private float _speed = 4.5f;

    private Vector3 _aim; // 進行方向
    private Quaternion _playerRotation; // キャラクターの回転

    // void Awake()
    // {
    //     StartCoroutine(WaitForGlobalObjectManager());
    // }

    // private IEnumerator WaitForGlobalObjectManager()
    // {
    //     // 別シーンのロードが完了するのを待つ
    //     while (GlobalObjectManager.Instance == null || GlobalObjectManager.Instance.sharedObject == null)
    //     {
    //         Debug.Log("Waiting for GlobalObjectManager to be initialized...");
    //         yield return null; // 次のフレームまで待機
    //     }

    //     Debug.Log("Setting sharedObject to inactive.");
    //     GlobalObjectManager.Instance.sharedObject.SetActive(false);
    // }

    void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _transform = GetComponent<Transform>();
        _animator = GetComponent<Animator>();

        _playerRotation = _transform.rotation;

        // PlayerPositionManagerに位置情報がある場合、適用する
        if (PlayerPositionManager.TargetPosition.HasValue)
        {
            transform.position = PlayerPositionManager.TargetPosition.Value;
        }

        if (PlayerPositionManager.TargetRotation.HasValue)
        {
            transform.rotation = PlayerPositionManager.TargetRotation.Value;
        }

        // 位置情報をリセット
        PlayerPositionManager.TargetPosition = null;
        PlayerPositionManager.TargetRotation = null;
    }

    void FixedUpdate()
    {
        // シングルトン経由でジョイスティックの入力を取得
        _horizontal = JoystickInputManager.Instance.Horizontal;
        _vertical = JoystickInputManager.Instance.Vertical;

        var _horizontalRotation = Quaternion.AngleAxis(Camera.main.transform.eulerAngles.y, Vector3.up);
        _velocity = _horizontalRotation * new Vector3(_horizontal, 0, _vertical).normalized;

        _aim = _velocity;

        if (_aim.magnitude > 0.1f)
        {
            _playerRotation = Quaternion.LookRotation(_aim, Vector3.up);
        }

        _transform.rotation = Quaternion.RotateTowards(_transform.rotation, _playerRotation, 600 * Time.deltaTime);

        _animator.SetBool("Walk", _velocity.magnitude > 0.1f);

        _rigidbody.velocity = _velocity * _speed;
    }

    // private void OnTriggerStay(Collider other)
    // {
    //     if (other.gameObject.CompareTag("Target"))
    //     {
    //         // トリガー内に入ったらオブジェクトを表示
    //         if (GlobalObjectManager.Instance != null && GlobalObjectManager.Instance.sharedObject != null)
    //         {
    //             GlobalObjectManager.Instance.sharedObject.SetActive(true);
    //         }
    //     }
    // }

    // private void OnTriggerExit(Collider other)
    // {
    //     if (other.gameObject.CompareTag("Target"))
    //     {
    //         // トリガー外に出たらオブジェクトを非表示
    //         if (GlobalObjectManager.Instance != null && GlobalObjectManager.Instance.sharedObject != null)
    //         {
    //             GlobalObjectManager.Instance.sharedObject.SetActive(false);
    //         }
    //     }
    // }
}
