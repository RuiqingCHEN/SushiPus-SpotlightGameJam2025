using UnityEngine;

public class GroundController : MonoBehaviour
{
    [SerializeField] private float _groundDistanceTolerance;

    [SerializeField] private LayerMask _groundLayerMask;

    private CapsuleCollider _capsuleCollider;
    private RespawnController _respawnController;

    public bool IsGrounded { get; private set; }

    public float? DistanceToGround { get; private set; }

    private void Awake()
    {
        _capsuleCollider = GetComponent<CapsuleCollider>();
        _respawnController = GetComponent<RespawnController>();
    }

    void Update()
    {
        float sphereCastRadius = _capsuleCollider.radius - 0.1f;
        Vector3 sphereCastOrigin = transform.position + new Vector3(0, _capsuleCollider.radius, 0);

        bool isGroundBelow = Physics.SphereCast(
            sphereCastOrigin,
            sphereCastRadius,
            Vector3.down,
            out RaycastHit hitInfo,
            1000,
            _groundLayerMask,
            QueryTriggerInteraction.Ignore);

        if (isGroundBelow)
        {
            DistanceToGround = transform.position.y - hitInfo.point.y;
        }
        else
        {
            DistanceToGround = null;
        }

        IsGrounded = isGroundBelow && DistanceToGround <= _groundDistanceTolerance;

        bool isAnyGroundBelow = Physics.SphereCast(
            sphereCastOrigin,
            sphereCastRadius,
            Vector3.down,
            out RaycastHit anyGroundHit,
            1000,
            ~0,
            QueryTriggerInteraction.Ignore);

        if (isAnyGroundBelow)
        {
            float distanceToAnyGround = transform.position.y - anyGroundHit.point.y;

            if (distanceToAnyGround <= _groundDistanceTolerance && !IsGrounded)
            {
                _respawnController.Die();
            }
        }
    }
    
    public LayerMask GetGroundLayerMask()
    {
        return _groundLayerMask;
    }
}
