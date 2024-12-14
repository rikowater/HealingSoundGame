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
private float _speed = 2f;

private Vector3 _aim; // 追記
private Quaternion _playerRotation; // 追記

void Start()
{
_rigidbody = GetComponent<Rigidbody>();
_transform = GetComponent<Transform>();
_animator = GetComponent<Animator>();

_playerRotation = _transform.rotation; // 追記

}

void FixedUpdate()
{
_horizontal = Input.GetAxis("Horizontal");
_vertical = Input.GetAxis("Vertical");

var _horizontalRotation = Quaternion.AngleAxis(Camera.main.transform.eulerAngles.y, Vector3.up); // 追記

_velocity = _horizontalRotation * new Vector3(_horizontal, _rigidbody.velocity.y, _vertical).normalized; // 修正

_aim = _horizontalRotation * new Vector3(_horizontal, 0, _vertical).normalized; // 追記

if(_aim.magnitude > 0.5f) { // 以下追記
_playerRotation = Quaternion.LookRotation(_aim, Vector3.up);
}

_transform.rotation = Quaternion.RotateTowards(_transform.rotation, _playerRotation, 600 * Time.deltaTime); // 追記

if (_velocity.magnitude > 0.1f) {
_animator.SetBool("Walk", true);
} else {
_animator.SetBool("Walk", false);
}

_rigidbody.velocity = _velocity * _speed;
}
}