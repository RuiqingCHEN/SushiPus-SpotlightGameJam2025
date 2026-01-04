using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float _speed;
    [SerializeField] private float _jumpSpeed;
    [SerializeField] private float _rotateSpeed = 20.0f;
    [SerializeField] private float _jumpButtonGracePeriod = 0.15f;
    [SerializeField] private Transform _cameraTransform;
     [SerializeField] private ParticleSystem _movementParticle;

    private PlayerInputController _playerInputController;
    private GroundController _groundController;
    private Rigidbody _rigidbody;
    private bool _jumpTriggered;

    private float? _lastGroundedTime;
    private float? _jumpButtonPressedTime;

    RespawnController respawnController; 

    [SerializeField] private float _footstepInterval = 1.0f;
    private float _footstepTimer = 0f;

    private void Awake()
    {
        _playerInputController = GetComponent<PlayerInputController>();
        _groundController = GetComponent<GroundController>();
        _rigidbody = GetComponent<Rigidbody>();

        _playerInputController.OnJumpButtonPressed += JumpButtonPressed;

        respawnController = GetComponent<RespawnController>();
    }

    private void FixedUpdate()
    {
        if (respawnController.IsDead) return;

        if (_groundController.IsGrounded)
            _lastGroundedTime = Time.time;

        Vector3 cameraForward = _cameraTransform.forward;
        Vector3 cameraRight = _cameraTransform.right;
        cameraForward.y = 0;
        cameraRight.y = 0;
        cameraForward.Normalize();
        cameraRight.Normalize();

        Vector3 moveDirection = cameraForward * _playerInputController.MovementInputVector.y + cameraRight * _playerInputController.MovementInputVector.x;

        Vector3 velocity = moveDirection * _speed;
        velocity.y = _rigidbody.linearVelocity.y;

        if (Time.time - _lastGroundedTime <= _jumpButtonGracePeriod)
        {
            if (_jumpTriggered || (Time.time - _jumpButtonPressedTime <= _jumpButtonGracePeriod))
            {
                velocity.y = _jumpSpeed;
                _jumpTriggered = false;
                _jumpButtonPressedTime = null;
                _lastGroundedTime = null;

                SoundManager.PlaySFX("Jump");
            }
        }

        _rigidbody.linearVelocity = velocity;

        if (moveDirection.sqrMagnitude > 0.01f)
        {
            Quaternion targetRotation = Quaternion.LookRotation(moveDirection);
            Quaternion smoothRotation = Quaternion.Lerp(
                _rigidbody.rotation,
                targetRotation,
                _rotateSpeed * Time.fixedDeltaTime);
                
            _rigidbody.MoveRotation(smoothRotation);

            if (_movementParticle != null && !_movementParticle.isPlaying)
            {
                _movementParticle.Play();
            }

            if (_groundController.IsGrounded)
            {
                _footstepTimer += Time.fixedDeltaTime;

                if (_footstepTimer >= _footstepInterval)
                {
                    SoundManager.PlaySFX("Footstep");
                    _footstepTimer = 0f;
                }
            }
            else
            {
                _footstepTimer = 0f;
            }
        }
        else
        {
            _footstepTimer = _footstepInterval;
            if (_movementParticle != null && _movementParticle.isPlaying)
            {
                _movementParticle.Stop();
            }
        }
    }

    private void JumpButtonPressed()
    {
        _jumpButtonPressedTime = Time.time;

        if (_groundController.IsGrounded)
        {
            _jumpTriggered = true;
        }
    }
}
