using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [Header("UI Ayarlarý")]
    public GameObject gameOverPanel;

    [Header("Level Ayarlarý")]
    public int sonrakiSahneIndex = 2;

    [Header("Ödül Ayarlarý")]
    public int birimBasinaAltin = 50; // Inspector'dan her asker için ne kadar altýn vereceðini buradan ayarla

    private bool oyunDevamEdiyor = false;
    private bool levelTamamlandi = false; // Kazanma kodunun sadece bir kez çalýþmasý için kontrol

    void Start()
    {
        // --- DEÐÝÞÝKLÝK: Altýn oyun baþlar baþlamaz sýfýrlanýyor ---
        // DÝKKAT: Eðer bölüm sonu kazandýðýn altýnýn bir sonraki bölümde silinmesini istemiyorsan
        // buradaki SetGoldTo100 satýrýný silmelisin veya yorum satýrý yapmalýsýn.
        if (GoldManager.Instance != null)
        {
            GoldManager.Instance.SetGoldTo100();
        }
        else
        {
            Debug.LogWarning("GoldManager sahnede bulunamadý!");
        }
        // -------------------------------------------------------------

        Time.timeScale = 0f;

        if (gameOverPanel != null)
        {
            gameOverPanel.SetActive(false);
        }
    }

    void Update()
    {
        if (oyunDevamEdiyor && !levelTamamlandi) // Level tamamlandýysa buraya bir daha girme
        {
            // --- 1. DÜÞMAN KONTROLÜ (KAZANMA DURUMU) ---
            int dusmanSayisi = GameObject.FindGameObjectsWithTag("Enemy").Length;

            if (dusmanSayisi <= 0)
            {
                levelTamamlandi = true; // Kodu kilitliyoruz ki sürekli altýn eklemesin
                Debug.Log("Tüm düþmanlar temizlendi!");

                // --- YENÝ EKLENEN KISIM: ÖDÜL HESAPLAMA ---
                CalculateAndGiveReward();
                // -----------------------------------------

                Debug.Log(sonrakiSahneIndex + ". Sahne yükleniyor...");
                SahneYukle();
            }

            // --- 2. PLAYER (OYUNCU) KONTROLÜ (KAYBETME DURUMU) ---
            if (GameObject.FindGameObjectWithTag("Player") == null)
            {
                Debug.Log("Oyuncu öldü! Panel açýlýyor...");

                if (gameOverPanel != null)
                {
                    gameOverPanel.SetActive(true);
                }

                oyunDevamEdiyor = false;
            }
        }
    }

    // --- ÖDÜL HESAPLAMA FONKSÝYONU ---
    void CalculateAndGiveReward()
    {
        // Sahnede kalan tüm dost birimleri bul
        GameObject[] kalanAskerler = GameObject.FindGameObjectsWithTag("Player");
        int askerSayisi = kalanAskerler.Length;

        // Toplam ödülü hesapla
        int kazanilanAltin = askerSayisi * birimBasinaAltin;

        // GoldManager'a ekle
        if (GoldManager.Instance != null)
        {
            GoldManager.Instance.AddGold(kazanilanAltin);
            Debug.Log("Level Bitti! Kalan Asker: " + askerSayisi + " | Kazanýlan Altýn: " + kazanilanAltin);
        }
    }

    public void OyunuBaslat(GameObject basilanButon)
    {
        Time.timeScale = 1f;
        oyunDevamEdiyor = true;
        Destroy(basilanButon);
    }

    void SahneYukle()
    {
        SceneManager.LoadScene(sonrakiSahneIndex);
    }
}