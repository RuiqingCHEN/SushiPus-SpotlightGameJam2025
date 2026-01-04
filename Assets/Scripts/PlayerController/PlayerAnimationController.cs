using UnityEngine;

public class PlayerAnimationController : MonoBehaviour
{
    [SerializeField] private Animator _animator;
    [SerializeField] private float _transitionDuration = 0.15f;

    private PlayerInputController _playerInputController;
    private GroundController _groundController;
    private Rigidbody _rigidbody;
    
    private bool _wasGrounded;
    private bool _isJumping;
    private int _currentStateHash;
    private float _jumpLandStartTime;
    private float _jumpStartTime;

    // Animation state hashes
    private static readonly int Idle = Animator.StringToHash("Idle");
    private static readonly int Walk = Animator.StringToHash("Walk");
    private static readonly int JumpStart = Animator.StringToHash("JumpStart");
    private static readonly int InAir = Animator.StringToHash("InAir");
    private static readonly int JumpLand = Animator.StringToHash("JumpLand");

    private void Awake()
    {
        _playerInputController = GetComponent<PlayerInputController>();
        _groundController = GetComponent<GroundController>();
        _rigidbody = GetComponent<Rigidbody>();

        _playerInputController.OnJumpButtonPressed += OnJump;
        
        _jumpLandStartTime = -1000f; 
        _jumpStartTime = -1000f;
    }

    private void Update()
    {
        bool isGrounded = _groundController.IsGrounded;
        bool isMoving = _playerInputController.MovementInputVector.sqrMagnitude > 0.01f;
        float verticalVelocity = _rigidbody.linearVelocity.y;
        if (Time.time - _jumpStartTime < 0.625f || Time.time - _jumpLandStartTime < 0.625f)
        {
            return;
        }

        if (isGrounded && !_wasGrounded)
        {
            PlayAnimation(JumpLand);
            _jumpLandStartTime = Time.time;
            _isJumping = false;
        }
        else if (isGrounded)
        {
            if (isMoving)
            {
                PlayAnimation(Walk);
            }
            else
            {
                PlayAnimation(Idle);
            }
        }
        else
        {
            if (!_isJumping && verticalVelocity < -0.5f)
            {
                PlayAnimation(InAir);
            }
        }

        _wasGrounded = isGrounded;
    }

    private void PlayAnimation(int stateHash)
    {
        if (_currentStateHash != stateHash)
        {
            _animator.CrossFadeInFixedTime(stateHash, _transitionDuration, 0);
            _currentStateHash = stateHash;
        }
    }

    private void OnJump()
    {
        if (_groundController.IsGrounded)
        {
            _isJumping = true;
            PlayAnimation(JumpStart);
            _jumpStartTime = Time.time;
            Invoke(nameof(TransitionToInAir), 0.5f);
        }
    }

    private void TransitionToInAir()
    {
        if (_isJumping)
        {
            PlayAnimation(InAir);
        }
    }

    private void OnDestroy()
    {
        if (_playerInputController != null)
        {
            _playerInputController.OnJumpButtonPressed -= OnJump;
        }
    }
}