using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

    [SerializeField] private float _moveSpeed = 2.0f;
    [SerializeField] private float _sprintSpeed = 4.3f;
    private float _speedChangeRate = 10f;
    private float _rotationSmoothTime = 0.12f;

    private InputReader _input;
    private CharacterController _controller;

    private float _speed;
    private float _targetRotation;
    private float _rotationVelocity;

    private GameObject _mainCamera;
    [SerializeField] private GameObject _cinemacineCameraTarget;

    private float _topClamp = 70.0f;
    private float _bottomClamp = -30.0f;

    private const float _threshold = 0.01f;

    private float _cinemacineTargetYaw;
    private float _cinemacineTargetPitch;


    private void Awake() {
        if (_mainCamera == null) {
            _mainCamera = GameObject.FindGameObjectWithTag("MainCamera");
        }
    }

    private void Start() {
        _cinemacineTargetYaw = _cinemacineCameraTarget.transform.rotation.eulerAngles.y;
        _controller = GetComponent<CharacterController>();
        _input = GetComponent<InputReader>();
    }

    private void Update() {
        Vector2 inputVector = _input.GetMovementVector();
        float targetSpeed = _input.GetSprintInput() ? _sprintSpeed : _moveSpeed;

        if (inputVector == Vector2.zero) targetSpeed = 0.0f;

        float currentHorizontalSpeed = new Vector3(_controller.velocity.x, 0f, _controller.velocity.z).magnitude;
        float speedOffset = 0.1f;
        float inputMagnitude = 1f;

        // accelerate or decelerate to target speed
        if (currentHorizontalSpeed < targetSpeed - speedOffset ||
            currentHorizontalSpeed > targetSpeed + speedOffset) {
            _speed = Mathf.Lerp(currentHorizontalSpeed, targetSpeed * inputMagnitude, Time.deltaTime * _speedChangeRate);
            _speed = Mathf.Round(_speed * 1000f) / 1000f;
        } else {
            _speed = targetSpeed;
        }

        Vector3 inputDir = new Vector3(inputVector.x, 0f, inputVector.y).normalized;
        if (inputVector != Vector2.zero) {
            _targetRotation = Mathf.Atan2(inputDir.x, inputDir.z) * Mathf.Rad2Deg +
                                _mainCamera.transform.eulerAngles.y;

            float rotation = Mathf.SmoothDampAngle(transform.eulerAngles.y, _targetRotation, ref _rotationVelocity,
                _rotationSmoothTime);

            // rotate to face input direction relative to camera position
            transform.rotation = Quaternion.Euler(0.0f, rotation, 0.0f);
        }

        Vector3 targetDir = Quaternion.Euler(0f, _targetRotation, 0f) * Vector3.forward;
        _controller.Move(targetDir.normalized * (_speed * Time.deltaTime));


    }

    private void LateUpdate() {
        // camera rotate
        Vector2 lookVector = _input.GetLookVector();
        if (lookVector.sqrMagnitude >= _threshold) {
            _cinemacineTargetYaw += lookVector.x * 1.0f;
            _cinemacineTargetPitch += lookVector.y * 1.0f;
        }

        _cinemacineTargetYaw = ClampAngle(_cinemacineTargetYaw, float.MinValue, float.MaxValue);
        _cinemacineTargetPitch = ClampAngle(_cinemacineTargetPitch, _bottomClamp, _topClamp);

        _cinemacineCameraTarget.transform.rotation = Quaternion.Euler(_cinemacineTargetPitch, _cinemacineTargetYaw, 0f);
    }

    private static float ClampAngle(float lfAngle, float lfMin, float lfMax) {
        if (lfAngle < -360f) lfAngle += 360f;
        if (lfAngle > 360f) lfAngle -= 360f;
        return Mathf.Clamp(lfAngle, lfMin, lfMax);
    }
}
