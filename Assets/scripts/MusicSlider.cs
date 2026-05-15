using UnityEngine;
using UnityEngine.UI;

public class MusicVolumeSlider : MonoBehaviour
{
    [Header("UI Components")]
    public Slider volumeSlider;

    private void Start()
    {
        if (MusicManager.Instance != null)
        {
            // Slider'ý MusicManager'a kaydet
            MusicManager.Instance.RegisterSlider(this);

            // Kayýtlý sesi slider'a uygula
            volumeSlider.value = MusicManager.Instance.GetVolume();

            // Deðer deðiþtiðinde ses seviyesini güncelle
            volumeSlider.onValueChanged.AddListener(OnSliderValueChanged);
        }
        else
        {
            Debug.LogError("MusicManager.Instance null!");
        }

        // Slider bileþeninin tam sayý olmamasý için kontrol
        if (volumeSlider != null && volumeSlider.wholeNumbers)
        {
            Debug.LogWarning(gameObject.name + " üzerindeki Slider bileþeninin 'Whole Numbers' seçeneði iþaretli. Ses seviyesi kontrolü için bu seçeneði kaldýrýn.");
        }
    }

    private void OnSliderValueChanged(float value)
    {
        // MusicManager üzerinden sesi ayarla
        MusicManager.Instance?.SetVolume(value);
    }

    public void UpdateSliderUI()
    {
        if (MusicManager.Instance != null && volumeSlider != null)
        {
            volumeSlider.value = MusicManager.Instance.GetVolume();
        }
    }
    public Text volumeText; // veya TMP_Text

    private void On_Slider_Value_Changed(float value)
    {
        MusicManager.Instance?.SetVolume(value);

        if (volumeText != null)
        {
            volumeText.text = Mathf.RoundToInt(value * 100) + "%";
        }
    }

}