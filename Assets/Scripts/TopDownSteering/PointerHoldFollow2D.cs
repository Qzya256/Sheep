using UnityEngine;

[DisallowMultipleComponent]
public class PointerHoldFollow2D : MonoBehaviour
{
    [SerializeField] private Camera _worldCamera;
    [SerializeField] private TopDownMotor2D _motor;
    [SerializeField] private LayerMask _obstacleLayers;
    [SerializeField] private float _arrivalClearance = 0.75f;
    [SerializeField] private float _moveSpeed = 3.5f;
    [SerializeField] private float _obstacleCastRadius = 0.35f;
    [SerializeField] private float _obstacleCastDistance = 1.2f;

    private void Awake()
    {
        if (_worldCamera == null)
        {
            _worldCamera = Camera.main;
        }

        Debug.Assert(_worldCamera != null);
        Debug.Assert(_motor != null);
    }

    private void FixedUpdate()
    {
        if (!Input.GetMouseButton(0))
        {
            _motor.SetWorldVelocity(Vector2.zero);
            return;
        }

        Vector3 pointerWorld = _worldCamera.ScreenToWorldPoint(Input.mousePosition);
        pointerWorld.z = 0f;

        Vector2 position = transform.position;
        Vector2 toTarget = (Vector2)pointerWorld - position;
        float distance = toTarget.magnitude;

        Vector2 desiredVelocity = Vector2.zero;
        if (distance > _arrivalClearance)
        {
            Vector2 direction = toTarget / distance;
            float approachFactor = Mathf.Clamp01(Mathf.InverseLerp(_arrivalClearance, _arrivalClearance + 1.5f, distance));
            desiredVelocity = direction * (_moveSpeed * approachFactor);
        }

        Vector2 adjusted = ObstacleSteering2D.AdjustDirection(
            position,
            desiredVelocity,
            _obstacleCastRadius,
            _obstacleCastDistance,
            _obstacleLayers);

        _motor.SetWorldVelocity(adjusted);
        _motor.RotateTowardsWorldDirection(toTarget);
    }
}
