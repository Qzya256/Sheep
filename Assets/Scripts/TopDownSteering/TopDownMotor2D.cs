using UnityEngine;

[DisallowMultipleComponent]
[RequireComponent(typeof(Rigidbody2D))]
public class TopDownMotor2D : MonoBehaviour
{
    [SerializeField] private Rigidbody2D _rigidbody2D;
    [SerializeField] private float _maxSpeed = 4f;
    [SerializeField] private float _velocitySmoothTime = 0.1f;
    [SerializeField] private float _rotationSmoothTime = 0.09f;
    [SerializeField] private float _rotationDegreesPerSecond = 540f;

    private Vector2 _velocitySmoothDerivative;
    private float _rotationAngleSmoothVelocity;

    private void Awake()
    {
        if (_rigidbody2D == null)
        {
            _rigidbody2D = GetComponent<Rigidbody2D>();
        }

        Debug.Assert(_rigidbody2D != null);
        Debug.Assert(_rigidbody2D.bodyType == RigidbodyType2D.Dynamic);
    }

    public void SetWorldVelocity(Vector2 worldVelocity)
    {
        Vector2 target = Vector2.ClampMagnitude(worldVelocity, _maxSpeed);
        Vector2 current = _rigidbody2D.linearVelocity;
        float smoothTime = Mathf.Max(0.0001f, _velocitySmoothTime);
        Vector2 smoothed = Vector2.SmoothDamp(
            current,
            target,
            ref _velocitySmoothDerivative,
            smoothTime,
            Mathf.Max(_maxSpeed, target.magnitude) * 3f,
            Time.fixedDeltaTime);
        _rigidbody2D.linearVelocity = smoothed;
    }

    public void RotateTowardsWorldDirection(Vector2 worldDirection)
    {
        if (worldDirection.sqrMagnitude < 1e-6f)
        {
            return;
        }

        float targetAngleDegrees = Mathf.Atan2(worldDirection.y, worldDirection.x) * Mathf.Rad2Deg;
        float currentZ = transform.eulerAngles.z;
        float rotationSmooth = Mathf.Max(0.0001f, _rotationSmoothTime);
        float nextZ = Mathf.SmoothDampAngle(
            currentZ,
            targetAngleDegrees,
            ref _rotationAngleSmoothVelocity,
            rotationSmooth,
            _rotationDegreesPerSecond,
            Time.fixedDeltaTime);
        transform.rotation = Quaternion.Euler(0f, 0f, nextZ);
    }
}
