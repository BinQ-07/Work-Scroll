// File: _Scripts/UI/UIManager.cs
using UnityEngine;
using UnityEngine.UI;
using TMPro; // Pastikan Anda sudah import TextMeshPro

public class UIManager : MonoBehaviour
{
    [Header("UI Elements")]
    [SerializeField] private Slider progressBar;
    [SerializeField] private Slider moodBar;
    [SerializeField] private TextMeshProUGUI timerText;
    [SerializeField] private GameObject winScreen;
    [SerializeField] private GameObject loseScreen;
    [SerializeField] private GameObject phoneScreenAnimation; // Animasi HP

    private void OnEnable()
    {
        // Dengarkan event dari stats dan game manager
        PlayerStats.OnStatsUpdated += UpdateStatsUI;
        InputManager.OnViewToggled += TogglePhoneScreen;
        GameManager.OnStateChanged += HandleGameStateChange;
    }

    private void OnDisable()
    {
        PlayerStats.OnStatsUpdated -= UpdateStatsUI;
        InputManager.OnViewToggled -= TogglePhoneScreen;
        GameManager.OnStateChanged -= HandleGameStateChange;
    }

    private void Update()
    {
        // Selalu update timer
        if (GameManager.Instance.CurrentState == GameState.Playing)
        {
            float time = GameManager.Instance.GetCurrentTime();
            int minutes = Mathf.FloorToInt(time / 60);
            int seconds = Mathf.FloorToInt(time % 60);
            timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
        }
    }

    private void UpdateStatsUI(float progress, float mood)
    {
        progressBar.value = progress;
        moodBar.value = mood;
    }

    private void TogglePhoneScreen(bool show)
    {
        phoneScreenAnimation.SetActive(show);
    }

    private void HandleGameStateChange(GameState newState)
    {
        winScreen.SetActive(newState == GameState.Won);
        loseScreen.SetActive(newState == GameState.Lost);
    }
}