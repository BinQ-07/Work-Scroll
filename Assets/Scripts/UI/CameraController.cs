// File: _Scripts/UI/CameraController.cs
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [Header("Camera Targets")]
    [SerializeField] private Transform deskViewTarget;
    [SerializeField] private Transform phoneViewTarget;

    [Header("Settings")]
    [SerializeField] private float transitionSpeed = 10f;

    private Transform _currentTarget;

    private void Awake()
    {
        _currentTarget = deskViewTarget; // Mulai dari view meja
    }

    private void OnEnable()
    {
        InputManager.OnViewToggled += HandleViewToggle;
    }

    private void OnDisable()
    {
        InputManager.OnViewToggled -= HandleViewToggle;
    }

    // LateUpdate baik untuk pergerakan kamera agar tidak stutter
    private void LateUpdate()
    {
        // Bergerak mulus ke target
        transform.position = Vector3.Lerp(transform.position, _currentTarget.position, Time.deltaTime * transitionSpeed);
        transform.rotation = Quaternion.Slerp(transform.rotation, _currentTarget.rotation, Time.deltaTime * transitionSpeed);
    }

    private void HandleViewToggle(bool showPhone)
    {
        _currentTarget = showPhone ? phoneViewTarget : deskViewTarget;
    }
}