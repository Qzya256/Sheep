using UnityEngine;

[DisallowMultipleComponent]
public class PanicFleeFromTransform2D : MonoBehaviour
{
    private const int OverlapBufferCapacity = 16;

    [SerializeField] private Transform _threatOriginTransform;
    [SerializeField] private TopDownMotor2D _motor;
    [SerializeField] private Rigidbody2D _rigidbody2D;
    [SerializeField] private LayerMask _obstacleLayers;
    [SerializeField] private LayerMask _neighborLayers;
    [SerializeField] private float _threatRadius = 3f;
    [SerializeField] private float _fleeSpeed = 3f;
    [SerializeField] private float _obstacleCastRadius = 0.35f;
    [SerializeField] private float _obstacleCastDistance = 1f;
    [SerializeField] private float _separationRadius = 0.65f;
    [SerializeField] private float _separationMaxPush = 1.1f;
    [SerializeField] private float _linearDamping = 0.85f;
    [SerializeField] private float _steeringSmoothTime = 0.085f;

    private readonly Collider2D[] _overlapBuffer = new Collider2D[OverlapBufferCapacity];
    private ContactFilter2D _neighborContactFilter;
    private Vector2 _smoothedSteeringVelocity;
    private Vector2 _steeringSmoothDerivative;

    private void Awake()
    {
        if (_rigidbody2D == null)
        {
            _rigidbody2D = GetComponent<Rigidbody2D>();
        }

        Debug.Assert(_threatOriginTransform != null);
        Debug.Assert(_motor != null);
        Debug.Assert(_rigidbody2D != null);

        _rigidbody2D.linearDamping = _linearDamping;
        _smoothedSteeringVelocity = _rigidbody2D.linearVelocity;

        _neighborContactFilter = new ContactFilter2D();
        _neighborContactFilter.useLayerMask = true;
        _neighborContactFilter.layerMask = _neighborLayers;
        _neighborContactFilter.useTriggers = false;
    }

    private void FixedUpdate()
    {
        Vector2 position = transform.position;
        Vector2 desiredVelocity = Vector2.zero;

        Vector2 awayFromThreat = position - (Vector2)_threatOriginTransform.position;
        float distanceToThreat = awayFromThreat.magnitude;
        if (distanceToThreat > 1e-4f && distanceToThreat < _threatRadius)
        {
            Vector2 fleeDirection = awayFromThreat / distanceToThreat;
            float urgency = 1f - Mathf.Clamp01(distanceToThreat / _threatRadius);
            desiredVelocity = fleeDirection * (_fleeSpeed * urgency);
        }

        desiredVelocity += GetSeparationVector(position);

        Vector2 adjusted = ObstacleSteering2D.AdjustDirection(
            position,
            desiredVelocity,
            _obstacleCastRadius,
            _obstacleCastDistance,
            _obstacleLayers);

        float steeringSmooth = Mathf.Max(0.0001f, _steeringSmoothTime);
        float steeringMaxSpeed = _fleeSpeed + _separationMaxPush + 2f;
        _smoothedSteeringVelocity = Vector2.SmoothDamp(
            _smoothedSteeringVelocity,
            adjusted,
            ref _steeringSmoothDerivative,
            steeringSmooth,
            steeringMaxSpeed,
            Time.fixedDeltaTime);

        _motor.SetWorldVelocity(_smoothedSteeringVelocity);

        if (_smoothedSteeringVelocity.sqrMagnitude > 1e-4f)
        {
            _motor.RotateTowardsWorldDirection(_smoothedSteeringVelocity);
        }
    }

    private Vector2 GetSeparationVector(Vector2 position)
    {
        int count = Physics2D.OverlapCircle(position, _separationRadius, _neighborContactFilter, _overlapBuffer);
        Vector2 sum = Vector2.zero;
        for (int index = 0; index < count; index++)
        {
            Collider2D other = _overlapBuffer[index];
            if (other.attachedRigidbody == _rigidbody2D)
            {
                continue;
            }

            Vector2 otherPosition = other.transform.position;
            Vector2 away = position - otherPosition;
            float distance = away.magnitude;
            if (distance < 1e-4f)
            {
                continue;
            }

            float overlap = Mathf.Max(0f, _separationRadius - distance);
            sum += away.normalized * overlap;
        }

        return Vector2.ClampMagnitude(sum, _separationMaxPush);
    }
}
