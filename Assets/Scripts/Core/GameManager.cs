// File: _Scripts/Core/GameManager.cs
using UnityEngine;
using System;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    // Singleton pattern untuk akses mudah dari script lain
    public static GameManager Instance { get; private set; }

    public GameState CurrentState { get; private set; }
    public static event Action<GameState> OnStateChanged;

    [Header("Level Settings")]
    [SerializeField] private float timeLimit = 180f; // 3 menit

    private float _currentTime;

    private void Awake()
    {
        // Setup Singleton
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }
    }

    private void Start()
    {
        StartNewGame();
    }

    public void StartNewGame()
    {
        Time.timeScale = 1f;
        _currentTime = timeLimit;
        UpdateState(GameState.Playing);
    }

    private void Update()
    {
        if (CurrentState != GameState.Playing) return;

        // Hitung mundur waktu
        _currentTime -= Time.deltaTime;
        if (_currentTime <= 0)
        {
            _currentTime = 0;
            UpdateState(GameState.Lost);
        }
    }

    public void UpdateState(GameState newState)
    {
        if (CurrentState == newState) return;

        CurrentState = newState;
        OnStateChanged?.Invoke(newState); // Kirim event saat state berubah (Prinsip Open/Closed)

        // Contoh: Membekukan game saat menang/kalah
        if (newState == GameState.Won || newState == GameState.Lost)
        {
            Time.timeScale = 0f; // Hentikan semua pergerakan dan timer
        }
        else
        {
            Time.timeScale = 1f;
        }
    }

    // Metode ini bisa dipanggil dari mana saja untuk mendapatkan sisa waktu
    public float GetCurrentTime() => _currentTime;

    public void RestartGame()
    {
        // PENTING: Kembalikan Time.timeScale ke 1 SEBELUM me-load scene baru.
        // Jika tidak, scene yang baru di-load akan tetap dalam keadaan beku (Time.timeScale = 0).
        Time.timeScale = 1f;

        // Me-load ulang scene yang sedang aktif saat ini.
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}