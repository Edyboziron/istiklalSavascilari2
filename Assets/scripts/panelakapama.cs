using UnityEngine;
using System.Collections;

public class PanelKapatma : MonoBehaviour
{
    [Header("Panel Ayarlarý")]
    public GameObject hedefPanel;
    public float otomatikKapanmaSuresi = 70f;
    public bool oyunBaslayincaOtomatikAc = true;

    [Header("Ses Ayarlarý")]
    public AudioSource muzikKaynagi;

    [Header("Geri Sayým Durumu")]
    [SerializeField]
    private float kalanSure;

    private Coroutine otomatikKapatmaCoroutine;

    // --- DEÐÝÞÝKLÝK BURADA ---
    // 'static' olduðu için sahne deðiþse de unutmaz.
    // Oyun kapatýlýp açýlýnca otomatik olarak 'false' olur.
    private static bool buOturumdaGosterildi = false;

    private void Start()
    {
        // Müzik kaynaðýný otomatik bul
        if (muzikKaynagi == null)
        {
            GameObject muzikObjesi = GameObject.FindGameObjectWithTag("Music");
            if (muzikObjesi != null)
                muzikKaynagi = muzikObjesi.GetComponent<AudioSource>();
        }

        // KONTROL: Bu oyun oturumunda daha önce açýldý mý?
        if (oyunBaslayincaOtomatikAc && !buOturumdaGosterildi)
        {
            // Henüz gösterilmemiþ (ilk açýlýþ), o zaman çalýþtýr.
            PaneliAcVeZamanlayiciyiBaslat();

            // Ve hafýzaya at: "Bu oturum için hakkýný kullandý."
            buOturumdaGosterildi = true;
        }
        else
        {
            // Zaten gösterilmiþ, o yüzden kapalý kalsýn.
            if (hedefPanel != null)
            {
                hedefPanel.SetActive(false);
            }
        }
    }

    // BUTONSAL: Bu fonksiyonu Unity'de Butonun OnClick olayýna sürükle.
    public void PaneliKapat()
    {
        DurdurVeKapat();
    }

    public void PaneliAcVeZamanlayiciyiBaslat()
    {
        if (hedefPanel != null)
        {
            if (otomatikKapatmaCoroutine != null) StopCoroutine(otomatikKapatmaCoroutine);

            hedefPanel.SetActive(true);

            // Panel açýldýðýnda zaman normal akýyor
            Time.timeScale = 1f;

            if (muzikKaynagi != null)
            {
                muzikKaynagi.mute = true;
            }

            kalanSure = otomatikKapanmaSuresi;
            otomatikKapatmaCoroutine = StartCoroutine(OtomatikKapatmaCoroutine(otomatikKapanmaSuresi));
        }
    }

    // MERKEZÝ FONKSÝYON
    private void DurdurVeKapat()
    {
        if (hedefPanel != null)
        {
            if (otomatikKapatmaCoroutine != null)
            {
                StopCoroutine(otomatikKapatmaCoroutine);
                otomatikKapatmaCoroutine = null;
            }

            kalanSure = 0f;

            Debug.Log("zaman durduruldu");
            Time.timeScale = 0f;
            hedefPanel.SetActive(false);

            if (muzikKaynagi != null)
            {
                muzikKaynagi.mute = false;
            }
        }
    }

    // ZAMANSAL
    IEnumerator OtomatikKapatmaCoroutine(float gecikmeSuresi)
    {
        float baslangicZamani = Time.time;
        float bitisZamani = baslangicZamani + gecikmeSuresi;

        while (Time.time < bitisZamani)
        {
            kalanSure = bitisZamani - Time.time;
            yield return null;
        }

        DurdurVeKapat();
    }

    // Test amaçlý manuel sýfýrlama (Ýstersen kullanabilirsin)
    [ContextMenu("Oturum Hafýzasýný Sýfýrla")]
    public void HafizayiSifirla()
    {
        buOturumdaGosterildi = false;
        Debug.Log("Hafýza sýfýrlandý. Sahne yeniden yüklenirse panel tekrar açýlacak.");
    }
}