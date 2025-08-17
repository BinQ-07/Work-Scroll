// File: _Scripts/Player/PlayerStats.cs
using UnityEngine;
using System;

public class PlayerStats : MonoBehaviour
{
    public static event Action<float, float> OnStatsUpdated; // progress (0-1), mood (0-1)

    [Header("Stats Settings")]
    [SerializeField] private float workRate = 5f;       // Poin progress per detik
    [SerializeField] private float moodDrainRate = 10f; // Poin mood hilang per detik
    [SerializeField] private float moodGainRate = 20f;  // Poin mood pulih per detik

    private float _currentProgress = 0f;
    private float _currentMood = 40f;

    private void Start()
    {
        // Kirim status awal
        BroadcastStats();
    }

    public void DoWork(float deltaTime)
    {
        // Kecepatan kerja masih dipengaruhi mood
        // Jika mood 0, kecepatan kerja hanya 25% dari normal. Ini menjadi penalti utama dari mood rendah.
        float moodModifier = Mathf.Lerp(0.25f, 1f, _currentMood / 100f);

        _currentProgress += workRate * moodModifier * deltaTime;
        _currentMood -= moodDrainRate * deltaTime;

        CheckWinCondition(); // Hanya cek kondisi menang
        BroadcastStats();
    }

    public void DoScroll(float deltaTime)
    {
        _currentMood += moodGainRate * deltaTime;
        // Tidak perlu cek apa-apa di sini, karena scrolling tidak bisa membuat menang/kalah
        BroadcastStats();
    }

    // Nama metode diubah agar lebih jelas
    private void CheckWinCondition()
    {
        _currentProgress = Mathf.Clamp(_currentProgress, 0f, 100f);
        _currentMood = Mathf.Clamp(_currentMood, 0f, 100f);

        // Hanya cek jika progress sudah 100%
        if (_currentProgress >= 100f)
        {
            // Panggil GameManager untuk menyatakan kemenangan
            if (GameManager.Instance.CurrentState == GameState.Playing)
            {
                GameManager.Instance.UpdateState(GameState.Won);
            }
        }
    }

    private void BroadcastStats()
    {
        // Kirim nilai dalam format 0-1 untuk UI
        OnStatsUpdated?.Invoke(_currentProgress / 100f, _currentMood / 100f);
    }
}