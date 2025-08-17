// File: _Scripts/Player/PlayerController.cs
using UnityEngine;

[RequireComponent(typeof(PlayerStats))]
public class PlayerController : MonoBehaviour
{
    private PlayerStats _playerStats;
    private bool _isPhoneView = false;

    private void Awake()
    {
        _playerStats = GetComponent<PlayerStats>();
    }

    // Berlangganan event saat script aktif
    private void OnEnable()
    {
        InputManager.OnWorkActionPerformed += HandleWork;
        InputManager.OnViewToggled += HandleViewToggle;
    }

    // Berhenti berlangganan saat script non-aktif untuk mencegah error
    private void OnDisable()
    {
        InputManager.OnWorkActionPerformed -= HandleWork;
        InputManager.OnViewToggled -= HandleViewToggle;
    }

    private void HandleWork()
    {
        _playerStats.DoWork(Time.deltaTime);
    }

    private void HandleViewToggle(bool phoneView)
    {
        _isPhoneView = phoneView;
    }

    // Doomscrolling terjadi otomatis jika view HP aktif
    private void Update()
    {
        if (_isPhoneView && GameManager.Instance.CurrentState == GameState.Playing)
        {
            _playerStats.DoScroll(Time.deltaTime);
        }
    }
}