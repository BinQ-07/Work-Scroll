// File: _Scripts/Input/InputManager.cs
using UnityEngine;
using System;

public class InputManager : MonoBehaviour
{
    public static event Action OnWorkActionPerformed;
    public static event Action<bool> OnViewToggled; // true = phone, false = desk

    [Tooltip("Area di bawah layar (dalam persen) untuk trigger view HP")]
    [SerializeField, Range(0f, 0.2f)] private float phoneViewTriggerArea = 0.1f;

    private bool _isPhoneView = false;

    void Update()
    {
        // Hanya proses input jika game sedang berjalan
        if (GameManager.Instance.CurrentState != GameState.Playing) return;

        // Cek posisi mouse untuk ganti view
        bool shouldBePhoneView = Input.mousePosition.y < Screen.height * phoneViewTriggerArea;
        if (shouldBePhoneView != _isPhoneView)
        {
            _isPhoneView = shouldBePhoneView;
            OnViewToggled?.Invoke(_isPhoneView); // Kirim event
        }

        // Cek klik kiri mouse untuk bekerja (hanya saat di view meja)
        if (!_isPhoneView && Input.GetMouseButton(0))
        {
            OnWorkActionPerformed?.Invoke(); // Kirim event
        }
    }
}