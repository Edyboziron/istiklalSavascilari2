using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManagerB : MonoBehaviour
{
    [Header("UI Ayarlarý")]
    public GameObject gameOverPanel;

    [Header("Level Ayarlarý")]
    public int sonrakiSahneIndex = 2;

    [Header("Ekonomi Ayarlarý")]
    public int birimBasinaAltin = 50;   // Bölüm sonu hayatta kalan her asker için ödül
    public int baslangicBonusAltini = 50; // --- YENÝ: Bölüm baþladýðýnda verilecek ekstra harçlýk

    private bool oyunDevamEdiyor = false;
    private bool levelTamamlandi = false;

    void Start()
    {
        // --- DEÐÝÞÝKLÝK: Artýk altýný sýfýrlamýyoruz, üzerine ekliyoruz ---
        if (GoldManager.Instance != null)
        {
            // SetGoldTo100 yerine AddGold kullanýyoruz
            GoldManager.Instance.AddGold(baslangicBonusAltini);
            Debug.Log("Yeni bölüm baþladý! Oyuncuya baþlangýç desteði olarak " + baslangicBonusAltini + " altýn verildi.");
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
        if (oyunDevamEdiyor && !levelTamamlandi)
        {
            // --- 1. DÜÞMAN KONTROLÜ (KAZANMA DURUMU) ---
            // Sahnede hiç Enemy kalmadýysa kazan
            if (GameObject.FindGameObjectsWithTag("Enemy").Length <= 0)
            {
                levelTamamlandi = true;
                Debug.Log("Tüm düþmanlar temizlendi!");

                // Asker baþýna ödülü hesapla ve ver
                CalculateAndGiveReward();

                Debug.Log(sonrakiSahneIndex + ". Sahne yükleniyor...");
                SahneYukle();
            }

            // --- 2. PLAYER (OYUNCU) KONTROLÜ (KAYBETME DURUMU) ---
            // Sahnede hiç Player kalmadýysa kaybet
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

    // --- ÖDÜL HESAPLAMA ---
    void CalculateAndGiveReward()
    {
        GameObject[] kalanAskerler = GameObject.FindGameObjectsWithTag("Player");
        int askerSayisi = kalanAskerler.Length;

        int kazanilanAltin = askerSayisi * birimBasinaAltin;

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