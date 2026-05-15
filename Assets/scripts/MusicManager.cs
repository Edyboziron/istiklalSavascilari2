using UnityEngine;

public class MusicManager : MonoBehaviour
{
    public static MusicManager Instance;

    private AudioSource audioSource;
    private bool isMuted = false;
    private float volume = 1f;

    private MusicVolumeSlider slider;

    private float lastVolume = 1f; // en son volume deðeri, sessize alýnmadan önceki

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            audioSource = GetComponent<AudioSource>();

            // Load saved settings
            isMuted = PlayerPrefs.GetInt("musicMuted", 0) == 1;
            volume = PlayerPrefs.GetFloat("musicVolume", 1f);

            audioSource.volume = volume;
            audioSource.mute = isMuted;
            audioSource.loop = true;
            audioSource.Play();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void ToggleMusic()
    {
        if (volume > 0f)
        {
            lastVolume = volume; // Mevcut deðeri sakla
            SetVolume(0f);       // Sessize al
        }
        else
        {
            SetVolume(lastVolume); // Önceki deðere dön
        }

        slider?.UpdateSliderUI();
        Debug.Log("Ses Togglelandý. Yeni Volume: " + volume);
    }

    public void SetVolume(float newVolume)
    {
        volume = Mathf.Clamp01(newVolume);
        audioSource.volume = volume; // Doðrudan ses seviyesini ayarla

        // Mute durumunu sadece ses seviyesi 0 olduðunda güncelle
        isMuted = volume <= 0f;
        audioSource.mute = isMuted; // AudioSource'un mute durumunu da güncelle

        PlayerPrefs.SetFloat("musicVolume", volume);
        PlayerPrefs.SetInt("musicMuted", isMuted ? 1 : 0);
        PlayerPrefs.Save();

        slider?.UpdateSliderUI();
        Debug.Log("Ses Seviyesi Ayarlandý: " + volume);
    }

    public float GetVolume()
    {
        return volume;
    }

    public bool IsMuted()
    {
        return isMuted;
    }

    public void RefreshVolume()
    {
        audioSource.volume = isMuted ? 0f : volume;
    }

    public void RegisterSlider(MusicVolumeSlider newSlider)
    {
        slider = newSlider;
        slider.UpdateSliderUI();
    }
}