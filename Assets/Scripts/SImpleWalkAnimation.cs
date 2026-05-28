using UnityEngine;

public class SImpleWalkAnimation : MonoBehaviour
{
    [SerializeField] private SpriteRenderer _spriteRenderer;
    [SerializeField] private Rigidbody2D _rigidbody2D;

    [SerializeField] private Sprite _frameA;
    [SerializeField] private Sprite _frameB;

    [SerializeField] private float _stopSpeed = 0.05f;
    [SerializeField] private float _walkAnimSpeed = 4f;

    private float _timer;
    private bool _showA = true;

    private void Update()
    {
        float speed = _rigidbody2D.linearVelocity.magnitude;
        if (speed < _stopSpeed)
        {
            _spriteRenderer.sprite = _frameA;
            _showA = true;
            _timer = 0f;
            return;
        }

        float delay = 1f / (speed * _walkAnimSpeed);
        _timer += Time.deltaTime;
        if (_timer < delay)
            return;

        _timer = 0f;
        _showA = !_showA;
        _spriteRenderer.sprite = _showA ? _frameA : _frameB;
    }
}
