using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Rigidbody _rigidbody;
    private Transform _transform;
    private Animator _animator;
    private float _horizontal;
    private float _vertical;
    private Vector3 _velocity;
    private float _speed = 4f;

    private Vector3 _aim; // 進行方向
    private Quaternion _playerRotation; // キャラクターの回転

    public FixedJoystick inputMove; // JoyStick

    // 特定のオブジェクトを設定
    [SerializeField] private GameObject targetObjectToCheck; // チェック対象のオブジェクト
    [SerializeField] private GameObject objectToShow;        // 表示したいオブジェクト
    
    void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _transform = GetComponent<Transform>();
        _animator = GetComponent<Animator>();

        _playerRotation = _transform.rotation;

        // 表示したいオブジェクトを非表示にしておく
        if (objectToShow != null)
        {
            objectToShow.SetActive(false);
        }
    }

    void FixedUpdate()
{
    // シングルトン経由でジョイスティックの入力を取得
    _horizontal = JoystickInputManager.Instance.Horizontal;
    _vertical = JoystickInputManager.Instance.Vertical;

    // カメラの向きを基準にした回転を計算
    var _horizontalRotation = Quaternion.AngleAxis(Camera.main.transform.eulerAngles.y, Vector3.up);

    // 入力方向をカメラ基準で計算
    _velocity = _horizontalRotation * new Vector3(_horizontal, 0, _vertical).normalized;

    // 進行方向を計算
    _aim = _velocity;

    // 移動中のキャラクターの向き設定
    if (_aim.magnitude > 0.1f)
    {
        _playerRotation = Quaternion.LookRotation(_aim, Vector3.up);
    }

    // キャラクターの回転をスムーズに反映
    _transform.rotation = Quaternion.RotateTowards(_transform.rotation, _playerRotation, 600 * Time.deltaTime);

    // アニメーションの設定
    if (_velocity.magnitude > 0.1f)
    {
        _animator.SetBool("Walk", true);
    }
    else
    {
        _animator.SetBool("Walk", false);
    }

    // Rigidbodyを使った移動処理
    _rigidbody.velocity = _velocity * _speed;
}

    private void OnTriggerStay(Collider other)
    {
        // トリガー内にいるオブジェクトが指定したオブジェクトか確認
        if (other.gameObject == targetObjectToCheck)
        {
            // 表示したいオブジェクトを表示
            if (objectToShow != null)
            {
                objectToShow.SetActive(true);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        // トリガーから離れた場合、オブジェクトを非表示に戻す
        if (other.gameObject == targetObjectToCheck)
        {
            if (objectToShow != null)
            {
                objectToShow.SetActive(false);
            }
        }
    }
}
