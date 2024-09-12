using UnityEngine;

public class SmoothCameraFollow : MonoBehaviour
{
    [SerializeField] private float smoothSpeed = 0.125f; 
    
    private Transform _player; 
    private Vector3 _initialOffset; 
    private Quaternion _initialRotation; 

    void Start()
    {
        _player = GameObject.FindWithTag("Player").transform;
            _initialOffset = transform.position - _player.position;
            _initialRotation = transform.rotation;
    }

    void LateUpdate()
    {
        if (_player == null) return; 
        Vector3 desiredPosition = _player.position + _initialOffset;
        transform.position = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);
        transform.rotation = Quaternion.Slerp(transform.rotation, _initialRotation, smoothSpeed);
    }
}