// File: _Scripts/Audio/AudioManager.cs
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [Header("Audio Sources")]
    [Tooltip("AudioSource untuk suara mengetik.")]
    [SerializeField] private AudioSource typingAudioSource;

    [Tooltip("AudioSource untuk suara doomscrolling.")]
    [SerializeField] private AudioSource scrollingAudioSource;

    [Header("Audio Clips")]
    [SerializeField] private AudioClip typingClip;
    [SerializeField] private AudioClip laughingClip;

    // Flag untuk menangani event yang dipanggil setiap frame
    private bool _isWorkActionFrame = false;

    private void OnEnable()
    {
        // Berlangganan event dari InputManager
        InputManager.OnWorkActionPerformed += HandleWorkAction;
        InputManager.OnViewToggled += HandleViewToggle;

        SetupAudioSources();
    }

    private void OnDisable()
    {
        // Selalu berhenti berlangganan untuk mencegah memory leak
        InputManager.OnWorkActionPerformed -= HandleWorkAction;
        InputManager.OnViewToggled -= HandleViewToggle;
    }

    private void SetupAudioSources()
    {
        // Pastikan AudioSource sudah di-assign clipnya dan di-set untuk looping
        if (typingAudioSource != null)
        {
            typingAudioSource.clip = typingClip;
            typingAudioSource.loop = true;
        }

        if (scrollingAudioSource != null)
        {
            scrollingAudioSource.clip = laughingClip;
            scrollingAudioSource.loop = true;
        }
    }

    // Metode ini dipanggil saat pemain beralih view
    private void HandleViewToggle(bool isPhoneView)
    {
        if (isPhoneView)
        {
            // Jika pindah ke view HP, hentikan suara mengetik dan mulai suara tertawa
            if (typingAudioSource.isPlaying)
            {
                typingAudioSource.Stop();
            }

            if (!scrollingAudioSource.isPlaying)
            {
                scrollingAudioSource.Play();
            }
        }
        else
        {
            // Jika kembali ke view meja, hentikan suara tertawa
            if (scrollingAudioSource.isPlaying)
            {
                scrollingAudioSource.Stop();
            }
        }
    }

    // Metode ini dipanggil SETIAP FRAME selama pemain menahan tombol kerja
    private void HandleWorkAction()
    {
        // Set flag bahwa aksi kerja sedang terjadi di frame ini
        _isWorkActionFrame = true;

        // Jika suara mengetik belum berputar, mulai putar
        if (!typingAudioSource.isPlaying)
        {
            typingAudioSource.Play();
        }
    }

    // LateUpdate dijalankan setelah semua Update() selesai
    private void LateUpdate()
    {
        // Cek kondisi setelah semua event frame ini selesai
        // Jika flag kerja tidak di-set di frame ini, TAPI suara mengetik masih berputar, maka hentikan.
        if (!_isWorkActionFrame && typingAudioSource.isPlaying)
        {
            typingAudioSource.Stop();
        }

        // Reset flag untuk frame berikutnya
        _isWorkActionFrame = false;
    }
}