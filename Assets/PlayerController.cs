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

    void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _transform = GetComponent<Transform>();
        _animator = GetComponent<Animator>();

        _playerRotation = _transform.rotation;
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

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("Target"))
        {
            // GlobalObjectManager を使って別シーンのオブジェクトを表示
            if (GlobalObjectManager.Instance != null && GlobalObjectManager.Instance.sharedObject != null)
            {
                GlobalObjectManager.Instance.sharedObject.SetActive(true);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Target"))
        {
            // GlobalObjectManager を使って別シーンのオブジェクトを非表示
            if (GlobalObjectManager.Instance != null && GlobalObjectManager.Instance.sharedObject != null)
            {
                GlobalObjectManager.Instance.sharedObject.SetActive(false);
            }
        }
    }
}
