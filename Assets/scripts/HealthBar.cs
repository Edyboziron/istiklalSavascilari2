using UnityEngine;
using UnityEngine.UI; // UI iþlemleri için þart

public class HealthBar : MonoBehaviour
{
    public Image fillImage; // Yeþil olan Image
    private Camera mainCam;

    void Start()
    {
        // Sahnedeki ana kamerayý bul
        mainCam = Camera.main;
    }

    // Can deðerini güncelleme fonksiyonu
    public void UpdateHealthBar(float currentHealth, float maxHealth)
    {
        // Yüzdelik hesapla (Örn: 50 / 100 = 0.5)
        fillImage.fillAmount = currentHealth / maxHealth;
    }

    void LateUpdate()
    {
        // Barýn her zaman kameraya bakmasýný saðla (Billboard etkisi)
        if (mainCam != null)
        {
            transform.LookAt(transform.position + mainCam.transform.forward);
        }
    }
}